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

    public async Task<bool> DoesUserExistAsync(string username)
    {
        var userExists = await userManager.FindByNameAsync(username);

        if (userExists == null)
        {
            return false;
        }

        return true;
    }

    public async Task<AuthDataModel> LoginAsync(LoginRequestModel model)
    {
        var user = await userManager.FindByNameAsync(model.Email);

        if (user == null)
        {
            throw new Exception("User does not exist");
        }

        bool isPasswordValid = await userManager.CheckPasswordAsync(user, model.Password);

        if (!isPasswordValid)
        {
            throw new Exception("Invalid password");
        }
        var userRoles = await userManager.GetRolesAsync(user);

        var authClaims = GenerateUserAuthClaims(user,userRoles);

        var token = GenerateToken(authClaims);

        return new AuthDataModel
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Email = user.Email,
            Id = user.Id.ToString()
        };
       
    }

    public async Task<AuthDataModel> RegisterAsync(RegisterRequestModel model)
    {
        ApplicationUser user = new ApplicationUser()
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            UserName = model.Email,
            Email = model.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            CreatedOn = DateTime.UtcNow,
        };

        var result = await userManager.CreateAsync(user, model.Password);

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
            Id = user.Id.ToString()
        };

    }

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
