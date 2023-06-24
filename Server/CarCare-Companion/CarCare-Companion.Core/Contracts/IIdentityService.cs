using CarCare_Companion.Core.Models.Identity;

namespace CarCare_Companion.Core.Contracts;

public interface IIdentityService
{
    public Task<bool> DoesUserExistAsync(string username);

    public Task<AuthDataModel> RegisterAsync(RegisterRequestModel model);

    public Task<AuthDataModel> LoginAsync(LoginRequestModel model);
}
