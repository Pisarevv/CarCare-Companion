namespace CarCare_Companion.Core.Services;

using System;
using System.Threading.Tasks;

using CarCare_Companion.Core.Contracts;
using CarCare_Companion.Core.Models.Trip;
using CarCare_Companion.Infrastructure.Data.Common;
using CarCare_Companion.Infrastructure.Data.Models.Records;

public class TripService : ITripService
{
    private readonly IRepository repository;

    public TripService(IRepository repository)
    {
        this.repository = repository;
    }

    public async Task<string> CreateTripAsync(string userId, TripCreateRequestModel model)
    {
        TripRecord tripToAdd = new TripRecord()
        {
            StartDestination = model.StartDestination,
            EndDestination = model.EndDestination,
            MileageTravelled = model.MileageTravelled,
            UsedFuel = model.UsedFuel,
            FuelPrice = model.FuelPrice,
            CreatedOn = DateTime.UtcNow,
            VehicleId = Guid.Parse(model.VehicleId),
            OwnerId = Guid.Parse(userId)

        };

        await repository.AddAsync<TripRecord>(tripToAdd);
        await repository.SaveChangesAsync();

        return tripToAdd.Id.ToString();
    }


}
