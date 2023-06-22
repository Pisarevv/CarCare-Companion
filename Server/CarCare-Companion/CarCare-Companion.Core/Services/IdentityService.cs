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

    public async Task<LoginRequestStatus> LoginAsync(LoginRequestModel model)
    {
        var user = await userManager.FindByNameAsync(model.Email);
        if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
        {
            var userRoles = await userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var token = GenerateToken(authClaims);

            return new LoginRequestStatus
            {
                LoginSuccessful = true,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo,
                StatusMassage = "Successful login."
            };
        }

        return new LoginRequestStatus
        {
            LoginSuccessful = false,
            StatusMassage = "Invalid email or password."
        };
    }

    public async Task<bool> RegisterAsync(RegisterRequestModel model)
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
            return false;
        }

        return true;
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
