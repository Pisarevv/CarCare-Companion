namespace CarCare_Companion.Core.Contracts;

using CarCare_Companion.Core.Models.Admin.Users;

public interface IUserService
{
    public Task<ICollection<UserInformationResponseModel>> GetAllUsersAsync();

    public Task<UserDetailsResponseModel?> GetUserDetailsByIdAsync(string userId);

    public Task<int> GetRecentlyJoinedUsersCountAsync(int days);
}
