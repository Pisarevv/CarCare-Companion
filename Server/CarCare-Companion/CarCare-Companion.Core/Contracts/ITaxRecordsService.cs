namespace CarCare_Companion.Core.Contracts;

using CarCare_Companion.Core.Models.TaxRecords;

public interface  ITaxRecordsService
{
    public Task<string> CreateAsync(string userId, TaxRecordFormRequestModel model);
}

