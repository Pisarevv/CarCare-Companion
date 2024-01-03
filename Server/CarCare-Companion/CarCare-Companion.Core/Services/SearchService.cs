namespace CarCare_Companion.Core.Services;


using CarCare_Companion.Core.Contracts;
using CarCare_Companion.Core.Enums;
using CarCare_Companion.Core.Models.Search;
using CarCare_Companion.Infrastructure.Data.Models.Records;
using System;



/// <summary>
/// Service for searching tax, service, and trip records.
/// </summary>
public class SearchService : ISearchService
{
    private readonly ITaxRecordsService taxRecordsService;
    private readonly IServiceRecordsService serviceRecordsService;
    private readonly ITripRecordsService tripRecordsService;
    private readonly ISortService sortService;

    private IQueryable<TripRecord> tripRecords;
    private IQueryable<TaxRecord> taxRecords;
    private IQueryable<ServiceRecord> serviceRecords;

    RecordsQueryModel result;

    public SearchService(ITaxRecordsService taxRecordsService, IServiceRecordsService serviceRecordsService, ITripRecordsService tripRecordsService, ISortService sortService)
    {
        this.taxRecordsService = taxRecordsService;
        this.serviceRecordsService = serviceRecordsService;
        this.tripRecordsService = tripRecordsService;
        this.sortService = sortService;

        this.tripRecords = Enumerable.Empty<TripRecord>().AsQueryable();
        this.taxRecords =  Enumerable.Empty<TaxRecord>().AsQueryable();
        this.serviceRecords = Enumerable.Empty<ServiceRecord>().AsQueryable();

        this.result = new RecordsQueryModel();
    }

    /// <summary>
    /// Searches the records of a user based on specified criteria.
    /// </summary>
    /// <param name="userId">The identifier of the user.</param>
    /// <param name="category">The category of records to search. Default is all categories.</param>
    /// <param name="searchTerm">The term to search the records for. Default is null.</param>
    /// <param name="sorting">The sorting order of the records. Default is newest.</param>
    /// <param name="currentPage">The current page of the records. Default is 1.</param>
    /// <param name="recordsPerPage">The number of records per page. Default is 1.</param>
    /// <returns>A records query model.</returns>
    public async Task<RecordsQueryModel> Search(string userId, RecordCategories category = RecordCategories.All, 
                                               string? searchTerm = null, RecordsSorting sorting = RecordsSorting.Newest, 
                                               int currentPage = 1, int recordsPerPage = 1)
    {

        switch (category)
        {
            case RecordCategories.TaxRecords:
                {
                    var taxRecordsResult = await RetrieveAndSortUserTaxRecords(userId, searchTerm, sorting, currentPage, recordsPerPage);

                    result.Records = CastRecordsToObject(taxRecordsResult);
                    result.TotalRecordsCount = await GetTotalUserTaxRecordsCount(userId);

                    break;
                }
            case RecordCategories.TripRecords:
                {
                    var tripRecordsResult = await RetrieveAndSortUserTripRecords(userId, searchTerm, sorting, currentPage, recordsPerPage);

                    result.Records = CastRecordsToObject(tripRecordsResult);
                    result.TotalRecordsCount = await GetTotalUserTripRecordsCount(userId);

                    break;
                }
            case RecordCategories.ServiceRecords:
                {
                    var serviceRecordsResult = await RetrieveAndSortUserServiceRecords(userId, searchTerm, sorting, currentPage, recordsPerPage);

                    result.Records = CastRecordsToObject(serviceRecordsResult);
                    result.TotalRecordsCount = await GetTotalUserServiceRecordsCount(userId);

                    break;
                }
            case RecordCategories.All:
                {
                    var taxRecordsResult = await RetrieveAndSortUserTaxRecords(userId, searchTerm, sorting, currentPage, recordsPerPage);
                    var tripRecordsResult = await RetrieveAndSortUserTripRecords(userId, searchTerm, sorting, currentPage, recordsPerPage);
                    var serviceRecordsResult = await RetrieveAndSortUserServiceRecords(userId, searchTerm, sorting, currentPage, recordsPerPage);

                    int recordsPerType = recordsPerPage / 3; 

                    int additionalRecordsNeeded = recordsPerPage - (taxRecordsResult.Count + tripRecordsResult.Count + serviceRecordsResult.Count);

                    if (additionalRecordsNeeded > 0)
                    {
                        var additionalTaxRecords = await taxRecordsService.RetrieveAdditionalTaxRecordsByPageAsync(taxRecords, currentPage, recordsPerType, additionalRecordsNeeded);
                        taxRecordsResult.AddRange(additionalTaxRecords);

                        var additionalTripRecords = await tripRecordsService.RetrieveAdditionalTripRecordsByPageAsync(tripRecords, currentPage, recordsPerType, additionalRecordsNeeded);
                        tripRecordsResult.AddRange(additionalTripRecords);

                        var additionalServiceRecords = await serviceRecordsService.RetrieveAdditionalServiceRecordsByPageAsync(serviceRecords, currentPage, recordsPerType, additionalRecordsNeeded);
                        serviceRecordsResult.AddRange(additionalServiceRecords);
                    }

                    var allRecords = taxRecordsResult.Cast<object>()
                                     .Concat(tripRecordsResult.Cast<object>())
                                     .Concat(serviceRecordsResult.Cast<object>())
                                     .Take(recordsPerPage)
                                     .ToList();

                    var sortedRecords = sortService.AdditionalSortOfAllResults<object>(allRecords, sorting);

                    result.Records = sortedRecords;

                    result.TotalRecordsCount = await RetrieveAllUserRecordsCount(userId);

                    break;
                }
            default: 
                {
                    //TODO Implement exception
                    throw new InvalidProgramException();
                }

        }

        return result;
    }

  
    private async Task<List<TaxRecordDetailsQueryResponseModel>> RetrieveAndSortUserTaxRecords(string userId, string searchTerm, RecordsSorting sorting, int currentPage, int recordsPerPage)
    {
        await GetUserTaxRecords(userId);

        if (string.IsNullOrEmpty(searchTerm) == false)
        {
            FilterTaxRecords(searchTerm);
        }

        taxRecords = sortService.SortCostableRecords(taxRecords, sorting);

        return await taxRecordsService.RetrieveTaxRecordsByPageAsync(taxRecords, currentPage, recordsPerPage);

    }
    private async Task GetUserTaxRecords(string userId)
    {
        taxRecords = await taxRecordsService.GetAllByUserIdAsQueryableAsync(userId);
    }

    private void FilterTaxRecords(string searchTerm)
    {
        taxRecords = taxRecordsService.FilterRecordsBySearchTerm(taxRecords, searchTerm);
    }

    private async Task<int> GetTotalUserTaxRecordsCount(string userId)
    {
        return await taxRecordsService.GetAllUserTaxRecordsCountAsync(userId);
    }

    private async Task<List<TripDetailsByUserQueryResponseModel>> RetrieveAndSortUserTripRecords(string userId, string searchTerm, RecordsSorting sorting, int currentPage, int recordsPerPage)
    {
        await GetUserTripRecords(userId);

        if (string.IsNullOrEmpty(searchTerm) == false)
        {
            FilterTripRecords(searchTerm);
        }

        tripRecords = sortService.SortOptionalCostableRecords(tripRecords, sorting);

        return await tripRecordsService.RetrieveTripRecordsByPageAsync(tripRecords, currentPage, recordsPerPage);

    }

    private async Task GetUserTripRecords(string userId)
    {
        tripRecords = await tripRecordsService.GetAllByUserIdAsQueryableAsync(userId);
    }

    private void FilterTripRecords(string searchTerm)
    {
        tripRecords = tripRecordsService.FilterRecordsBySearchTerm(tripRecords, searchTerm);
    }
    
    private async Task<int> GetTotalUserTripRecordsCount(string userId)
    {
        return await tripRecordsService.GetAllUserTripsCountAsync(userId);
    }

    private async Task<List<ServiceRecordDetailsQueryResponseModel>> RetrieveAndSortUserServiceRecords(string userId, string searchTerm, RecordsSorting sorting, int currentPage, int recordsPerPage)
    {
        await GetUserServiceRecords(userId);

        if (string.IsNullOrEmpty(searchTerm) == false)
        {
            FilterServiceRecords(searchTerm);
        }

        serviceRecords = sortService.SortCostableRecords(serviceRecords, sorting);

        return await serviceRecordsService.RetrieveServiceRecordsByPageAsync(serviceRecords, currentPage, recordsPerPage);
    }

    private async Task GetUserServiceRecords(string userId)
    {
        serviceRecords = await serviceRecordsService.GetAllByUserIdAsQueryableAsync(userId);
    }

    private void FilterServiceRecords(string searchTerm)
    {
        serviceRecords = serviceRecordsService.FilterRecordsBySearchTerm(serviceRecords, searchTerm);
    }


    private async Task<int> GetTotalUserServiceRecordsCount(string userId)
    {
        return await serviceRecordsService.GetAllUserServiceRecordsCountAsync(userId);
    }

    private IList<object> CastRecordsToObject<T>(IEnumerable<T> records)
    {
        return records.Cast<object>().ToList();
    }

    private async Task<int> RetrieveAllUserRecordsCount(string userId)
    {
        var taxRecordsCount = await GetTotalUserTaxRecordsCount(userId);
        var serviceRecordsCount = await GetTotalUserServiceRecordsCount(userId);
        var tripRecordsCount = await GetTotalUserTripRecordsCount(userId);

        return taxRecordsCount + tripRecordsCount + serviceRecordsCount;
    }


}
