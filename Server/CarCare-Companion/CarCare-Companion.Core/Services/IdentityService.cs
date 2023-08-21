namespace CarCare_Companion.Core.Services;

using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

using CarCare_Companion.Core.Contracts;
using CarCare_Companion.Core.Models.Identity;
using CarCare_Companion.Infrastructure.Data.Models.Identity;

using CarCare_Companion.Infrastructure.Data.Common;

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
    private readonly IJWTService jwtService;
    private readonly IRefreshTokenService refreshTokenService;

    public IdentityService
        (
        UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager , 
        IConfiguration configuration, IRepository repository,
        IJWTService jwtService, IRefreshTokenService refreshTokenService
        )
    {
        this.userManager = userManager;
        this.configuration = configuration;
        this.roleManager = roleManager;
        this.repository = repository;
        this.jwtService = jwtService;
        this.refreshTokenService = refreshTokenService;
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

        var authClaims = jwtService.GenerateUserAuthClaims(user,userRoles);

        var token = jwtService.GenerateJwtToken(authClaims);

        var refreshToken = await refreshTokenService.UpdateRefreshTokenAsync(user);

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

        await refreshTokenService.TerminateUserRefreshTokenAsync(userId);

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

        await refreshTokenService.TerminateUserRefreshTokenAsync(userId);

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

    
}
