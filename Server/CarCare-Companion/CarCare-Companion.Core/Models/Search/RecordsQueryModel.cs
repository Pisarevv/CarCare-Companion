namespace CarCare_Companion.Core.Models.Search;

public class RecordsQueryModel
{

    public RecordsQueryModel()
    {
        this.Records = new List<object>();
    }

    public int TotalRecordsCount { get; set; }

    public ICollection<object> Records { get; set; }
}
