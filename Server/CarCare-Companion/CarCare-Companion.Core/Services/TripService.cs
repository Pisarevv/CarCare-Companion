namespace CarCare_Companion.Core.Services;

using CarCare_Companion.Core.Contracts;
using CarCare_Companion.Core.Models.Trip;
using CarCare_Companion.Infrastructure.Data.Common;
using CarCare_Companion.Infrastructure.Data.Models.Records;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            OwnerId = Guid.Parse(userId),
            Cost = CalculateTripCost(model.FuelPrice, model.UsedFuel)

        };

        await repository.AddAsync<TripRecord>(tripToAdd);
        await repository.SaveChangesAsync();

        return tripToAdd.Id.ToString();
    }

    public async Task<ICollection<TripDetailsByUserResponseModel>> GetAllTripsByUsedIdAsync(string userId)
    {
        return await repository.AllReadonly<TripRecord>()
               .Where(v => v.IsDeleted == false)
               .Where(t => t.OwnerId == Guid.Parse(userId))
               .OrderByDescending(t => t.CreatedOn)
               .Select(t => new TripDetailsByUserResponseModel
               {
                   Id = t.Id.ToString(),
                   StartDestination = t.StartDestination,
                   EndDestination = t.EndDestination,
                   MileageTravelled = t.MileageTravelled,
                   FuelPrice = t.FuelPrice,
                   UsedFuel = t.UsedFuel,
                   VehicleMake = t.Vehicle.Make,
                   VehicleModel = t.Vehicle.Model,
                   DateCreated = t.CreatedOn.ToString("dd/MM/yyyy"),
                   TripCost = t.Cost
             
               })
               .ToListAsync();

    }

    public async Task<int> GetAllUserTripsCountAsync(string userId)
    {
        return await repository.AllReadonly<TripRecord>()
               .Where(v => v.IsDeleted == false)
               .Where(tr => tr.OwnerId == Guid.Parse(userId))
               .CountAsync();
    }

    public async Task<decimal?> GetAllUserTripsCostAsync(string userId)
    {
        return await repository.All<TripRecord>()
               .Where(v => v.IsDeleted == false)
               .Where(tr => tr.OwnerId == Guid.Parse(userId) && tr.Cost != null)
               .SumAsync(tr => tr.Cost);
       
           
    }

    private decimal? CalculateTripCost(decimal? fuelPrice, double? usedFuel)
    {
        return fuelPrice * Convert.ToDecimal(usedFuel);
    }
}
