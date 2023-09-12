namespace CarCare_Companion.Core.Services;

using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

using CarCare_Companion.Core.Contracts;
using CarCare_Companion.Core.Models.TaxRecords;
using CarCare_Companion.Infrastructure.Data.Common;
using CarCare_Companion.Infrastructure.Data.Models.Records;

using static Common.CacheKeysAndDurations.TaxRecords;


public class TaxRecordsService : ITaxRecordsService
{
    private readonly IRepository repository;
    private readonly IMemoryCache memoryCache;

    public TaxRecordsService(IRepository repository, IMemoryCache memoryCache)
    {
        this.repository = repository;
        this.memoryCache = memoryCache;
    }



    /// <summary>
    /// Retrieves all user tax records
    /// </summary>
    /// <param name="userId">The user identifier</param>
    /// <returns>Collection of tax records</returns>
    public async Task<ICollection<TaxRecordDetailsResponseModel>> GetAllByUserIdAsync(string userId)
    {
        ICollection<TaxRecordDetailsResponseModel>? taxRecords = 
            this.memoryCache.Get<ICollection<TaxRecordDetailsResponseModel>>(userId + UserTaxRecordsCacheKeyAddition);

        if(taxRecords == null)
        {
            taxRecords = await repository.AllReadonly<TaxRecord>()
               .Where(tr => tr.IsDeleted == false)
               .Where(tr => tr.OwnerId == Guid.Parse(userId))
               .OrderByDescending(tr => tr.CreatedOn)
               .Select(tr => new TaxRecordDetailsResponseModel()
               {
                   Id = tr.Id.ToString(),
                   Title = tr.Title,
                   ValidFrom = tr.ValidFrom,
                   ValidTo = tr.ValidTo,
                   Cost = tr.Cost,
                   VehicleMake = tr.Vehicle.Make,
                   VehicleModel = tr.Vehicle.Model,
                   Description = tr.Description
               })
               .ToListAsync();

            MemoryCacheEntryOptions options = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(UserTaxRecordsCacheDuration));

            this.memoryCache.Set(userId + UserTaxRecordsCacheKeyAddition, taxRecords, options);
        }
        
        return taxRecords;
    }

    /// <summary>
    /// Retrieves all user tax records as queryable
    /// </summary>
    /// <param name="userId">The user identifier</param>
    /// <returns>A queryable of tax records</returns>
    public async Task<IQueryable<TaxRecord>> GetAllByUserIdAsQueryableAsync(string userId)
    {

        return repository.AllReadonly<TaxRecord>()
            .Where(sr => sr.IsDeleted == false && sr.OwnerId == Guid.Parse(userId))
            .AsQueryable();
    }

    /// <summary>
    /// Creates a new tax record
    /// </summary>
    /// <param name="model">The input model containing the tax record information</param>
    /// <param name="userId">The user identifier</param>
    /// <returns>String containing the newly created tax record Id</returns>
    public async Task<TaxRecordResponseModel> CreateAsync(string userId, TaxRecordFormRequestModel model)
    {
        TaxRecord recordToAdd = new TaxRecord()
        {
            Title = model.Title,
            ValidFrom = model.ValidFrom,
            ValidTo = model.ValidTo,
            Cost = model.Cost,
            Description = model.Description,
            VehicleId = Guid.Parse(model.VehicleId),
            OwnerId = Guid.Parse(userId),
            CreatedOn = DateTime.UtcNow
        };

        await repository.AddAsync(recordToAdd);
        await repository.SaveChangesAsync();


        this.memoryCache.Remove(userId + UserTaxRecordsCacheKeyAddition);
        this.memoryCache.Remove(userId + UserTaxRecordsCostCacheKeyAddition);
        this.memoryCache.Remove(userId + UserTaxRecordsCountCacheKeyAddition);
        this.memoryCache.Remove(userId + UserTaxRecordsUpcomingCacheKeyAddition);

        return new TaxRecordResponseModel
        {
            Id = recordToAdd.Id.ToString(),
            Title = recordToAdd.Title,
            ValidFrom = recordToAdd.ValidFrom,
            ValidTo = recordToAdd.ValidTo,
            Cost = recordToAdd.Cost,
            Description = recordToAdd.Description,
            VehicleId = recordToAdd.VehicleId.ToString(),
        };
    }

    /// <summary>
    /// Edits a tax record
    /// </summary>
    /// <param name="model">The input model containing the tax record information</param>
    /// <param name="recordId">The tax record identifier</param>
    public async Task<TaxRecordResponseModel> EditAsync(string recordId, string userId, TaxRecordFormRequestModel model)
    {
        TaxRecord recordToEdit = await repository.GetByIdAsync<TaxRecord>(Guid.Parse(recordId));

        recordToEdit.Title = model.Title;
        recordToEdit.ValidFrom = model.ValidFrom;
        recordToEdit.ValidTo = model.ValidTo;
        recordToEdit.Cost = model.Cost;
        recordToEdit.Description = model.Description;
        recordToEdit.VehicleId = Guid.Parse(model.VehicleId);
        recordToEdit.ModifiedOn = DateTime.UtcNow;

        await repository.SaveChangesAsync();

        this.memoryCache.Remove(userId + UserTaxRecordsCacheKeyAddition);
        this.memoryCache.Remove(userId + UserTaxRecordsCostCacheKeyAddition);
        this.memoryCache.Remove(userId + UserTaxRecordsUpcomingCacheKeyAddition);

        return new TaxRecordResponseModel
        {
            Id = recordToEdit.Id.ToString(),
            Title = recordToEdit.Title,
            ValidFrom = recordToEdit.ValidFrom,
            ValidTo = recordToEdit.ValidTo,
            Cost = recordToEdit.Cost,
            Description = recordToEdit.Description,
            VehicleId = recordToEdit.VehicleId.ToString(),
        };
    }

    /// <summary>
    /// Deletes a tax record
    /// </summary>
    /// <param name="recordId">The tax record identifier</param>
    public async Task DeleteAsync(string recordId, string userId)
    {
        TaxRecord taxRecordToDelete = await repository.GetByIdAsync<TaxRecord>(Guid.Parse(recordId));

        repository.SoftDelete(taxRecordToDelete);

        await repository.SaveChangesAsync();

        this.memoryCache.Remove(userId + UserTaxRecordsCacheKeyAddition);
        this.memoryCache.Remove(userId + UserTaxRecordsCostCacheKeyAddition);
        this.memoryCache.Remove(userId + UserTaxRecordsCountCacheKeyAddition);
        this.memoryCache.Remove(userId + UserTaxRecordsUpcomingCacheKeyAddition);
    }


    /// <summary>
    /// Checks if a tax record exists
    /// </summary>
    /// <param name="recordId">The tax record identifier</param>
    /// <returns>Boolean based on the search result</returns>
    public async Task<bool> DoesRecordExistByIdAsync(string recordId)
    {
        return await repository.AllReadonly<TaxRecord>()
               .Where(tr => tr.IsDeleted == false && tr.Id == Guid.Parse(recordId))
               .AnyAsync();
               
    }

    /// <summary>
    /// Checks if the user is the creator of the tax record
    /// </summary>
    /// <param name="userId">The user identifier</param>
    /// <param name="recordId">The tax record identifier</param>
    /// <returns>Boolean based on the search result</returns>
    public async Task<bool> IsUserRecordCreatorAsync(string userId, string recordId)
    {
        return await repository.AllReadonly<TaxRecord>()
              .Where(tr => tr.OwnerId == Guid.Parse(userId) && tr.Id == Guid.Parse(recordId))
              .AnyAsync();
    }

    /// <summary>
    /// Retrieves the tax record details to the user
    /// </summary>
    /// <param name="recordId">The tax record identifier</param>
    /// <returns>Detailed model containing all the tax record information</returns>
    public async Task<TaxRecordEditDetailsResponseModel> GetEditDetailsByIdAsync(string recordId)
    {
        return await repository.AllReadonly<TaxRecord>()
               .Where(tr => tr.Id == Guid.Parse(recordId))
               .Select(tr => new TaxRecordEditDetailsResponseModel
               {
                   Id = tr.Id.ToString(),
                   Title = tr.Title,
                   ValidFrom = tr.ValidFrom,
                   ValidTo = tr.ValidTo,
                   Cost = tr.Cost,
                   VehicleMake = tr.Vehicle.Make,
                   VehicleModel = tr.Vehicle.Model,
                   VehicleId = tr.VehicleId.ToString(),
                   Description = tr.Description
               })
               .FirstAsync();

    }

    /// <summary>
    /// Retrieves all the user tax records count
    /// </summary>
    /// <param name="userId">The user identifier</param>
    /// <returns>An integer containing the count of user tax records</returns>
    public async Task<int> GetAllUserTaxRecordsCountAsync(string userId)
    {
        int taxRecordsCount =
            this.memoryCache.Get<int>(userId + UserTaxRecordsCountCacheKeyAddition);

        if(taxRecordsCount == 0)
        {
            taxRecordsCount = await repository.AllReadonly<TaxRecord>()
               .Where(tr => tr.IsDeleted == false && tr.OwnerId == Guid.Parse(userId))
               .CountAsync();

            MemoryCacheEntryOptions options = new MemoryCacheEntryOptions()
               .SetSlidingExpiration(TimeSpan.FromMinutes(UserTaxRecordsCountCacheDuration));

            this.memoryCache.Set(userId + UserTaxRecordsCountCacheKeyAddition, taxRecordsCount, options);
        }

        return taxRecordsCount;
    }

    /// <summary>
    /// Retrieves all the user tax records cost
    /// </summary>
    /// <param name="userId">The user identifier</param>
    /// <returns>An decimal containing the cost of all the user tax records</returns>
    public async Task<decimal> GetAllUserTaxRecordsCostAsync(string userId)
    {
        decimal taxRecordsCost =
            this.memoryCache.Get<decimal>(userId + UserTaxRecordsCostCacheKeyAddition);

        if(taxRecordsCost == 0)
        {
            taxRecordsCost = await repository.AllReadonly<TaxRecord>()
               .Where(tr => tr.IsDeleted == false && tr.OwnerId == Guid.Parse(userId))
               .SumAsync(tr => tr.Cost);

            MemoryCacheEntryOptions options = new MemoryCacheEntryOptions()
              .SetSlidingExpiration(TimeSpan.FromMinutes(UserTaxRecordsCostCacheDuration));

            this.memoryCache.Set(userId + UserTaxRecordsCostCacheKeyAddition, taxRecordsCost, options);
        }

        return taxRecordsCost;
    }

    /// <summary>
    /// Retrieves a specified count of user upcoming taxes based on a time period
    /// </summary>
    /// <param name="userId">The user identifier</param>
    /// <param name="count">The amount of records to be retrieved</param>
    /// <returns>Collection of upcoming taxes</returns>
    public async Task<ICollection<UpcomingTaxRecordResponseModel>> GetUpcomingTaxesAsync(string userId, int count)
    {
        ICollection<UpcomingTaxRecordResponseModel>? upcomingTaxes =
            this.memoryCache.Get<ICollection<UpcomingTaxRecordResponseModel>>(userId + UserTaxRecordsUpcomingCacheKeyAddition);

        if(upcomingTaxes == null)
        {
            DateTime filterDay = DateTime.Today.AddMonths(2);

            DateTime startDateFilter = DateTime.UtcNow;
            DateTime endDateFilter = new DateTime(filterDay.Year, filterDay.Month, filterDay.Day, 23, 59, 59);

            upcomingTaxes =  await repository.AllReadonly<TaxRecord>()
                   .Where(tr => tr.IsDeleted == false && tr.OwnerId == Guid.Parse(userId))
                   .Where(tr => tr.ValidTo >= startDateFilter && tr.ValidTo <= endDateFilter)
                   .OrderBy(tr => tr.ValidTo)
                   .Take(count)
                   .Select(tr => new UpcomingTaxRecordResponseModel
                   {
                       Id = tr.Id.ToString(),
                       Title = tr.Title,
                       ValidTo = tr.ValidTo,
                       VehicleMake = tr.Vehicle.Make,
                       VehicleModel = tr.Vehicle.Model
                   })
                   .ToListAsync();

            MemoryCacheEntryOptions options = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromMinutes(UserTaxRecordsUpcomingCacheDuration));

            this.memoryCache.Set(userId + UserTaxRecordsUpcomingCacheKeyAddition, upcomingTaxes, options);
        }

        return upcomingTaxes;
        
    }

    /// <summary>
    /// Retrieves users and their taxes that are expiring the next day ordered by the time of record creation
    /// </summary>
    /// <returns>Collection of taxes that are expiring the next day</returns>
    public async Task<ICollection<UpcomingUserTaxResponseModel>> GetUpcomingUsersTaxesAsync()
    {

        DateTime filterDay = DateTime.Today.AddDays(1);

        DateTime startDateFilter = new DateTime(filterDay.Year, filterDay.Month, filterDay.Day, 0, 0, 0);
        DateTime endDateFilter = new DateTime(filterDay.Year, filterDay.Month, filterDay.Day, 23,59, 59);

        return await repository.AllReadonly<TaxRecord>()
               .Where(tr => tr.IsDeleted == false)
               .Where(tr => tr.ValidTo >= startDateFilter && tr.ValidTo <= endDateFilter)
               .OrderBy(tr => tr.CreatedOn)
               .Select(tr => new UpcomingUserTaxResponseModel
               {
                   Email = tr.Owner.Email,
                   FirstName = tr.Owner.FirstName,
                   LastName = tr.Owner.LastName,
                   TaxName = tr.Title,
                   TaxValidTo = tr.ValidTo.ToString("dd/MM/yyyy"),
                   VehicleMake = tr.Vehicle.Make,
                   VehicleModel = tr.Vehicle.Model
               })
               .ToListAsync();

    }

    /// <summary>
    /// Retrieves a list of tax records for a specified page.
    /// </summary>
    /// <param name="taxRecords">The complete set of tax records to paginate.</param>
    /// <param name="currentPage">The page number to retrieve.</param>
    /// <param name="recordPerPage">The number of records per page.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of TaxRecordDetailsResponseModel for the specified page.</returns>
    public async Task<List<TaxRecordDetailsResponseModel>> RetrieveTaxRecordsByPage(IQueryable<TaxRecord> taxRecords, int currentPage, int recordPerPage)
    {
        return await taxRecords
                     .Skip((currentPage - 1) * recordPerPage)
                     .Take(recordPerPage)
                     .Select(tr => new TaxRecordDetailsResponseModel
                     {
                         Id = tr.Id.ToString(),
                         Title = tr.Title,
                         Description = tr.Description,
                         Cost = tr.Cost,
                         ValidFrom = tr.ValidFrom,
                         ValidTo = tr.ValidTo,
                         VehicleMake = tr.Vehicle.Make,
                         VehicleModel = tr.Vehicle.Model
                     })
                     .ToListAsync();
    }
}
