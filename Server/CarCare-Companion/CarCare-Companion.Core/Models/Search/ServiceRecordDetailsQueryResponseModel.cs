using CarCare_Companion.Infrastructure.Data.Models.Contracts;

namespace CarCare_Companion.Core.Models.Search;

public class ServiceRecordDetailsQueryResponseModel : ICostable
{
    public string Type { get; } = "ServiceRecord";

    public string Id { get; set; } = null!;

    public string Title { get; set; } = null!;

    public DateTime PerformedOn { get; set; }

    public double Mileage { get; set; }

    public string? Description { get; set; }

    public decimal Cost { get; set; }

    public string VehicleMake { get; set; } = null!;

    public string VehicleModel { get; set; } = null!;

    public DateTime DateCreated { get; set; }
}
