namespace CarCare_Companion.Core.Services;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

using CarCare_Companion.Core.Contracts;
using CarCare_Companion.Core.Models.Identity;
using CarCare_Companion.Infrastructure.Data.Models.Identity;
using CarCare_Companion.Common;

/// <summary>
/// The IdentityService is responsible for all the operations regarding the user-related actions
/// </summary>
public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> userManager;
    private readonly RoleManager<ApplicationRole> roleManager;
    private readonly IConfiguration configuration;

    public IdentityService(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager , IConfiguration configuration)
    {
        this.userManager = userManager;
        this.configuration = configuration;
        this.roleManager = roleManager;
    }

    /// <summary>
    /// Checks if an user exists.
    /// </summary>
    /// <param name="username">The user username used for the searching.</param>
    /// <returns>A boolean if the user exists.</returns>
    public async Task<bool> DoesUserExistAsync(string username)
    {
        var userExists = await userManager.FindByNameAsync(username);

        if (userExists == null)
        {
            return false;
        }

        return true;
    }


    /// <summary>
    /// Logs in the user.
    /// </summary>
    /// <param name="inputModel">The input data containg the user email and password.</param>
    /// <returns>An object containing the user email,id and generated JWT token.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the user with the specified Email is not found.</exception>
    /// <exception cref="ArgumentException">Thrown when the user password is invalid.</exception>
    public async Task<AuthDataModel> LoginAsync(LoginRequestModel inputModel)
    {
        var user = await userManager.FindByNameAsync(inputModel.Email);

        if (user == null)
        {
            throw new ArgumentNullException("User does not exist");
        }

        bool isPasswordValid = await userManager.CheckPasswordAsync(user, inputModel.Password);

        if (!isPasswordValid)
        {
            throw new ArgumentException("Invalid password");
        }
        var userRoles = await userManager.GetRolesAsync(user);

        if (userRoles.Count == 0)
        {
            userRoles.Add("User");
        }

        var authClaims = GenerateUserAuthClaims(user,userRoles);

        var token = GenerateToken(authClaims);

        return new AuthDataModel
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Email = user.Email,
            Id = user.Id.ToString(),
            Role = userRoles.First().ToString()
        };
       
    }

    /// <summary>
    /// Creates a new user.
    /// </summary>
    /// <param name="inputModel">The input model containing the user first name, 
    /// last name, email, password and confirm password</param>
    /// <returns>The created users data - email, id and JWT token</returns>
    /// <exception cref="Exception">Thrown when the user creating is not successful</exception>
    public async Task<AuthDataModel> RegisterAsync(RegisterRequestModel inputModel)
    {
     
        ApplicationUser user = new ApplicationUser()
        {
            FirstName = inputModel.FirstName,
            LastName = inputModel.LastName,
            UserName = inputModel.Email,
            Email = inputModel.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            CreatedOn = DateTime.UtcNow,
        };

        var result = await userManager.CreateAsync(user, inputModel.Password);

        if (!result.Succeeded)
        {
            throw new Exception(result.Errors.First().ToString());
        }

        var userRoles = await userManager.GetRolesAsync(user);

        var authClaims = GenerateUserAuthClaims(user, userRoles);

        var token = GenerateToken(authClaims);

        return new AuthDataModel
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Email = user.Email,
            Id = user.Id.ToString(),
            Role = "User"
        };

    }

    

    /// <summary>
    /// Generates the authentication claims of the user for the JWT token
    /// </summary>
    /// <param name="user">The target user</param>
    /// <param name="userRoles">The user roles</param>
    /// <returns>Collection of Claims to be used in the JWT token generator</returns>
    private List<Claim> GenerateUserAuthClaims(ApplicationUser user, IList<string> userRoles)
    {
     
        var authClaims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

        foreach (var userRole in userRoles)
        {
            authClaims.Add(new Claim(ClaimTypes.Role, userRole));
        }

        return authClaims;
    }

    /// <summary>
    /// Generates a JWT token.
    /// </summary>
    /// <param name="authClaims">The user authorization claims</param>
    /// <returns>A JWT token containing an issuer, audience, expiration date and user claims</returns>
    private JwtSecurityToken GenerateToken(List<Claim> authClaims)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]));

        var token = new JwtSecurityToken(
            issuer: configuration["JWT:Issuer"],
            audience: configuration["JWT:Audience"],
            expires: DateTime.Now.AddHours(GlobalConstants.JWTTokenExpirationTime),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

        return token;
    }
}
