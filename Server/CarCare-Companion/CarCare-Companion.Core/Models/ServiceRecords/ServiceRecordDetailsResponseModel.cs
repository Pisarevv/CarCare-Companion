namespace CarCare_Companion.Core.Models.ServiceRecords;

public class ServiceRecordDetailsResponseModel
{
    public string Id { get; set; } = null!;

    public string Title { get; set; } = null!;

    public DateTime PerformedOn { get; set; }

    public double Mileage { get; set; }

    public string? Description { get; set; }

    public decimal Cost { get; set; }

    public string VehicleMake { get; set; } = null!;

    public string VehicleModel { get; set; } = null!;

}
