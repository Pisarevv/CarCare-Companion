namespace CarCare_Companion.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

using CarCare_Companion.Common;
using CarCare_Companion.Core.Contracts;
using CarCare_Companion.Core.Models.Search;

/// <summary>
/// The search controller handles records searching related operations
/// </summary>
public class SearchController : BaseController
{
    private readonly ISearchService searchService;
    private readonly ILogger<SearchController> logger;

    public SearchController(ISearchService searchService,ILogger<SearchController> logger)
    {
        this.searchService = searchService;
        this.logger = logger;
    }

    /// <summary>
    /// Retrieves user records based on the filtering, sorting and searching criteria.
    /// </summary>
    /// <param name="query">The input query containing the filtering, sorting and searching criteria.</param>
    /// <returns>A collection of records</returns>
    [HttpPost]
    [Route("/Search")]
    public async Task<IActionResult> Search(AllRecordsQueryModel query)
    {
        try
        {
            string? userId = this.User.GetId();

            if (string.IsNullOrEmpty(userId))
            {
                return StatusCode(403, new ProblemDetails
                {
                    Title = StatusResponses.InvalidUser
                });
            }

            var result = await searchService
            .Search
            (userId,
             query.Category,
             query.SearchTerm,
             query.Sorting,
             query.CurrentPage,
             AllRecordsQueryModel.RecordsPerPage
             );

            query.TaxRecords = result.TaxRecords;
            query.TripRecords = result.TripRecords;
            query.ServiceRecords = result.ServiceRecords;
            query.TotalRecords = result.TotalRecordsCount;

            return StatusCode(200, query);
        }

        catch (SqlException ex)
        {
            logger.LogWarning(ex.Message);
            return StatusCode(400, new ProblemDetails
            {
                Title = StatusResponses.GenericError
            });
        }
        catch (InvalidProgramException ex)
        {
            logger.LogError(ex.Message);
            return StatusCode(400, new ProblemDetails
            {
                Title = StatusResponses.GenericError
            });
        }
        catch (Exception ex)
        {
            logger.LogInformation(ex.Message);
            return StatusCode(400, new ProblemDetails
            {
                Title = StatusResponses.BadRequest
            });
        }
    }
}
