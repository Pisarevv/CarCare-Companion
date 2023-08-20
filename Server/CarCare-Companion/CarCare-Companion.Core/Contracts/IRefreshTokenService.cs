namespace CarCare_Companion.Core.Contracts;

using CarCare_Companion.Infrastructure.Data.Models.Identity;

public interface IRefreshTokenService
{
    /// <summary>
    /// Updates the refresh token for a given user asynchronously.
    /// </summary>
    /// <param name="user">The user for whom to update the refresh token.</param>
    /// <returns>The updated refresh token.</returns>
    public Task<string> UpdateRefreshTokenAsync(ApplicationUser user);

    ///// <summary>
    ///// Refreshes the JWT token for a user based on the provided username asynchronously.
    ///// </summary>
    ///// <param name="username">The username for whom to refresh the JWT token.</param>
    ///// <returns>The refreshed authentication data.</returns>
    //public Task<AuthDataModel> RefreshJWTTokenAsync(string username);

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
