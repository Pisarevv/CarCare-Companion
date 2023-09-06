namespace CarCare_Companion.Core.Models.Search;

using CarCare_Companion.Core.Models.ServiceRecords;
using CarCare_Companion.Core.Models.TaxRecords;
using CarCare_Companion.Core.Models.Trip;

public class RecordsQueryModel
{

    public RecordsQueryModel()
    {
        this.TaxRecords = new List<TaxRecordDetailsResponseModel>();
        this.TripRecords = new List<TripDetailsByUserResponseModel>();
        this.ServiceRecords = new List<ServiceRecordDetailsResponseModel>();
    }

    public int TotalRecordsCount { get; set; }

    public ICollection<TaxRecordDetailsResponseModel> TaxRecords { get; set; }

    public ICollection<TripDetailsByUserResponseModel> TripRecords { get; set; }

    public ICollection<ServiceRecordDetailsResponseModel> ServiceRecords { get; set; }
}
