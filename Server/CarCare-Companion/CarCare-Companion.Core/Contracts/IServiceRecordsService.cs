namespace CarCare_Companion.Core.Contracts;

using CarCare_Companion.Core.Models.ServiceRecords;

public interface IServiceRecordsService
{
    public Task<string> CreateAsync(string userId, ServiceRecordFormRequestModel model);

    public Task<ICollection<ServiceRecordResponseModel>> GetAllByUserIdAsync(string userId);

    public Task EditAsync(string serviceRecordId, ServiceRecordFormRequestModel model);

    public Task<bool> DoesRecordExistById(string serviceRecordId);
}
