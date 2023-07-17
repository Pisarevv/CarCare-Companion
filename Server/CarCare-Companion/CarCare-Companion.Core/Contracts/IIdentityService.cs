namespace CarCare_Companion.Core.Contracts;

using CarCare_Companion.Core.Models.Identity;

public interface IIdentityService
{
    public Task<bool> DoesUserExistByUsernameAsync(string username);

    public Task<AuthDataModel> RegisterAsync(RegisterRequestModel model);

    public Task<AuthDataModel> LoginAsync(LoginRequestModel model);
}
