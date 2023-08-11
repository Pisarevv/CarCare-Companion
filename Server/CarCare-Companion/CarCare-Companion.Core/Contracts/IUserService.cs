namespace CarCare_Companion.Core.Contracts;

using CarCare_Companion.Core.Models.Admin.Users;

public interface IUserService
{
    /// <summary>
    /// Retrieves all users asynchronously.
    /// </summary>
    /// <returns>A collection of UserInformationResponseModels representing the users.</returns>
    public Task<ICollection<UserInformationResponseModel>> GetAllUsersAsync();

    /// <summary>
    /// Retrieves detailed information of a specific user based on its ID asynchronously.
    /// </summary>
    /// <param name="userId">The ID of the user to retrieve details for.</param>
    /// <returns>The UserDetailsResponseModel containing the user's details.</returns>
    public Task<UserDetailsResponseModel> GetUserDetailsByIdAsync(string userId);
}