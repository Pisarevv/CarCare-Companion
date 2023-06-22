using CarCare_Companion.Core.Models.Identity;

namespace CarCare_Companion.Core.Contracts;

public interface IIdentityService
{
    public Task<bool> DoesUserExistAsync(string username);

    public Task<bool> RegisterAsync(RegisterRequestModel model);

    public Task<LoginRequestStatus> LoginAsync(LoginRequestModel model);
}
