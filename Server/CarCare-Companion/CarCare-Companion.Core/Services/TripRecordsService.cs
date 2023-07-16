namespace CarCare_Companion.Core.Services;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using CarCare_Companion.Core.Contracts;
using CarCare_Companion.Core.Models.Trip;
using CarCare_Companion.Infrastructure.Data.Common;
using CarCare_Companion.Infrastructure.Data.Models.Records;

using static Common.FormattingMethods;

/// <summary>
/// The TripService is responsible for operations regarding the trip record-related actions
/// </summary>
public class TripRecordsService : ITripRecordsService
{
    private readonly IRepository repository;

    public TripRecordsService(IRepository repository)
    {
        this.repository = repository;
    }

    /// <summary>
    /// Creates a new trip record
    /// </summary>
    /// <param name="model">The input model containing the trip information</param>
    /// <returns>String containing the newly created trip record Id</returns>
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

    /// <summary>
    /// Retrieves all user record trips ordered by date of their creation
    /// </summary>
    /// <param name="userId">The user identifier</param>
    /// <returns>A collection of trip records</returns>
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
                   DateCreated = FormatDateTimeToString(t.CreatedOn),
                   TripCost = t.Cost
             
               })
               .ToListAsync();

    }

    /// <summary>
    /// Retrieves all user trip records count
    /// </summary>
    /// <returns>The count of the user trip records</returns>
    public async Task<int> GetAllUserTripsCountAsync(string userId)
    {
        return await repository.AllReadonly<TripRecord>()
               .Where(v => v.IsDeleted == false)
               .Where(tr => tr.OwnerId == Guid.Parse(userId))
               .CountAsync();
    }

    /// <summary>
    /// Retrieves all user trip records cost
    /// </summary>
    /// <returns>The total cost of the user trip records</returns>
    public async Task<decimal?> GetAllUserTripsCostAsync(string userId)
    {
        return await repository.All<TripRecord>()
               .Where(v => v.IsDeleted == false)
               .Where(tr => tr.OwnerId == Guid.Parse(userId) && tr.Cost != null)
               .SumAsync(tr => tr.Cost);
                 
    }

    /// <summary>
    /// Retrieves a specified count of records containing basic information about the user trip records
    /// ordered by time
    /// </summary>
    /// <param name="userId">The user identifier</param>
    /// <param name="count">The amount of record to be retrieved</param>
    /// <returns>A collection of trip records</returns>
    public async Task<ICollection<TripBasicInformationByUserResponseModel>> GetLastNCountAsync(string userId, int count)
    {
        return await repository.AllReadonly<TripRecord>()
               .Where(v => v.IsDeleted == false)
               .Where(t => t.OwnerId == Guid.Parse(userId))
               .OrderByDescending(t => t.CreatedOn)
               .Take(count)
               .Select(t => new TripBasicInformationByUserResponseModel
               {
                   Id = t.Id.ToString(),
                   StartDestination = t.StartDestination,
                   EndDestination = t.EndDestination,
                   MileageTravelled = t.MileageTravelled,     
                   Vehicle = $"{t.Vehicle.Make} {t.Vehicle.Model}"
               })
               .ToListAsync();
    }

    /// <summary>
    /// Calculates the total cost of the user trips
    /// </summary>
    /// <returns>The total cost of the user trip record</returns>
    private static decimal? CalculateTripCost(decimal? fuelPrice, double? usedFuel)
    {
        return fuelPrice * Convert.ToDecimal(usedFuel);
    }

    
}
