namespace CarCare_Companion.Core.Contracts;

using CarCare_Companion.Core.Models.Identity;
using CarCare_Companion.Infrastructure.Data.Models.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

public interface IJWTService
{

    /// <summary>
    /// Refreshes the JWT token for a user based on the provided username asynchronously.
    /// </summary>
    /// <param name="username">The username for whom to refresh the JWT token.</param>
    /// <returns>The refreshed authentication data.</returns>
    public Task<AuthDataModel> RefreshJWTTokenAsync(string username);

    /// <summary>
    /// Creates a collection of user claims
    /// </summary>
    /// <param name="user">The user</param>
    /// <param name="userRoles">Collection of user roles</param>
    /// <returns>Collection of claims for the user</returns>
    public ICollection<Claim> GenerateUserAuthClaims(ApplicationUser user, ICollection<string> userRoles);

    /// <summary>
    /// Generates a user JWT token
    /// </summary>
    /// <param name="authClaims">The authentication claims for the user</param>
    /// <returns>JwtSecurityToken</returns>
    public JwtSecurityToken GenerateJwtToken(ICollection<Claim> authClaims);
}
