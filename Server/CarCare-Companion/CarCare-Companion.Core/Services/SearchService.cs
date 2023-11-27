namespace CarCare_Companion.Core.Services;

using Microsoft.EntityFrameworkCore;

using CarCare_Companion.Core.Contracts;
using CarCare_Companion.Core.Enums;
using CarCare_Companion.Core.Models.Search;
using CarCare_Companion.Infrastructure.Data.Models.BaseModels;
using CarCare_Companion.Infrastructure.Data.Models.Contracts;
using CarCare_Companion.Infrastructure.Data.Models.Records;



/// <summary>
/// Service for searching tax, service, and trip records.
/// </summary>
public class SearchService : ISearchService
{
    private readonly ITaxRecordsService taxRecordsService;
    private readonly IServiceRecordsService serviceRecordsService;
    private readonly ITripRecordsService tripRecordsService;

    public SearchService(ITaxRecordsService taxRecordsService, IServiceRecordsService serviceRecordsService, ITripRecordsService tripRecordsService)
    {
        this.taxRecordsService = taxRecordsService;
        this.serviceRecordsService = serviceRecordsService;
        this.tripRecordsService = tripRecordsService;
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
        RecordsQueryModel result = new RecordsQueryModel();

        IQueryable<TripRecord> tripRecords = Enumerable.Empty<TripRecord>().AsQueryable();
        IQueryable<TaxRecord> taxRecords = Enumerable.Empty<TaxRecord>().AsQueryable();
        IQueryable<ServiceRecord> serviceRecords = Enumerable.Empty<ServiceRecord>().AsQueryable();


        switch (category)
        {
            case RecordCategories.TaxRecords:
                {
                    taxRecords = await taxRecordsService.GetAllByUserIdAsQueryableAsync(userId);

                    if (string.IsNullOrEmpty(searchTerm) == false)
                    {
                        taxRecords = taxRecordsService.FilterRecordsBySearchTerm(taxRecords, searchTerm);
                    }
                    taxRecords = SortCostableRecords(taxRecords, sorting);

                    var taxRecordsResult = await taxRecordsService.RetrieveTaxRecordsByPageAsync(taxRecords, currentPage, recordsPerPage);

                    result.Records = taxRecordsResult.Cast<object>().ToList();

                    result.TotalRecordsCount = await taxRecordsService.GetAllUserTaxRecordsCountAsync(userId);
                    break;
                }
            case RecordCategories.ServiceRecords:
                {
                    serviceRecords = await serviceRecordsService.GetAllByUserIdAsQueryableAsync(userId);
                    if (string.IsNullOrEmpty(searchTerm) == false)
                    {
                        serviceRecords = serviceRecordsService.FilterRecordsBySearchTerm(serviceRecords, searchTerm);
                    }
                    serviceRecords = SortCostableRecords(serviceRecords, sorting);

                    var serviceRecordsResult = await serviceRecordsService.RetrieveServiceRecordsByPageAsync(serviceRecords, currentPage, recordsPerPage);

                    result.Records = serviceRecordsResult.Cast<object>().ToList();

                    result.TotalRecordsCount = await serviceRecordsService.GetAllUserServiceRecordsCountAsync(userId);
                    break;
                }
            case RecordCategories.TripRecords:
                {
                    tripRecords = await tripRecordsService.GetAllByUserIdAsQueryableAsync(userId);
                    if (string.IsNullOrEmpty(searchTerm) == false)
                    {
                        tripRecords = tripRecordsService.FilterRecordsBySearchTerm(tripRecords, searchTerm);
                    }
                    tripRecords = SortOptionalCostableRecords(tripRecords, sorting);

                    var tripRecordsResult = await tripRecordsService.RetrieveTripRecordsByPageAsync(tripRecords, currentPage, recordsPerPage);

                    result.Records = tripRecordsResult.Cast<object>().ToList();

                    result.TotalRecordsCount = await tripRecordsService.GetAllUserTripsCountAsync(userId);
                    break;
                }
            case RecordCategories.All:
                {
                    taxRecords = await taxRecordsService.GetAllByUserIdAsQueryableAsync(userId);
                    serviceRecords = await serviceRecordsService.GetAllByUserIdAsQueryableAsync(userId);
                    tripRecords = await tripRecordsService.GetAllByUserIdAsQueryableAsync(userId);

                    if (string.IsNullOrEmpty(searchTerm) == false)
                    {
                        taxRecords = taxRecordsService.FilterRecordsBySearchTerm(taxRecords, searchTerm);
                        serviceRecords = serviceRecordsService.FilterRecordsBySearchTerm(serviceRecords, searchTerm);
                        tripRecords = tripRecordsService.FilterRecordsBySearchTerm(tripRecords, searchTerm);
                    }

                    taxRecords = SortCostableRecords(taxRecords, sorting);
                    serviceRecords = SortCostableRecords(serviceRecords, sorting);
                    tripRecords = SortOptionalCostableRecords(tripRecords, sorting);

                    int recordsPerType = recordsPerPage / 3;
                 
                    var taxRecordsResult = await taxRecordsService.RetrieveTaxRecordsByPageAsync(taxRecords, currentPage, recordsPerPage);
                    var tripRecordsResult = await tripRecordsService.RetrieveTripRecordsByPageAsync(tripRecords, currentPage, recordsPerType);
                    var serviceRecordsResult = await serviceRecordsService.RetrieveServiceRecordsByPageAsync(serviceRecords, currentPage, recordsPerType);

                    int additionalRecordsNeeded = recordsPerPage - (taxRecordsResult.Count + tripRecordsResult.Count + serviceRecordsResult.Count);

                    if(additionalRecordsNeeded > 0)
                    {
                        var additionalTaxRecords = await taxRecordsService.RetrieveAdditionalTaxRecordsByPageAsync(taxRecords,currentPage,recordsPerType, additionalRecordsNeeded);
                        taxRecordsResult.AddRange(additionalTaxRecords);

                        var additionalTripRecords = await tripRecordsService.RetrieveAdditionalTripRecordsByPageAsync(tripRecords, currentPage, recordsPerType, additionalRecordsNeeded);
                        tripRecordsResult.AddRange(additionalTripRecords);

                        var additionalServiceRecords = await serviceRecordsService.RetrieveAdditionalServiceRecordsByPageAsync(serviceRecords,currentPage, recordsPerType, additionalRecordsNeeded);
                        serviceRecordsResult.AddRange(additionalServiceRecords);
                    }

                    var allRecords = taxRecordsResult.Cast<object>()
                                     .Concat(tripRecordsResult.Cast<object>())
                                     .Concat(serviceRecordsResult.Cast<object>())
                                     .Take(recordsPerPage)
                                     .ToList();

                    result.Records = allRecords; 

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

    // Method to retrieve the total count of all user records (tax, service, trip).
    private async Task<int> RetrieveAllUserRecordsCount(string userId)
    {
        var taxRecordsCount = await taxRecordsService.GetAllUserTaxRecordsCountAsync(userId);
        var serviceRecordsCount = await serviceRecordsService.GetAllUserServiceRecordsCountAsync(userId);
        var tripRecordsCount = await tripRecordsService.GetAllUserTripsCountAsync(userId);

        return taxRecordsCount + tripRecordsCount + serviceRecordsCount;
    }

    // Method to sort the costable records based on the sorting option selected by the user.
    private IQueryable<T> SortCostableRecords<T>(IQueryable<T> records, RecordsSorting sorting) where T : BaseDeletableModel<T>, ICostable
    {
        return sorting switch
        {
            RecordsSorting.Newest => records
                                     .OrderByDescending(r => r.CreatedOn),
            RecordsSorting.Oldest => records
                                     .OrderBy(r => r.CreatedOn),
            RecordsSorting.MostExpensive => records
                                            .OrderByDescending(r => r.Cost),
            RecordsSorting.LeastExpensive => records
                                            .OrderBy(r => r.Cost),
            _ => records
                 .OrderByDescending(r => r.Id)
        };
    }

    // Method to sort the optionally costable records based on the sorting option selected by the user.
    private IQueryable<T> SortOptionalCostableRecords<T>(IQueryable<T> records, RecordsSorting sorting) where T : BaseDeletableModel<T>, IOptionalCostable
    {
        return sorting switch
        {
            RecordsSorting.Newest => records
                                     .OrderByDescending(r => r.CreatedOn),
            RecordsSorting.Oldest => records
                                     .OrderBy(r => r.CreatedOn),
            RecordsSorting.MostExpensive => records
                                            .OrderByDescending(r => r.Cost),
            RecordsSorting.LeastExpensive => records
                                            .OrderBy(r => r.Cost),
            _ => records
                 .OrderByDescending(r => r.Id)
        };
    }

}
