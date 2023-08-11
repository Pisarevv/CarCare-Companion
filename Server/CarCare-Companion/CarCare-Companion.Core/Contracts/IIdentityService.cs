namespace CarCare_Companion.Core.Contracts;

using CarCare_Companion.Core.Models.Identity;
using CarCare_Companion.Infrastructure.Data.Models.Identity;


public interface IIdentityService
{
    /// <summary>
    /// Checks if a user exists based on the provided username asynchronously.
    /// </summary>
    /// <param name="username">The username to check.</param>
    /// <returns>True if the user exists, otherwise false.</returns>
    public Task<bool> DoesUserExistByUsernameAsync(string username);

    /// <summary>
    /// Checks if a user exists based on the provided user ID asynchronously.
    /// </summary>
    /// <param name="userId">The user ID to check.</param>
    /// <returns>True if the user exists, otherwise false.</returns>
    public Task<bool> DoesUserExistByIdAsync(string userId);

    /// <summary>
    /// Adds administrative privileges to a user based on the provided user ID asynchronously.
    /// </summary>
    /// <param name="userId">The user ID to grant admin privileges to.</param>
    /// <returns>True if the operation was successful, otherwise false.</returns>
    public Task<bool> AddAdminAsync(string userId);

    /// <summary>
    /// Removes administrative privileges from a user based on the provided user ID asynchronously.
    /// </summary>
    /// <param name="userId">The user ID to revoke admin privileges from.</param>
    /// <returns>True if the operation was successful, otherwise false.</returns>
    public Task<bool> RemoveAdminAsync(string userId);

    /// <summary>
    /// Registers a new user based on the provided registration model asynchronously.
    /// </summary>
    /// <param name="model">The registration details model.</param>
    /// <returns>True if registration was successful, otherwise false.</returns>
    public Task<bool> RegisterAsync(RegisterRequestModel model);

    /// <summary>
    /// Checks if a user belongs to a specific role based on the provided username and role asynchronously.
    /// </summary>
    /// <param name="username">The username to check.</param>
    /// <param name="role">The role to verify.</param>
    /// <returns>True if the user is in the specified role, otherwise false.</returns>
    public Task<bool> IsUserInRoleAsync(string username, string role);

    /// <summary>
    /// Logs a user in and returns authentication data based on the provided login model asynchronously.
    /// </summary>
    /// <param name="model">The login details model.</param>
    /// <returns>The authentication data for the logged-in user.</returns>
    public Task<AuthDataInternalTransferModel> LoginAsync(LoginRequestModel model);

    /// <summary>
    /// Updates the refresh token for a given user asynchronously.
    /// </summary>
    /// <param name="user">The user for whom to update the refresh token.</param>
    /// <returns>The updated refresh token.</returns>
    public Task<string> UpdateRefreshTokenAsync(ApplicationUser user);

    /// <summary>
    /// Refreshes the JWT token for a user based on the provided username asynchronously.
    /// </summary>
    /// <param name="username">The username for whom to refresh the JWT token.</param>
    /// <returns>The refreshed authentication data.</returns>
    public Task<AuthDataModel> RefreshJWTTokenAsync(string username);

    /// <summary>
    /// Checks if a user is the owner of the provided refresh token asynchronously.
    /// </summary>
    /// <param name="username">The username to check.</param>
    /// <param name="refreshToken">The refresh token to verify.</param>
    /// <returns>True if the user owns the provided refresh token, otherwise false.</returns>
    public Task<bool> IsUserRefreshTokenOwnerAsync(string username, string refreshToken);

    /// <summary>
    /// Checks if a provided refresh token has expired asynchronously.
    /// </summary>
    /// <param name="refreshToken">The refresh token to check.</param>
    /// <returns>True if the refresh token has expired, otherwise false.</returns>
    public Task<bool> IsUserRefreshTokenExpiredAsync(string refreshToken);

    /// <summary>
    /// Gets the owner of a provided refresh token asynchronously.
    /// </summary>
    /// <param name="refreshToken">The refresh token to query.</param>
    /// <returns>The username of the owner, or null if not found.</returns>
    public Task<string?> GetRefreshTokenOwnerAsync(string refreshToken);

    /// <summary>
    /// Invalidates and removes the refresh token for a user based on the provided user ID asynchronously.
    /// </summary>
    /// <param name="userId">The user ID for whom to terminate the refresh token.</param>
    /// <returns>True if the operation was successful, otherwise false.</returns>
    public Task<bool> TerminateUserRefreshTokenAsync(string userId);
}

