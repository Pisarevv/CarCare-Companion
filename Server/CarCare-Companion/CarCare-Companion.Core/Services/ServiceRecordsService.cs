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

    public async Task<string> CreateAsync(string userId, ServiceRecordFormRequestModel model)
    {
        ServiceRecord serviceRecordToAdd = new ServiceRecord()
        {
            Title = model.Title,
            Description = model.Description,
            Cost = model.Cost,
            Mileage = model.Mileage,
            //PerformedOn = DateTime.ParseExact(model.PerformedOn, DefaultDateFormat, null),
            VehicleId = Guid.Parse(model.VehicleId),
            OwnerId = Guid.Parse(userId)

        }; 

        await repository.AddAsync<ServiceRecord>(serviceRecordToAdd);
        await repository.SaveChangesAsync();

        return serviceRecordToAdd.Id.ToString();
    }

    public async Task<ICollection<ServiceRecordResponseModel>> GetAllByUserIdAsync(string userId)
    {
        return await repository.AllReadonly<ServiceRecord>()
               .Where(sr => sr.IsDeleted == false)
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
