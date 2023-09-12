namespace CarCare_Companion.Core.Services;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

using CarCare_Companion.Core.Contracts;
using CarCare_Companion.Core.Models.Trip;
using CarCare_Companion.Infrastructure.Data.Common;
using CarCare_Companion.Infrastructure.Data.Models.Records;

using static Common.CacheKeysAndDurations.Trips;
using CarCare_Companion.Core.Models.TripRecords;




/// <summary>
/// The TripService is responsible for operations regarding the trip record-related actions
/// </summary>
public class TripRecordsService : ITripRecordsService
{
    private readonly IRepository repository;
    private readonly IMemoryCache memoryCache;

    public TripRecordsService(IRepository repository, IMemoryCache memoryCache)
    {
        this.repository = repository;
        this.memoryCache = memoryCache;
    }

    /// <summary>
    /// Creates a new trip record
    /// </summary>
    /// <param name="model">The input model containing the trip information</param>
    /// <returns>String containing the newly created trip record Id</returns>
    public async Task<TripResponseModel> CreateAsync(string userId, TripFormRequestModel model)
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

        this.memoryCache.Remove(userId + UserTripsCacheKeyAddition);
        this.memoryCache.Remove(userId + UserTripsCostCacheKeyAddition);
        this.memoryCache.Remove(userId + UserTripsCountCacheKeyAddition);

        return new TripResponseModel
        {
            Id = tripToAdd.Id.ToString(),
            StartDestination = tripToAdd.StartDestination,
            EndDestination = tripToAdd.EndDestination,
            FuelPrice = tripToAdd.FuelPrice,
            UsedFuel = tripToAdd.UsedFuel,
            MileageTravelled = tripToAdd.MileageTravelled
        };

    }

    /// <summary>
    /// Edits a user trip record
    /// </summary>
    /// <param name="tripId">The trip record identifier</param>
    /// <param name="model">The input model containing the trip information</param>
    /// <returns></returns>
    public async Task<TripResponseModel> EditAsync(string tripId, string userId,TripFormRequestModel model)
    {
        TripRecord tripToEdit = await repository.GetByIdAsync<TripRecord>(Guid.Parse(tripId));

        tripToEdit.StartDestination = model.StartDestination;
        tripToEdit.EndDestination = model.EndDestination;
        tripToEdit.MileageTravelled = model.MileageTravelled;
        tripToEdit.UsedFuel = model.UsedFuel;
        tripToEdit.FuelPrice = model.FuelPrice;
        tripToEdit.ModifiedOn = DateTime.UtcNow;
        tripToEdit.VehicleId = Guid.Parse(model.VehicleId);
        tripToEdit.Cost = CalculateTripCost(model.FuelPrice, model.UsedFuel);

        await repository.SaveChangesAsync();

        this.memoryCache.Remove(userId + UserTripsCacheKeyAddition);
        this.memoryCache.Remove(userId + UserTripsCostCacheKeyAddition);
        this.memoryCache.Remove(userId + UserTripsCountCacheKeyAddition);

        return new TripResponseModel
        {
            Id = tripToEdit.Id.ToString(),
            StartDestination = tripToEdit.StartDestination,
            EndDestination = tripToEdit.EndDestination,
            FuelPrice = tripToEdit.FuelPrice,
            UsedFuel = tripToEdit.UsedFuel,
            MileageTravelled = tripToEdit.MileageTravelled
        };
    }

    /// <summary>
    /// Deletes a trip record
    /// </summary>
    /// <param name="tripId">The trip record identifier</param>
    public async Task DeleteAsync(string tripId, string userId)
    {
        TripRecord tripToDelete = await repository.GetByIdAsync<TripRecord>(Guid.Parse(tripId));

        repository.SoftDelete<TripRecord>(tripToDelete);

        await repository.SaveChangesAsync();

        this.memoryCache.Remove(userId + UserTripsCacheKeyAddition);
        this.memoryCache.Remove(userId + UserTripsCostCacheKeyAddition);
        this.memoryCache.Remove(userId + UserTripsCountCacheKeyAddition);
    }

    /// <summary>
    /// Checks if a trip record exists
    /// </summary>
    /// <param name="tripId">The trip record identifier</param>
    /// <returns>Boolean based on the search result</returns>
    public async Task<bool> DoesTripExistByIdAsync(string tripId)
    {
        return await repository.AllReadonly<TripRecord>()
                     .Where(tr => tr.Id == Guid.Parse(tripId))
                     .AnyAsync();
    }

    /// <summary>
    /// Retrieves all user record trips ordered by date of their creation
    /// </summary>
    /// <param name="userId">The user identifier</param>
    /// <returns>A collection of trip records</returns>
    public async Task<ICollection<TripDetailsByUserResponseModel>> GetAllTripsByUsedIdAsync(string userId)
    {
        ICollection<TripDetailsByUserResponseModel>? userTrips =
            this.memoryCache.Get<ICollection<TripDetailsByUserResponseModel>>(userId + UserTripsCacheKeyAddition);

        if(userTrips == null)
        {
            userTrips = await repository.AllReadonly<TripRecord>()
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
                   DateCreated = t.CreatedOn,
                   TripCost = t.Cost

               })
               .ToListAsync();

            MemoryCacheEntryOptions options = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(UserTripsCacheDurationMinutes));

            this.memoryCache.Set(userId + UserTripsCacheKeyAddition, userTrips, options);
        }

        return userTrips;

    }

    /// <summary>
    /// Retrieves all user trip records as queryable
    /// </summary>
    /// <param name="userId">The user identifier</param>
    /// <returns>A queryable of trip records</returns>
    public async Task<IQueryable<TripRecord>> GetAllByUserIdAsQueryableAsync(string userId)
    {

        return repository.AllReadonly<TripRecord>()
            .Where(sr => sr.IsDeleted == false && sr.OwnerId == Guid.Parse(userId))
            .AsQueryable();
    }

    /// <summary>
    /// Checks if the user is the creator of the trip record
    /// </summary>
    /// <param name="userId">The user identifier</param>
    /// <param name="tripId">The trip record identifier</param>
    /// <returns>Boolean based on the search result</returns>
    public async Task<bool> IsUserCreatorOfTripAsync(string userId, string tripId)
    {
        return await repository.AllReadonly<TripRecord>().
                   Where(v => v.Id == Guid.Parse(tripId) && v.OwnerId == Guid.Parse(userId))
                   .AnyAsync();
    }

    /// <summary>
    /// Retrieves the trip details to the user
    /// </summary>
    /// <param name="tripId">The trip identifier</param>
    /// <returns>Detailed model containing all the trip information</returns>
    public async Task<TripEditDetailsResponseModel> GetTripDetailsByIdAsync(string tripId)
    {
        return await repository.AllReadonly<TripRecord>()
               .Where(t => t.Id == Guid.Parse(tripId))
               .Select(t => new TripEditDetailsResponseModel
               {
                   Id = t.Id.ToString(),
                   StartDestination = t.StartDestination,
                   EndDestination = t.EndDestination,
                   MileageTravelled = t.MileageTravelled,
                   FuelPrice = t.FuelPrice,
                   UsedFuel = t.UsedFuel,
                   Vehicle = t.VehicleId.ToString()
               })
               .FirstAsync();

    }

    /// <summary>
    /// Retrieves all user trip records count
    /// </summary>
    /// <returns>The count of the user trip records</returns>
    public async Task<int> GetAllUserTripsCountAsync(string userId)
    {
        int tripsCount =
            this.memoryCache.Get<int>(userId + UserTripsCountCacheKeyAddition);

        if(tripsCount == 0)
        {
            tripsCount = await repository.AllReadonly<TripRecord>()
               .Where(v => v.IsDeleted == false)
               .Where(tr => tr.OwnerId == Guid.Parse(userId))
               .CountAsync();

            MemoryCacheEntryOptions options = new MemoryCacheEntryOptions()
              .SetSlidingExpiration(TimeSpan.FromMinutes(UserTripsCountCacheDurationMinutes));

            this.memoryCache.Set(userId + UserTripsCountCacheKeyAddition, tripsCount, options);
        }
        return tripsCount;
    }   

    /// <summary>
    /// Retrieves all user trip records cost
    /// </summary>
    /// <returns>The total cost of the user trip records</returns>
    public async Task<decimal?> GetAllUserTripsCostAsync(string userId)
    {
        decimal? tripsCost =
            this.memoryCache.Get<decimal?>(userId + UserTripsCostCacheKeyAddition);

        if(tripsCost == null)
        {
            tripsCost = await repository.All<TripRecord>()
              .Where(v => v.IsDeleted == false)
              .Where(tr => tr.OwnerId == Guid.Parse(userId) && tr.Cost != null)
              .SumAsync(tr => tr.Cost);

            MemoryCacheEntryOptions options = new MemoryCacheEntryOptions()
             .SetSlidingExpiration(TimeSpan.FromMinutes(UserTripsCountCacheDurationMinutes));

            this.memoryCache.Set(userId + UserTripsCostCacheKeyAddition, tripsCost, options);
        }

        return tripsCost;

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

    /// <summary>
    /// Retrieves a list of trip records for a specified page.
    /// </summary>
    /// <param name="tripRecords">The complete set of trip records to paginate.</param>
    /// <param name="currentPage">The page number to retrieve.</param>
    /// <param name="recordPerPage">The number of records per page.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of TripDetailsByUserResponseModel for the specified page.</returns>
    public async Task<List<TripDetailsByUserResponseModel>> RetrieveTripRecordsByPage(IQueryable<TripRecord> tripRecords, int currentPage, int recordPerPage)
    {
        return await tripRecords
                     .Skip((currentPage - 1) * recordPerPage)
                     .Take(recordPerPage)
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
                         DateCreated = t.CreatedOn,
                         TripCost = t.Cost
                     })
                     .ToListAsync();
    }
}
