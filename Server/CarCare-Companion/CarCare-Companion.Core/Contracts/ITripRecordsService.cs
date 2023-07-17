namespace CarCare_Companion.Core.Contracts;

using CarCare_Companion.Core.Models.Trip;

public interface ITripRecordsService
{
    public Task<string> CreateAsync(string userId, TripCreateRequestModel model);

    public Task<ICollection<TripDetailsByUserResponseModel>> GetAllTripsByUsedIdAsync(string userId);

    public Task<ICollection<TripBasicInformationByUserResponseModel>> GetLastNCountAsync(string userId, int count);

    public Task<int> GetAllUserTripsCountAsync(string userId);

    public Task<decimal?> GetAllUserTripsCostAsync(string userId);
}
