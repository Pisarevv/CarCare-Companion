namespace CarCare_Companion.Core.Models.Search;

using CarCare_Companion.Core.Enums;

public class AllRecordsQueryModel
{
    public AllRecordsQueryModel()
    {
        this.Records = new List<object>();
    }
    public const int RecordsPerPage = 9;

    public int CurrentPage { get; set; } = 1;

    public RecordCategories Category { get; set; }
    
    public string? SearchTerm { get; set; }

    public RecordsSorting Sorting { get; set; }

    public int TotalRecords { get; set; }

    public ICollection<object> Records { get; set; }
}
