namespace CarCare_Companion.Core.Models.Search;

using CarCare_Companion.Core.Enums;
using CarCare_Companion.Core.Models.ServiceRecords;
using CarCare_Companion.Core.Models.TaxRecords;
using CarCare_Companion.Core.Models.Trip;

public class AllRecordsQueryModel
{
    public AllRecordsQueryModel()
    {
        this.Categories = new HashSet<string>();
        this.TaxRecords = new List<TaxRecordDetailsResponseModel>();
        this.TripRecords = new List<TripDetailsByUserResponseModel>();
        this.ServiceRecords = new List<ServiceRecordDetailsResponseModel>();
    }
    public const int RecordsPerPage = 10;

    public int CurrentPage { get; set; } = 1;

    public string? Category { get; set; }

    public string? SearchTerm { get; set; }

    public RecordsSorting Sorting { get; set; }

    public int TotalRecords { get; set; }

    public ICollection<string> Categories { get; set; }

    public ICollection<TaxRecordDetailsResponseModel> TaxRecords { get; set; }

    public ICollection<TripDetailsByUserResponseModel> TripRecords { get; set; }

    public ICollection<ServiceRecordDetailsResponseModel> ServiceRecords { get; set; }

}
