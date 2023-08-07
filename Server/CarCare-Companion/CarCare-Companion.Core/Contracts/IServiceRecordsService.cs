namespace CarCare_Companion.Core.Contracts;

using CarCare_Companion.Core.Models.ServiceRecords;

public interface IServiceRecordsService
{
    public Task CreateAsync(string userId, ServiceRecordFormRequestModel model);

    public Task<ICollection<ServiceRecordResponseModel>> GetAllByUserIdAsync(string userId);

    public Task EditAsync(string serviceRecordId, string userId, ServiceRecordFormRequestModel model);

    public Task DeleteAsync(string serviceRecordId, string userId);

    public Task<bool> DoesRecordExistByIdAsync(string serviceRecordId);

    public Task<bool> IsUserRecordCreatorAsync(string userId, string serviceRecordId);

    public Task<ServiceRecordEditDetailsResponseModel> GetEditDetailsByIdAsync(string serviceRecordId);

    public Task<int> GetAllUserServiceRecordsCountAsync(string userId);

    public Task<decimal> GetAllUserServiceRecordsCostAsync(string userId);

    public Task<ICollection<ServiceRecordBasicInformationResponseModel>> GetLastNCountAsync(string userId,int count);

    public Task<ICollection<ServiceRecordBasicInformationResponseModel>> GetRecentByVehicleId(string vehicleId, int count);
}
 