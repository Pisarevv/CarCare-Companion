namespace CarCare_Companion.Core.Contracts;

using CarCare_Companion.Core.Models.Identity;
using CarCare_Companion.Infrastructure.Data.Models.Identity;


public interface IIdentityService
{
    public Task<bool> DoesUserExistByUsernameAsync(string username);

    public Task<bool> DoesUserExistByIdAsync(string userId);

    public Task<bool> AddAdminAsync(string userId);

    public Task<bool> RemoveAdminAsync(string userId);

    public Task<bool> RegisterAsync(RegisterRequestModel model);

    public Task<bool> IsUserInRoleAsync(string username, string role);

    public Task<AuthDataInternalTransferModel> LoginAsync(LoginRequestModel model);

    public Task<string> UpdateRefreshTokenAsync(ApplicationUser user);

    public Task<AuthDataModel> RefreshJWTTokenAsync(string username);

    public Task<bool> IsUserRefreshTokenOwnerAsync(string username,  string refreshToken);

    public Task<bool> IsUserRefreshTokenExpiredAsync(string refreshToken);

    public Task<string?> GetRefreshTokenOwnerAsync(string refreshToken);

    public Task<bool> TerminateUserRefreshTokenAsync(string userId);



}
