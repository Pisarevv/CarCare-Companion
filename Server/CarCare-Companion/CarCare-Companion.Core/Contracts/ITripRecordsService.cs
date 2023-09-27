namespace CarCare_Companion.Core.Contracts;

using CarCare_Companion.Core.Models.Search;
using CarCare_Companion.Core.Models.Trip;
using CarCare_Companion.Core.Models.TripRecords;
using CarCare_Companion.Infrastructure.Data.Models.Records;

public interface ITripRecordsService
{
    /// <summary>
    /// Creates a new trip record for a user asynchronously.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="model">The model containing the trip record details.</param>
    /// <returns>The created TripResponseModel.</returns>
    public Task<TripResponseModel> CreateAsync(string userId, TripFormRequestModel model);

    /// <summary>
    /// Edits an existing trip record for a user asynchronously.
    /// </summary>
    /// <param name="tripId">The ID of the trip record to edit.</param>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="model">The model containing the updated trip record details.</param>
    /// <returns>The edited TripResponseModel.</returns>
    public Task<TripResponseModel> EditAsync(string tripId, string userId, TripFormRequestModel model);

    /// <summary>
    /// Deletes a trip record based on its ID and the associated user's ID asynchronously.
    /// </summary>
    /// <param name="tripId">The ID of the trip record to delete.</param>
    /// <param name="userId">The ID of the associated user.</param>
    public Task DeleteAsync(string tripId, string userId);

    /// <summary>
    /// Retrieves the edit details of a trip record based on its ID asynchronously.
    /// </summary>
    /// <param name="tripId">The ID of the trip record.</param>
    /// <returns>The TripEditDetailsResponseModel containing the record's details.</returns>
    public Task<TripEditDetailsResponseModel> GetTripDetailsByIdAsync(string tripId);

    /// <summary>
    /// Checks if a specific trip record exists based on its ID asynchronously.
    /// </summary>
    /// <param name="tripId">The ID of the trip record to check.</param>
    /// <returns>True if the trip record exists, otherwise false.</returns>
    public Task<bool> DoesTripExistByIdAsync(string tripId);

    /// <summary>
    /// Verifies if a user is the creator of a specific trip record asynchronously.
    /// </summary>
    /// <param name="userId">The user's ID.</param>
    /// <param name="tripId">The ID of the trip record to check.</param>
    /// <returns>True if the user is the creator of the trip record, otherwise false.</returns>
    public Task<bool> IsUserCreatorOfTripAsync(string userId, string tripId);

    /// <summary>
    /// Retrieves all trip records associated with a specific user ID asynchronously.
    /// </summary>
    /// <param name="userId">The user's ID.</param>
    /// <returns>A collection of TripDetailsByUserResponseModels.</returns>
    public Task<ICollection<TripDetailsByUserResponseModel>> GetAllTripsByUsedIdAsync(string userId);

    /// <summary>
    /// Retrieves all user trip records as queryable
    /// </summary>
    /// <param name="userId">The user identifier</param>
    /// <returns>A queryable of trip records</returns>
    public Task<IQueryable<TripRecord>> GetAllByUserIdAsQueryableAsync(string userId);

    /// <summary>
    /// Retrieves the most recent trip records for a user based on a specific count asynchronously.
    /// </summary>
    /// <param name="userId">The user's ID.</param>
    /// <param name="count">The number of recent trip records to fetch.</param>
    /// <returns>A collection of TripBasicInformationByUserResponseModels.</returns>
    public Task<ICollection<TripBasicInformationByUserResponseModel>> GetLastNCountAsync(string userId, int count);

    /// <summary>
    /// Counts all trip records associated with a specific user ID asynchronously.
    /// </summary>
    /// <param name="userId">The user's ID.</param>
    /// <returns>The count of all trip records for the user.</returns>
    public Task<int> GetAllUserTripsCountAsync(string userId);

    /// <summary>
    /// Sums the total cost of all trip records associated with a specific user ID asynchronously.
    /// </summary>
    /// <param name="userId">The user's ID.</param>
    /// <returns>The total cost of all trip records for the user.</returns>
    public Task<decimal?> GetAllUserTripsCostAsync(string userId);

    /// <summary>
    /// Retrieves a list of trip records for a specified page.
    /// </summary>
    /// <param name="tripRecords">The complete set of trip records to paginate.</param>
    /// <param name="currentPage">The page number to retrieve.</param>
    /// <param name="recordPerPage">The number of records per page.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of TripDetailsByUserQueryResponseModel for the specified page.</returns>
    public Task<List<TripDetailsByUserQueryResponseModel>> RetrieveTripRecordsByPageAsync(IQueryable<TripRecord> tripRecords, int currentPage, int recordPerPage);


    /// <summary>
    /// Asynchronously retrieves a specified number of additional trip records from a given page.
    /// </summary>
    /// <param name="tripRecords">An IQueryable of TripRecord from which to retrieve the records.</param>
    /// <param name="currentPage">The current page number.</param>
    /// <param name="recordsPerType">The number of records per type on a page.</param>
    /// <param name="additionalRecordsNeeded">The number of additional records to retrieve.</param>
    /// <returns>
    /// A list of TripDetailsByUserQueryResponseModel, representing the additional trip records retrieved.
    /// </returns>
    public Task<IList<TripDetailsByUserQueryResponseModel>> RetrieveAdditionalTripRecordsByPageAsync(IQueryable<TripRecord> tripRecords, int currentPage, int recordsPerType, int additionalRecordsNeeded);

    /// <summary>
    /// Filters the provided collection of TripRecords based on a search term.
    /// The method performs a case-insensitive search on the StartDestination, 
    /// EndDestination, Vehicle Make, and Vehicle Model fields of each TripRecord.
    /// </summary>
    /// <param name="tripRecords">The collection of TripRecords to filter.</param>
    /// <param name="inputSearchTerm">The search term to use for filtering the TripRecords.</param>
    /// <returns>A filtered collection of TripRecords where any of the specified fields match the search term.</returns>
    public IQueryable<TripRecord> FilterRecordsBySearchTerm(IQueryable<TripRecord> tripRecords, string inputSearchTerm);
}

