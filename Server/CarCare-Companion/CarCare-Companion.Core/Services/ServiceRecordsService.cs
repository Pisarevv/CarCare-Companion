namespace CarCare_Companion.Core.Services;

using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

using CarCare_Companion.Core.Contracts;
using CarCare_Companion.Core.Models.ServiceRecords;
using CarCare_Companion.Infrastructure.Data.Common;
using CarCare_Companion.Infrastructure.Data.Models.Records;

using static Common.CacheKeysAndDurations.ServiceRecords;
using CarCare_Companion.Core.Models.Search;



/// <summary>
/// The ServiceRecordsService is responsible for operations regarding the service records-related actions
/// </summary>
public class ServiceRecordsService : IServiceRecordsService
{
    private readonly IRepository repository;
    private readonly IMemoryCache memoryCache;

    public ServiceRecordsService(IRepository repository, IMemoryCache memoryCache)
    {
        this.repository = repository;
        this.memoryCache = memoryCache;
    }


    /// <summary>
    /// Creates a new service record
    /// </summary>
    /// <param name="model">The input model containing the service record information</param>
    /// <returns>String containing the newly created service record Id</returns>
    public async Task<ServiceRecordResponseModel> CreateAsync(string userId, ServiceRecordFormRequestModel model)
    {
        ServiceRecord serviceRecordToAdd = new ServiceRecord()
        {
            Title = model.Title,
            Description = model.Description,
            Cost = model.Cost,
            Mileage = model.Mileage,
            PerformedOn = model.PerformedOn,
            CreatedOn = DateTime.UtcNow,
            VehicleId = Guid.Parse(model.VehicleId),
            OwnerId = Guid.Parse(userId)

        }; 

        await repository.AddAsync<ServiceRecord>(serviceRecordToAdd);
        await repository.SaveChangesAsync();

        this.memoryCache.Remove(userId + UserServiceRecordsCacheKeyAddition);
        this.memoryCache.Remove(userId + UserServiceRecordsCountCacheKeyAddition);
        this.memoryCache.Remove(userId + UserServiceRecordsCostCacheKeyAddition);
        this.memoryCache.Remove(userId + UserServiceRecordsLastNCacheKeyAddition);

        return new ServiceRecordResponseModel
        {
            Id = serviceRecordToAdd.Id.ToString(),
            Title = serviceRecordToAdd.Title,
            Cost = serviceRecordToAdd.Cost,
            Description = serviceRecordToAdd.Description,
            Mileage = serviceRecordToAdd.Mileage,
            PerformedOn = serviceRecordToAdd.PerformedOn,
            VehicleId = serviceRecordToAdd.VehicleId.ToString()
        };
    }


    /// <summary>
    /// Edits a  service record
    /// </summary>
    /// <param name="serviceRecordId">The service record identifier</param>
    /// <param name="model">The input model containing the service record information</param>
    public async Task<ServiceRecordResponseModel> EditAsync(string serviceRecordId, string userId, ServiceRecordFormRequestModel model)
    {
        ServiceRecord recordToEdit = await repository.GetByIdAsync<ServiceRecord>(Guid.Parse(serviceRecordId));

        recordToEdit.Title = model.Title;
        recordToEdit.Description = model.Description;
        recordToEdit.Cost = model.Cost;
        recordToEdit.VehicleId = Guid.Parse(model.VehicleId);
        recordToEdit.Mileage = model.Mileage;
        recordToEdit.PerformedOn = model.PerformedOn;
        recordToEdit.ModifiedOn = DateTime.UtcNow;

        await repository.SaveChangesAsync();

        this.memoryCache.Remove(userId + UserServiceRecordsCacheKeyAddition);
        this.memoryCache.Remove(userId + UserServiceRecordsCostCacheKeyAddition);

        return new ServiceRecordResponseModel
        {
            Id = recordToEdit.Id.ToString(),
            Title = recordToEdit.Title,
            Cost = recordToEdit.Cost,
            Description = recordToEdit.Description,
            Mileage = recordToEdit.Mileage,
            PerformedOn = recordToEdit.PerformedOn,
            VehicleId = recordToEdit.VehicleId.ToString()
        };
    }

    /// <summary>
    /// Deletes a service record
    /// </summary>
    /// <param name="serviceRecordId">The service record identifier</param>
    public async Task DeleteAsync(string serviceRecordId, string userId)
    {
        ServiceRecord serviceRecordToDelete = await repository.GetByIdAsync<ServiceRecord>(Guid.Parse(serviceRecordId));

        repository.SoftDelete(serviceRecordToDelete);

        await repository.SaveChangesAsync();

        this.memoryCache.Remove(userId + UserServiceRecordsCacheKeyAddition);
        this.memoryCache.Remove(userId + UserServiceRecordsCountCacheKeyAddition);
        this.memoryCache.Remove(userId + UserServiceRecordsCostCacheKeyAddition);
        this.memoryCache.Remove(userId + UserServiceRecordsLastNCacheKeyAddition);
    }

    /// <summary>
    /// Retrieves all user service records ordered by date of their creation
    /// </summary>
    /// <param name="userId">The user identifier</param>
    /// <returns>A collection of service records</returns>
    public async Task<ICollection<ServiceRecordDetailsResponseModel>> GetAllByUserIdAsync(string userId)
    {
        ICollection<ServiceRecordDetailsResponseModel>? serviceRecords =
            this.memoryCache.Get<ICollection<ServiceRecordDetailsResponseModel>>(userId + UserServiceRecordsCacheKeyAddition);

        if(serviceRecords == null)
        {
           serviceRecords = await repository.AllReadonly<ServiceRecord>()
               .Where(sr => sr.IsDeleted == false && sr.OwnerId == Guid.Parse(userId))
               .OrderByDescending(sr => sr.CreatedOn)
               .Select(sr => new ServiceRecordDetailsResponseModel
               {
                   Id = sr.Id.ToString(),
                   Title = sr.Title,
                   Description = sr.Description,
                   Cost = sr.Cost,
                   Mileage = sr.Mileage,
                   PerformedOn = sr.PerformedOn,
                   VehicleMake = sr.Vehicle.Make,
                   VehicleModel = sr.Vehicle.Model
               })
               .ToListAsync();

            MemoryCacheEntryOptions options = new MemoryCacheEntryOptions()
               .SetSlidingExpiration(TimeSpan.FromMinutes(UserServiceRecordsCacheDuration));

            this.memoryCache.Set(userId + UserServiceRecordsCacheKeyAddition, serviceRecords, options);
        }

        return serviceRecords;
    }

    /// <summary>
    /// Retrieves all user service records as queryable
    /// </summary>
    /// <param name="userId">The user identifier</param>
    /// <returns>A queryable of service records</returns>
    public async Task<IQueryable<ServiceRecord>> GetAllByUserIdAsQueryableAsync(string userId)
    {
        return repository.AllReadonly<ServiceRecord>()
            .Where(sr => sr.IsDeleted == false && sr.OwnerId == Guid.Parse(userId))
            .AsQueryable();
    }

    /// <summary>
    /// Retrieves a user service record with details needed for editing
    /// </summary>
    /// <param name="serviceRecordId">The service record identifier</param>
    /// <returns>The service record model</returns>
    public async Task<ServiceRecordEditDetailsResponseModel> GetEditDetailsByIdAsync(string serviceRecordId)
    {
        return await repository.AllReadonly<ServiceRecord>()
               .Where(sr => sr.Id == Guid.Parse(serviceRecordId))
               .Select(sr => new ServiceRecordEditDetailsResponseModel
               {
                   Id = sr.Id.ToString(),
                   Title = sr.Title,
                   Description = sr.Description,
                   Cost = sr.Cost,
                   Mileage = sr.Mileage,
                   PerformedOn = sr.PerformedOn,
                   VehicleId = sr.VehicleId.ToString(),
               })
               .FirstAsync();
    }

    /// <summary>
    /// Checks if there exist a record and is not deleted 
    /// </summary>
    /// <param name="serviceRecordId">The service record identifier</param>
    /// <returns>Boolean based on the search result</returns>
    public async Task<bool> DoesRecordExistByIdAsync(string serviceRecordId)
    {
        return await repository.AllReadonly<ServiceRecord>()
               .Where(sr => sr.IsDeleted == false && sr.Id == Guid.Parse(serviceRecordId))
               .AnyAsync();
    }

    /// <summary>
    /// Checks if the user is the creator of the service record
    /// </summary>
    /// <param name="userId">The user identifier</param>
    /// <param name="serviceRecordId">The service record identifier</param>
    /// <returns>Boolean based on the search result</returns>
    public async Task<bool> IsUserRecordCreatorAsync(string userId, string serviceRecordId)
    {
        return await repository.AllReadonly<ServiceRecord>()
               .Where(sr => sr.OwnerId == Guid.Parse(userId) && sr.Id == Guid.Parse(serviceRecordId))
               .AnyAsync();
    }

    /// <summary>
    /// Retrieves all the user service records count
    /// </summary>
    /// <param name="userId">The user identifier</param>
    /// <returns>An integer containing the count of user service records</returns>
    public async Task<int> GetAllUserServiceRecordsCountAsync(string userId)
    {
        int serviceRecordsCount =
            this.memoryCache.Get<int>(userId + UserServiceRecordsCountCacheKeyAddition);

        if(serviceRecordsCount == 0)
        {
            serviceRecordsCount = await repository.AllReadonly<ServiceRecord>()
               .Where(tr => tr.IsDeleted == false && tr.OwnerId == Guid.Parse(userId))
               .CountAsync();

            MemoryCacheEntryOptions options = new MemoryCacheEntryOptions()
               .SetSlidingExpiration(TimeSpan.FromMinutes(UserServiceRecordsCountCacheDuration));

            this.memoryCache.Set(userId + UserServiceRecordsCountCacheKeyAddition, serviceRecordsCount, options);
        }

        return serviceRecordsCount;
    }

    /// <summary>
    /// Retrieves all the user service records cost
    /// </summary>
    /// <param name="userId">The user identifier</param>
    /// <returns>An decimal containing the cost of all the user service records</returns>
    public async Task<decimal> GetAllUserServiceRecordsCostAsync(string userId)
    {
        decimal serviceRecordsCost = 
            this.memoryCache.Get<decimal>(userId + UserServiceRecordsCostCacheKeyAddition);

        if(serviceRecordsCost == 0)
        {
            serviceRecordsCost = await repository.AllReadonly<ServiceRecord>()
               .Where(tr => tr.IsDeleted == false && tr.OwnerId == Guid.Parse(userId))
               .SumAsync(tr => tr.Cost);

            MemoryCacheEntryOptions options = new MemoryCacheEntryOptions()
              .SetSlidingExpiration(TimeSpan.FromMinutes(UserServiceRecordsCostCacheDuration));

            this.memoryCache.Set(userId + UserServiceRecordsCostCacheKeyAddition, serviceRecordsCost, options);
        }

        return serviceRecordsCost;
    }

    /// <summary>
    /// Retrieves a specified count of records containing basic information about the user service records
    /// ordered by time of creation
    /// </summary>
    /// <param name="userId">The user identifier</param>
    /// <param name="count">The amount of record to be retrieved</param>
    /// <returns>A collection of service records</returns>
    public async Task<ICollection<ServiceRecordBasicInformationResponseModel>> GetLastNCountAsync(string userId, int count)
    {

        ICollection<ServiceRecordBasicInformationResponseModel>? lastServiceRecords =
            this.memoryCache.Get<ICollection<ServiceRecordBasicInformationResponseModel>>(userId + UserServiceRecordsLastNCacheKeyAddition);

        if(lastServiceRecords == null)
        {
            lastServiceRecords = await repository.AllReadonly<ServiceRecord>()
               .Where(sr => sr.IsDeleted == false)
               .Where(sr => sr.OwnerId == Guid.Parse(userId))
               .OrderByDescending(sr => sr.CreatedOn)
               .Take(count)
               .Select(sr => new ServiceRecordBasicInformationResponseModel
               {
                   Id = sr.Id.ToString(),
                   Title = sr.Title,
                   PerformedOn = sr.PerformedOn,
                   VehicleMake = sr.Vehicle.Make,
                   VehicleModel = sr.Vehicle.Model

               })
               .ToListAsync();

            MemoryCacheEntryOptions options = new MemoryCacheEntryOptions()
           .SetSlidingExpiration(TimeSpan.FromMinutes(UserServiceRecordsLastNCacheDuration));

            this.memoryCache.Set(userId + UserServiceRecordsLastNCacheKeyAddition, lastServiceRecords, options);
        }

        return lastServiceRecords;
    }

    /// <summary>
    /// Retrieves a specified count of records containing basic information of the records for a vehicle
    /// ordered by time of creation
    /// </summary>
    /// <param name="vehicleId">The vehicle identifier</param>
    /// <param name="count">The amount of record to be retrieved</param>
    /// <returns>A collection of service records for a vehicle</returns>
    public async Task<ICollection<ServiceRecordBasicInformationResponseModel>> GetRecentByVehicleIdAsync(string vehicleId, int count)
    {

         return await repository.AllReadonly<ServiceRecord>()
               .Where(sr => sr.IsDeleted == false)
               .Where(sr => sr.VehicleId == Guid.Parse(vehicleId))
               .OrderByDescending(sr => sr.CreatedOn)
               .Take(count)
               .Select(sr => new ServiceRecordBasicInformationResponseModel
               {
                   Id = sr.Id.ToString(),
                   Title = sr.Title,
                   PerformedOn = sr.PerformedOn,
                   VehicleMake = sr.Vehicle.Make,
                   VehicleModel = sr.Vehicle.Model

               })
               .ToListAsync();

    }

    /// <summary>
    /// Retrieves a list of service records for a specified page.
    /// </summary>
    /// <param name="serviceRecords">The complete set of service records to paginate.</param>
    /// <param name="currentPage">The page number to retrieve.</param>
    /// <param name="recordPerPage">The number of records per page.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of ServiceRecordDetailsQueryResponseModel for the specified page.</returns>
    public async Task<List<ServiceRecordDetailsQueryResponseModel>> RetrieveServiceRecordsByPageAsync(IQueryable<ServiceRecord> serviceRecords, int currentPage, int recordPerPage)
    {
        return await serviceRecords
                     .Skip((currentPage - 1) * recordPerPage)
                     .Take(recordPerPage)
                     .Select(sr => new ServiceRecordDetailsQueryResponseModel
                     {
                         Id = sr.Id.ToString(),
                         Title = sr.Title,
                         Description = sr.Description,
                         Cost = sr.Cost,
                         Mileage = sr.Mileage,
                         PerformedOn = sr.PerformedOn,
                         VehicleMake = sr.Vehicle.Make,
                         VehicleModel = sr.Vehicle.Model
                     })
                     .ToListAsync();
    }
}
