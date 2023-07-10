namespace CarCare_Companion.Core.Contracts;

using CarCare_Companion.Core.Models.Trip;

public interface ITripService
{
    public Task<string> CreateTripAsync(string userId, TripCreateRequestModel model);

    public Task<ICollection<TripDetailsByUserResponseModel>> GetAllTripsByUsedIdAsync(string userId);

    public Task<int> GetAllUserTripsCountAsync(string userId);

    public Task<decimal?> GetAllUserTripsCostAsync(string userId);
}
