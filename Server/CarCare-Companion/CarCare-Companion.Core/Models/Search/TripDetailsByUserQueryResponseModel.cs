using CarCare_Companion.Infrastructure.Data.Models.Contracts;

namespace CarCare_Companion.Core.Models.Search;

public class TripDetailsByUserQueryResponseModel
{
    public string Type { get; } = "TripRecord";

    public string Id { get; set; } = null!;

    public string StartDestination { get; set; } = null!;

    public string EndDestination { get; set; } = null!;

    public double MileageTravelled { get; set; }

    public double? UsedFuel { get; set; }

    public decimal? FuelPrice { get; set; }

    public decimal? Cost { get; set; }

    public string VehicleMake { get; set; } = null!;

    public string VehicleModel { get; set; } = null!;

    public DateTime DateCreated { get; set; }
}
