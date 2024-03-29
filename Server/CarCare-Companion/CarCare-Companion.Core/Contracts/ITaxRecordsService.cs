﻿namespace CarCare_Companion.Core.Contracts;

using CarCare_Companion.Core.Models.Search;
using CarCare_Companion.Core.Models.TaxRecords;
using CarCare_Companion.Infrastructure.Data.Models.Records;

public interface ITaxRecordsService
{
    /// <summary>
    /// Creates a new tax record for a user asynchronously.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="model">The model containing the tax record details.</param>
    /// <returns>The created TaxRecordResponseModel.</returns>
    public Task<TaxRecordResponseModel> CreateAsync(string userId, TaxRecordFormRequestModel model);

    /// <summary>
    /// Edits an existing tax record for a user asynchronously.
    /// </summary>
    /// <param name="recordId">The ID of the tax record to edit.</param>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="model">The model containing the updated tax record details.</param>
    /// <returns>The edited TaxRecordResponseModel.</returns>
    public Task<TaxRecordResponseModel> EditAsync(string recordId, string userId, TaxRecordFormRequestModel model);

    /// <summary>
    /// Deletes a tax record based on its ID and the associated user's ID asynchronously.
    /// </summary>
    /// <param name="recordId">The ID of the tax record to delete.</param>
    /// <param name="userId">The ID of the associated user.</param>
    public Task DeleteAsync(string recordId, string userId);

    /// <summary>
    /// Retrieves all tax records associated with a specific user ID asynchronously.
    /// </summary>
    /// <param name="userId">The user's ID.</param>
    /// <returns>A collection of TaxRecordDetailsResponseModels.</returns>
    public Task<ICollection<TaxRecordDetailsResponseModel>> GetAllByUserIdAsync(string userId);

    /// <summary>
    /// Retrieves all user tax records as queryable
    /// </summary>
    /// <param name="userId">The user identifier</param>
    /// <returns>A queryable of tax records</returns>
    public Task<IQueryable<TaxRecord>> GetAllByUserIdAsQueryableAsync(string userId);


    /// <summary>
    /// Checks if a specific tax record exists based on its ID asynchronously.
    /// </summary>
    /// <param name="recordId">The ID of the tax record to check.</param>
    /// <returns>True if the tax record exists, otherwise false.</returns>
    public Task<bool> DoesRecordExistByIdAsync(string recordId);

    /// <summary>
    /// Verifies if a user is the creator of a specific tax record asynchronously.
    /// </summary>
    /// <param name="userId">The user's ID.</param>
    /// <param name="recordId">The ID of the tax record to check.</param>
    /// <returns>True if the user is the creator of the tax record, otherwise false.</returns>
    public Task<bool> IsUserRecordCreatorAsync(string userId, string recordId);

    /// <summary>
    /// Retrieves the edit details of a tax record based on its ID asynchronously.
    /// </summary>
    /// <param name="recordId">The ID of the tax record.</param>
    /// <returns>The TaxRecordEditDetailsResponseModel containing the record's details.</returns>
    public Task<TaxRecordEditDetailsResponseModel> GetEditDetailsByIdAsync(string recordId);

    /// <summary>
    /// Counts all tax records associated with a specific user ID asynchronously.
    /// </summary>
    /// <param name="userId">The user's ID.</param>
    /// <returns>The count of all tax records for the user.</returns>
    public Task<int> GetAllUserTaxRecordsCountAsync(string userId);

    /// <summary>
    /// Sums the total cost of all tax records associated with a specific user ID asynchronously.
    /// </summary>
    /// <param name="userId">The user's ID.</param>
    /// <returns>The total cost of all tax records for the user.</returns>
    public Task<decimal> GetAllUserTaxRecordsCostAsync(string userId);

    /// <summary>
    /// Retrieves the upcoming tax records for a user based on a specific count asynchronously.
    /// </summary>
    /// <param name="userId">The user's ID.</param>
    /// <param name="count">The number of upcoming tax records to fetch.</param>
    /// <returns>A collection of UpcomingTaxRecordResponseModels.</returns>
    public Task<ICollection<UpcomingTaxRecordResponseModel>> GetUpcomingTaxesAsync(string userId, int count);

    /// <summary>
    /// Retrieves all upcoming tax records for all users asynchronously.
    /// </summary>
    /// <returns>A collection of UpcomingUserTaxResponseModels.</returns>
    public Task<ICollection<UpcomingUserTaxResponseModel>> GetUpcomingUsersTaxesAsync();

    /// <summary>
    /// Retrieves a list of tax records for a specified page.
    /// </summary>
    /// <param name="taxRecords">The complete set of tax records to paginate.</param>
    /// <param name="currentPage">The page number to retrieve.</param>
    /// <param name="recordPerPage">The number of records per page.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of TaxRecordDetailsQueryResponseModel for the specified page.</returns>
    public Task<List<TaxRecordDetailsQueryResponseModel>> RetrieveTaxRecordsByPageAsync(IQueryable<TaxRecord> taxRecords, int currentPage, int recordPerPage);

    /// <summary>
    /// Asynchronously retrieves a specified number of additional tax records from a given page.
    /// </summary>
    /// <param name="taxRecords">An IQueryable of TaxRecord from which to retrieve the records.</param>
    /// <param name="currentPage">The current page number.</param>
    /// <param name="recordsPerType">The number of records per type on a page.</param>
    /// <param name="additionalRecordsNeeded">The number of additional records to retrieve.</param>
    /// <returns>
    /// A list of TaxRecordDetailsQueryResponseModel, representing the additional tax records retrieved.
    /// </returns>
    public Task<IList<TaxRecordDetailsQueryResponseModel>> RetrieveAdditionalTaxRecordsByPageAsync(IQueryable<TaxRecord> taxRecords, int currentPage, int recordsPerType, int additionalRecordsNeeded);

    /// <summary>
    /// Filters the provided collection of TaxRecords based on a search term.
    /// The method performs a case-insensitive search on the Title, 
    /// Description, Vehicle Make, and Vehicle Model fields of each TaxRecord.
    /// </summary>
    /// <param name="taxRecords">The collection of TaxRecords to filter.</param>
    /// <param name="inputSearchTerm">The search term to use for filtering the TaxRecords.</param>
    /// <returns>A filtered collection of TaxRecords where any of the specified fields match the search term.</returns>
    public IQueryable<TaxRecord> FilterRecordsBySearchTerm(IQueryable<TaxRecord> taxRecords, string inputSearchTerm);
}


