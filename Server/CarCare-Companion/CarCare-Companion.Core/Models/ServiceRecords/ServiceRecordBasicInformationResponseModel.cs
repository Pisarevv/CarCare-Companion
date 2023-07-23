namespace CarCare_Companion.Core.Models.ServiceRecords;

public class ServiceRecordBasicInformationResponseModel
{
    public string Id { get; set; } = null!;

    public string Title { get; set; } = null!;

    public DateTime PerformedOn { get; set; }

    public string VehicleMake { get; set; } = null!;

    public string VehicleModel { get; set; } = null!;
}
