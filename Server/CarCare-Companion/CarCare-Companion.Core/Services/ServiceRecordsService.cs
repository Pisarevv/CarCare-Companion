namespace CarCare_Companion.Core.Services;

using System.Threading.Tasks;

using CarCare_Companion.Core.Contracts;
using CarCare_Companion.Core.Models.ServiceRecords;
using CarCare_Companion.Infrastructure.Data.Common;
using CarCare_Companion.Infrastructure.Data.Models.Records;
using Microsoft.EntityFrameworkCore;
using static Common.GlobalConstants;

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
    /// Retrieves all user service records ordered by date of their creation
    /// </summary>
    /// <param name="userId">The user identifier</param>
    /// <returns>A collection of service records</returns>
    public async Task<ICollection<ServiceRecordResponseModel>> GetAllByUserIdAsync(string userId)
    {
        return await repository.AllReadonly<ServiceRecord>()
               .Where(sr => sr.IsDeleted == false)
               .OrderBy(sr => sr.PerformedOn)
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
}
