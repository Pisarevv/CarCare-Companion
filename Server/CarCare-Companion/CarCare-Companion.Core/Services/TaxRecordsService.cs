namespace CarCare_Companion.Core.Services;

using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using CarCare_Companion.Core.Contracts;
using CarCare_Companion.Core.Models.TaxRecords;
using CarCare_Companion.Infrastructure.Data.Common;
using CarCare_Companion.Infrastructure.Data.Models.Records;


public class TaxRecordsService : ITaxRecordsService
{
    private readonly IRepository repository;

    public TaxRecordsService(IRepository repository)
    {
        this.repository = repository;
    }

    /// <summary>
    /// Retrieves all user tax records
    /// </summary>
    /// <param name="userId">The user identifier</param>
    /// <returns>Collection of tax records</returns>
    public async Task<ICollection<TaxRecordResponseModel>> GetAllByUserIdAsync(string userId)
    {
        return await repository.AllReadonly<TaxRecord>()
               .Where(tr => tr.IsDeleted == false)
               .Where(tr => tr.OwnerId == Guid.Parse(userId))
               .OrderBy(tr => tr.CreatedOn) 
               .Select(tr => new TaxRecordResponseModel()
               {
                   Id = tr.Id.ToString(),
                   Title = tr.Title,
                   ValidFrom = tr.ValidFrom,
                   ValidTo = tr.ValidTo,
                   Cost = tr.Cost,
                   VehicleMake = tr.Vehicle.Make,
                   VehicleModel = tr.Vehicle.Model,
               })
               .ToListAsync();
    }

    /// <summary>
    /// Creates a new tax record
    /// </summary>
    /// <param name="model">The input model containing the tax record information</param>
    /// <param name="userId">The user identifier</param>
    /// <returns>String containing the newly created tax record Id</returns>
    public async Task<string> CreateAsync(string userId, TaxRecordFormRequestModel model)
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

        return recordToAdd.Id.ToString();
    }

    /// <summary>
    /// Edits a tax record
    /// </summary>
    /// <param name="model">The input model containing the tax record information</param>
    /// <param name="recordId">The tax record identifier</param>
    public async Task EditAsync(string recordId, TaxRecordFormRequestModel model)
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
                   VehicleId = tr.VehicleId.ToString()
               })
               .FirstAsync();

    }

   
}
