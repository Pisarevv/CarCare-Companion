﻿namespace CarCare_Companion.Core.Contracts;

using CarCare_Companion.Core.Models.Search;
using CarCare_Companion.Core.Models.ServiceRecords;
using CarCare_Companion.Infrastructure.Data.Models.Records;

public interface IServiceRecordsService
{
    /// <summary>
    /// Creates a new service record for a user asynchronously.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="model">The model containing the service record details.</param>
    /// <returns>The created ServiceRecordResponseModel.</returns>
    public Task<ServiceRecordResponseModel> CreateAsync(string userId, ServiceRecordFormRequestModel model);

    /// <summary>
    /// Retrieves all service records associated with a specific user ID asynchronously.
    /// </summary>
    /// <param name="userId">The user's ID.</param>
    /// <returns>A collection of ServiceRecordDetailsResponseModels.</returns>
    public Task<ICollection<ServiceRecordDetailsResponseModel>> GetAllByUserIdAsync(string userId);

    /// <summary>
    /// Retrieves all service records associated with a specific user ID  as Queryable asynchronously.
    /// </summary>
    /// <param name="userId">The user's ID</param>
    /// <returns>A queryable of ServiceRecords</returns>
    public Task<IQueryable<ServiceRecord>> GetAllByUserIdAsQueryableAsync(string userId);

    /// <summary>
    /// Edits an existing service record for a user asynchronously.
    /// </summary>
    /// <param name="serviceRecordId">The ID of the service record to edit.</param>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="model">The model containing the updated service record details.</param>
    /// <returns>The edited ServiceRecordResponseModel.</returns>
    public Task<ServiceRecordResponseModel> EditAsync(string serviceRecordId, string userId, ServiceRecordFormRequestModel model);

    /// <summary>
    /// Deletes a service record based on its ID and the associated user's ID asynchronously.
    /// </summary>
    /// <param name="serviceRecordId">The ID of the service record to delete.</param>
    /// <param name="userId">The ID of the associated user.</param>
    public Task DeleteAsync(string serviceRecordId, string userId);

    /// <summary>
    /// Checks if a specific service record exists based on its ID asynchronously.
    /// </summary>
    /// <param name="serviceRecordId">The ID of the service record to check.</param>
    /// <returns>True if the service record exists, otherwise false.</returns>
    public Task<bool> DoesRecordExistByIdAsync(string serviceRecordId);

    /// <summary>
    /// Verifies if a user is the creator of a specific service record asynchronously.
    /// </summary>
    /// <param name="userId">The user's ID.</param>
    /// <param name="serviceRecordId">The ID of the service record to check.</param>
    /// <returns>True if the user is the creator of the service record, otherwise false.</returns>
    public Task<bool> IsUserRecordCreatorAsync(string userId, string serviceRecordId);

    /// <summary>
    /// Retrieves the edit details of a service record based on its ID asynchronously.
    /// </summary>
    /// <param name="serviceRecordId">The ID of the service record.</param>
    /// <returns>The ServiceRecordEditDetailsResponseModel containing the record's details.</returns>
    public Task<ServiceRecordEditDetailsResponseModel> GetEditDetailsByIdAsync(string serviceRecordId);

    /// <summary>
    /// Counts all service records associated with a specific user ID asynchronously.
    /// </summary>
    /// <param name="userId">The user's ID.</param>
    /// <returns>The count of all service records for the user.</returns>
    public Task<int> GetAllUserServiceRecordsCountAsync(string userId);

    /// <summary>
    /// Sums the total cost of all service records associated with a specific user ID asynchronously.
    /// </summary>
    /// <param name="userId">The user's ID.</param>
    /// <returns>The total cost of all service records for the user.</returns>
    public Task<decimal> GetAllUserServiceRecordsCostAsync(string userId);

    /// <summary>
    /// Retrieves the last 'N' number of service records associated with a specific user ID asynchronously.
    /// </summary>
    /// <param name="userId">The user's ID.</param>
    /// <param name="count">The number of recent records to fetch.</param>
    /// <returns>A collection of the last 'N' ServiceRecordBasicInformationResponseModels for the user.</returns>
    public Task<ICollection<ServiceRecordBasicInformationResponseModel>> GetLastNCountAsync(string userId, int count);

    /// <summary>
    /// Retrieves the most recent service records associated with a specific vehicle ID asynchronously.
    /// </summary>
    /// <param name="vehicleId">The vehicle's ID.</param>
    /// <param name="count">The number of recent records to fetch.</param>
    /// <returns>A collection of the most recent ServiceRecordBasicInformationResponseModels for the vehicle.</returns>
    public Task<ICollection<ServiceRecordBasicInformationResponseModel>> GetRecentByVehicleIdAsync(string vehicleId, int count);

    /// <summary>
    /// Retrieves a list of service records for a specified page.
    /// </summary>
    /// <param name="serviceRecords">The complete set of service records to paginate.</param>
    /// <param name="currentPage">The page number to retrieve.</param>
    /// <param name="recordPerPage">The number of records per page.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of ServiceRecordDetailsQueryResponseModel for the specified page.</returns>
    public Task<List<ServiceRecordDetailsQueryResponseModel>> RetrieveServiceRecordsByPageAsync(IQueryable<ServiceRecord> serviceRecords, int currentPage, int recordPerPage);

    /// <summary>
    /// Asynchronously retrieves a specified number of additional service records from a given page.
    /// </summary>
    /// <param name="serviceRecords">An IQueryable of ServiceRecord from which to retrieve the records.</param>
    /// <param name="currentPage">The current page number.</param>
    /// <param name="recordsPerType">The number of records per type on a page.</param>
    /// <param name="additionalRecordsNeeded">The number of additional records to retrieve.</param>
    /// <returns>
    /// A list of ServiceRecordDetailsQueryResponseModel, representing the additional service records retrieved.
    /// </returns>
    public Task<IList<ServiceRecordDetailsQueryResponseModel>> RetrieveAdditionalServiceRecordsByPageAsync(IQueryable<ServiceRecord> serviceRecords, int currentPage, int recordsPerType, int additionalRecordsNeeded);


    /// <summary>
    /// Filters the provided collection of ServiceRecords based on a search term.
    /// The method performs a case-insensitive search on the Title, 
    /// Description, Vehicle Make, and Vehicle Model fields of each ServiceRecord.
    /// </summary>
    /// <param name="serviceRecords">The collection of ServiceRecords to filter.</param>
    /// <param name="inputSearchTerm">The search term to use for filtering the ServiceRecords.</param>
    /// <returns>A filtered collection of ServiceRecords where any of the specified fields match the search term.</returns>
    public IQueryable<ServiceRecord> FilterRecordsBySearchTerm(IQueryable<ServiceRecord> serviceRecords, string inputSearchTerm);
}

