namespace CarCare_Companion.Core.Contracts;

using CarCare_Companion.Core.Enums;
using CarCare_Companion.Core.Models.Search;

public interface ISearchService 
{
    public RecordsQueryModel Search(string userId, string? category = null, string? searchTerm = null, RecordsSorting sorting = RecordsSorting.Newest, int currentPage = 1, int recordsPerPage = 1);
}
