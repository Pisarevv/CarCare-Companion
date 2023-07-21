namespace CarCare_Companion.Core.Contracts;

using CarCare_Companion.Core.Models.TaxRecords;

public interface  ITaxRecordsService
{
    public Task<string> CreateAsync(string userId, TaxRecordFormRequestModel model);

    public Task EditAsync(string recordId,  TaxRecordFormRequestModel model);

    public Task<ICollection<TaxRecordResponseModel>> GetAllByUserIdAsync(string userId);

    public Task<bool> DoesRecordExistByIdAsync(string recordId);

    public Task<bool> IsUserRecordCreatorAsync(string userId, string recordId);

    public Task<TaxRecordEditDetailsResponseModel> GetEditDetailsByIdAsync(string recordId);
}

