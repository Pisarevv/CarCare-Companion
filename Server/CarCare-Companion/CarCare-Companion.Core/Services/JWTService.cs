namespace CarCare_Companion.Core.Services;

using CarCare_Companion.Common;
using CarCare_Companion.Core.Contracts;
using CarCare_Companion.Core.Models.Identity;
using CarCare_Companion.Infrastructure.Data.Common;
using CarCare_Companion.Infrastructure.Data.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

public class JWTService : IJWTService
{
    private readonly UserManager<ApplicationUser> userManager;
    private readonly IRepository repository;
    private readonly IConfiguration configuration;

    public JWTService(UserManager<ApplicationUser> userManager, IRepository repository, IConfiguration configuration)
    {
        this.userManager = userManager;
        this.repository = repository;
        this.configuration = configuration;
    }

    /// <summary>
    /// Updates the user JWT token
    /// </summary>
    /// <param name="username">The user username</param>
    /// <returns>AuthDataModel containing the new JWT token and user claims</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public async Task<AuthDataModel> RefreshJWTTokenAsync(string username)
    {
        var user = await userManager.FindByNameAsync(username);

        if (user == null)
        {
            throw new ArgumentNullException("User does not exist");
        }

        var userRoles = await userManager.GetRolesAsync(user);

        if (userRoles.Count == 0)
        {
            userRoles.Add("User");
        }

        var authClaims = GenerateUserAuthClaims(user, userRoles);

        var token = GenerateJwtToken(authClaims);

        return new AuthDataModel
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
            Email = user.Email,
            Role = userRoles.First()
        };
    }

    /// <summary>
    /// Generates the authentication claims of the user for the JWT token
    /// </summary>
    /// <param name="user">The target user</param>
    /// <param name="userRoles">The user roles</param>
    /// <returns>Collection of Claims to be used in the JWT token generator</returns>
    public ICollection<Claim> GenerateUserAuthClaims(ApplicationUser user, ICollection<string> userRoles)
    {

        var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
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
    public JwtSecurityToken GenerateJwtToken(ICollection<Claim> authClaims)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]!));

        var token = new JwtSecurityToken(
            issuer: configuration["JWT:Issuer"],
            audience: configuration["JWT:Audience"],
            expires: DateTime.Now.AddMinutes(GlobalConstants.JWTTokenExpirationTime),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

        return token;
    }
}
