namespace CarCare_Companion.Core.Contracts;

using CarCare_Companion.Core.Models.Trip;

public interface ITripRecordsService
{
    public Task<string> CreateAsync(string userId, TripFormRequestModel model);

    public Task EditAsync(string tripId, TripFormRequestModel model);

    public Task DeleteAsync(string tripId);

    public Task<TripEditDetailsResponseModel> GetTripDetailsByIdAsync(string tripId);

    public Task<bool> DoesTripExistByIdAsync(string tripId);

    public Task<bool> IsUserCreatorOfTripAsync(string userId, string tripId);

    public Task<ICollection<TripDetailsByUserResponseModel>> GetAllTripsByUsedIdAsync(string userId);

    public Task<ICollection<TripBasicInformationByUserResponseModel>> GetLastNCountAsync(string userId, int count);

    public Task<int> GetAllUserTripsCountAsync(string userId);

    public Task<decimal?> GetAllUserTripsCostAsync(string userId);
}
