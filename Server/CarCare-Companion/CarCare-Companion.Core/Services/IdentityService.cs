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

using CarCare_Companion.Infrastructure.Data.Common;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;

using static Common.GlobalConstants;

/// <summary>
/// The IdentityService is responsible for all the operations regarding the user-related actions
/// </summary>
public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> userManager;
    private readonly RoleManager<ApplicationRole> roleManager;
    private readonly IRepository repository;
    private readonly IConfiguration configuration;

    public IdentityService(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager , IConfiguration configuration, IRepository repository)
    {
        this.userManager = userManager;
        this.configuration = configuration;
        this.roleManager = roleManager;
        this.repository = repository;
    }

    /// <summary>
    /// Checks if an user exists by his username.
    /// </summary>
    /// <param name="username">The user username used for the searching.</param>
    /// <returns>A boolean if the user exists.</returns>
    public async Task<bool> DoesUserExistByUsernameAsync(string username)
    {
        var userExists = await userManager.FindByNameAsync(username);

        if (userExists == null)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Checks if an user exists by his identifier.
    /// </summary>
    /// <param name="userId">The user identifier used for the searching.</param>
    /// <returns>A boolean if the user exists.</returns>
    public async Task<bool> DoesUserExistByIdAsync(string userId)
    {
        var userExists = await userManager.FindByIdAsync(userId);

        if (userExists == null)
        {
            return false;
        }

        return true;
    }


    /// <summary>
    /// Logs in the user.
    /// </summary>
    /// <param name="inputModel">The input data containing the user email and password.</param>
    /// <returns>An object containing the user email and generated JWT token.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the user with the specified Email is not found.</exception>
    /// <exception cref="ArgumentException">Thrown when the user password is invalid.</exception>
    public async Task<AuthDataInternalTransferModel> LoginAsync(LoginRequestModel inputModel)
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

        var token = GenerateJwtToken(authClaims);

        var refreshToken = await UpdateRefreshTokenAsync(user);

        return new AuthDataInternalTransferModel
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
            Email = user.Email,
            Role = userRoles.First(),
            RefreshToken = refreshToken
        };
       
    }

    /// <summary>
    /// Creates a new user.
    /// </summary>
    /// <param name="inputModel">The input model containing the user first name, 
    /// last name, email, password and confirm password</param>
    /// <exception cref="Exception">Thrown when the user creating is not successful</exception>
    public async Task<bool> RegisterAsync(RegisterRequestModel inputModel)
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

        return result.Succeeded;

    }

    /// <summary>
    /// Adds an user to the Administrator role
    /// </summary>
    /// <param name="userId">The user identifier</param>
    /// <returns>A boolen based on the result</returns>
    /// <exception cref="Exception">Exception thrown if an error occurs during the adding to role of the user</exception>
    public async Task<bool> AddAdminAsync(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);

        var result = await userManager.AddToRoleAsync(user, AdministratorRoleName);

        if (!result.Succeeded)
        {
            throw new Exception(result.Errors.First().ToString());
        }

        await TerminateUserRefreshTokenAsync(userId);

        return result.Succeeded;

    }


    /// <summary>
    /// Removes an user from the Administrator role
    /// </summary>
    /// <param name="userId">The user identifier</param>
    /// <returns>A boolen based on the result</returns>
    /// <exception cref="Exception">Exception thrown if an error occurs during the adding to role of the user</exception>
    public async Task<bool> RemoveAdminAsync(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);

        var result = await userManager.RemoveFromRoleAsync(user, AdministratorRoleName);

        if (!result.Succeeded)
        {
            throw new Exception(result.Errors.First().ToString());
        }

        await TerminateUserRefreshTokenAsync(userId);

        return result.Succeeded;
    }

    /// <summary>
    /// Validates if a user is in a specific role
    /// </summary>
    /// <param name="userId">The user identifier </param>
    /// <param name="role">The role name</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">Thrown when the user does not exist</exception>
    public async Task<bool> IsUserInRoleAsync(string userId, string role)
    {
        var user = await userManager.FindByIdAsync(userId);

        if (user == null)
        {
            throw new ArgumentNullException("User does not exist");
        }

        return await userManager.IsInRoleAsync(user, role);
    }

    /// <summary>
    /// Updates the user refresh token. If the user doesn't have a token - a refresh token is added.
    /// If the user has a refresh token - the token is updated.
    /// </summary>
    /// <param name="user">The application user</param>
    /// <returns></returns>
    public async Task<string> UpdateRefreshTokenAsync(ApplicationUser user)
    {
        UserRefreshToken newToken = GenerateRefreshToken(user.Id.ToString());

        UserRefreshToken? userRefreshToken = await repository.All<UserRefreshToken>()
                                 .Where(urt => urt.UserId == user.Id)
                                 .FirstOrDefaultAsync();

        if(userRefreshToken == null)
        {
            user.RefreshToken = newToken;
            await repository.AddAsync<UserRefreshToken>(newToken);
            await repository.SaveChangesAsync();

            return newToken.RefreshToken.ToString();
            
        }

        userRefreshToken.RefreshToken = newToken.RefreshToken;
        userRefreshToken.RefreshTokenExpiration = newToken.RefreshTokenExpiration;
        await repository.SaveChangesAsync();

        return userRefreshToken.RefreshToken.ToString();
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

    public async Task<string?> GetRefreshTokenOwnerAsync(string refreshToken)
    {
        return await repository.AllReadonly<UserRefreshToken>()
               .Where(urf => urf.RefreshToken == refreshToken)
               .Select(urf => urf.User.UserName)
               .FirstOrDefaultAsync();
    }


    /// <summary>
    /// Checks if the user is the refresh token owner
    /// </summary>
    /// <param name="username">The user identifier</param>
    /// <param name="refreshToken">The refresh token</param>
    /// <returns>Boolean based on the search result</returns>
    public async Task<bool> IsUserRefreshTokenOwnerAsync(string username, string refreshToken)
    {
        return await repository.AllReadonly<UserRefreshToken>()
               .Where(urt => urt.User.UserName == username && urt.RefreshToken == refreshToken)
               .AnyAsync();
    }

    /// <summary>
    /// Checks if the user refresh token is expired
    /// </summary>
    /// <param name="refreshToken">The refresh token</param>
    /// <returns>Boolean based on the search result</returns>
    public async Task<bool> IsUserRefreshTokenExpiredAsync(string refreshToken)
    {
        DateTime? tokenExpirationDate = await repository.AllReadonly<UserRefreshToken>()
            .Where(urt => urt.RefreshToken == refreshToken)
            .Select(urt => urt.RefreshTokenExpiration)
            .FirstAsync();

        return tokenExpirationDate < DateTime.UtcNow;
    }

    /// <summary>
    /// Retrieves the claims of the JWT token
    /// </summary>
    /// <param name="token"></param>
    /// <returns>ClaimsPrincipal containing the user claims</returns>
    /// <exception cref="SecurityTokenException"></exception>
    public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]!)),
            ValidateLifetime = false
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
        if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Invalid token");

        return principal;

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
    private JwtSecurityToken GenerateJwtToken(List<Claim> authClaims)
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

    /// <summary>
    /// Generates a refresh token.
    /// </summary>
    /// <param name="userId">The user identifier</param>
    /// <returns>A refresh token containing an Id, UserId, RefreshToken and RefreshTokenExpiration</returns>
    private UserRefreshToken GenerateRefreshToken(string userId)
    {
        var randomNumber = new byte[256];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        var token = Convert.ToBase64String(randomNumber);

        UserRefreshToken refreshToken = new UserRefreshToken()
        {
            Id = Guid.NewGuid(),
            UserId = Guid.Parse(userId),
            RefreshToken = token,
            RefreshTokenExpiration = DateTime.UtcNow.AddDays(GlobalConstants.RefreshTokenExpirationTime),
        };


        return refreshToken;
    }

    public async Task TerminateUserRefreshTokenAsync(string userId)
    {
        UserRefreshToken? refreshToken = await repository.All<UserRefreshToken>()
                         .Where(urt => urt.UserId == Guid.Parse(userId))
                         .FirstOrDefaultAsync();

        if(refreshToken == null)
        {
            return;
        }

        refreshToken.RefreshToken = null;
        refreshToken.RefreshTokenExpiration = DateTime.UtcNow;

        await repository.SaveChangesAsync();
    }

   
}
