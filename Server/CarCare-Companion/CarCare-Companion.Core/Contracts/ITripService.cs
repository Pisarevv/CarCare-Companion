namespace CarCare_Companion.Core.Contracts;

using CarCare_Companion.Core.Models.Trip;

public interface ITripService
{
    public Task<string> CreateTripAsync(TripCreateRequestModel model);
}
