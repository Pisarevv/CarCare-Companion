namespace CarCare_Companion.Core.Contracts;

using CarCare_Companion.Core.Enums;
using CarCare_Companion.Core.Models.Search;

/// <summary>
/// Represents a service responsible for searching records.
/// </summary>
public interface ISearchService
{
    /// <summary>
    /// Searches the records based on provided criteria.
    /// </summary>
    /// <param name="userId">The unique identifier of the user initiating the search.</param>
    /// <param name="category">The category filter to narrow down the search results.</param>
    /// <param name="searchTerm">Optional. The term to search for within records. If null, it does not filter results by term.</param>
    /// <param name="sorting">Specifies the order in which the results should be returned. Default is by newest records first.</param>
    /// <param name="currentPage">Specifies the current page number for pagination. Default is the first page.</param>
    /// <param name="recordsPerPage">Specifies the number of records to be returned per page. Default is one record per page.</param>
    /// <returns>A <see cref="RecordsQueryModel"/> containing the search results and relevant metadata.</returns>
    public Task<RecordsQueryModel> Search(string userId, RecordCategories category = RecordCategories.All, string? searchTerm = null, RecordsSorting sorting = RecordsSorting.Newest, int currentPage = 1, int recordsPerPage = 1);
}
