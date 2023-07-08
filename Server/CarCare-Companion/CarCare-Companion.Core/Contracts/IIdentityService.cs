using CarCare_Companion.Core.Models.Identity;

namespace CarCare_Companion.Core.Contracts;

public interface IIdentityService
{
    public Task<bool> DoesUserExistByUsernameAsync(string username);

    public Task<bool> DoesUserExistByIdAsync(string userId);

    public Task<AuthDataModel> RegisterAsync(RegisterRequestModel model);

    public Task<AuthDataModel> LoginAsync(LoginRequestModel model);
}
