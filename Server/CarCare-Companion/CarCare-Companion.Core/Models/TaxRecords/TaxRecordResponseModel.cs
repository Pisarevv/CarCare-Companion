namespace CarCare_Companion.Core.Models.TaxRecords;


public class TaxRecordResponseModel
{
    public string Id { get; set; } = null!;

    public string Title { get; set; } = null!;

    public DateTime ValidFrom { get; set; }

    public DateTime ValidTo { get; set; }

    public decimal Cost { get; set; }

    public string VehicleMake { get; set; } = null!;

    public string VehicleModel { get; set;} = null!;

}
