namespace CarCare_Companion.Core.Services;

using System.Threading.Tasks;

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
}
