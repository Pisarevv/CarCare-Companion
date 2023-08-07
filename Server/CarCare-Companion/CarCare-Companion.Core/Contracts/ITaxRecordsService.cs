namespace CarCare_Companion.Core.Contracts;

using CarCare_Companion.Core.Models.TaxRecords;

public interface  ITaxRecordsService
{
    public Task CreateAsync(string userId, TaxRecordFormRequestModel model);

    public Task EditAsync(string recordId, string userId ,TaxRecordFormRequestModel model);

    public Task DeleteAsync(string recordId, string userId);

    public Task<ICollection<TaxRecordResponseModel>> GetAllByUserIdAsync(string userId);

    public Task<bool> DoesRecordExistByIdAsync(string recordId);

    public Task<bool> IsUserRecordCreatorAsync(string userId, string recordId);

    public Task<TaxRecordEditDetailsResponseModel> GetEditDetailsByIdAsync(string recordId);

    public Task<int> GetAllUserTaxRecordsCountAsync(string userId);

    public Task<decimal> GetAllUserTaxRecordsCostAsync (string userId);

    public Task<ICollection<UpcomingTaxRecordResponseModel>> GetUpcomingTaxesAsync(string userId, int count);

    public Task<ICollection<UpcomingUserTaxResponseModel>> GetUpcomingUsersTaxesAsync();
}

