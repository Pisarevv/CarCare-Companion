namespace CarCare_Companion.Core.Services;

using System.Threading.Tasks;

using CarCare_Companion.Core.Contracts;
using CarCare_Companion.Core.Models.ServiceRecords;
using CarCare_Companion.Infrastructure.Data.Common;
using CarCare_Companion.Infrastructure.Data.Models.Records;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// The ServiceRecordsService is responsible for operations regarding the service records-related actions
/// </summary>
public class ServiceRecordsService : IServiceRecordsService
{
    private readonly IRepository repository;

    public ServiceRecordsService(IRepository repository)
    {
        this.repository = repository;
    }

    /// <summary>
    /// Creates a new service record
    /// </summary>
    /// <param name="model">The input model containing the service record information</param>
    /// <returns>String containing the newly created service record Id</returns>
    public async Task<string> CreateAsync(string userId, ServiceRecordFormRequestModel model)
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

        return serviceRecordToAdd.Id.ToString();
    }


    /// <summary>
    /// Edits a  service record
    /// </summary>
    /// <param name="serviceRecordId">The service record identifier</param>
    /// <param name="model">The input model containing the service record information</param>
    public async Task EditAsync(string serviceRecordId, ServiceRecordFormRequestModel model)
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
       
    }

    /// <summary>
    /// Deletes a service record
    /// </summary>
    /// <param name="serviceRecordId">The service record identifier</param>
    public async Task DeleteAsync(string serviceRecordId)
    {
        ServiceRecord serviceRecordToDelete = await repository.GetByIdAsync<ServiceRecord>(Guid.Parse(serviceRecordId));

        repository.SoftDelete(serviceRecordToDelete);

        await repository.SaveChangesAsync();
    }

    /// <summary>
    /// Retrieves all user service records ordered by date of their creation
    /// </summary>
    /// <param name="userId">The user identifier</param>
    /// <returns>A collection of service records</returns>
    public async Task<ICollection<ServiceRecordResponseModel>> GetAllByUserIdAsync(string userId)
    {
        return await repository.AllReadonly<ServiceRecord>()
               .Where(sr => sr.IsDeleted == false)
               .OrderByDescending(sr => sr.CreatedOn)
               .Select(sr => new ServiceRecordResponseModel
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
        return await repository.AllReadonly<ServiceRecord>()
               .Where(tr => tr.IsDeleted == false && tr.OwnerId == Guid.Parse(userId))
               .CountAsync();
    }

    /// <summary>
    /// Retrieves all the user service records cost
    /// </summary>
    /// <param name="userId">The user identifier</param>
    /// <returns>An decimal containing the cost of all the user service records</returns>
    public async Task<decimal> GetAllUserServiceRecordsCostAsync(string userId)
    {
        return await repository.AllReadonly<ServiceRecord>()
               .Where(tr => tr.IsDeleted == false && tr.OwnerId == Guid.Parse(userId))
               .SumAsync(tr => tr.Cost);
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
        return await repository.AllReadonly<ServiceRecord>()
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
    }
}
