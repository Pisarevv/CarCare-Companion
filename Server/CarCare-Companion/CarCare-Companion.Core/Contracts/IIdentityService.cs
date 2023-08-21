namespace CarCare_Companion.Core.Contracts;

using CarCare_Companion.Core.Models.Identity;



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

}

