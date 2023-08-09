namespace CarCare_Companion.Core.Models.TripRecords;


public class TripResponseModel
{
    public string Id { get; set; } = null!;

    public string StartDestination { get; set; } = null!;

    public string EndDestination { get; set; } = null!;

    public double MileageTravelled { get; set; }

    public double? UsedFuel { get; set; }

    public decimal? FuelPrice { get; set; }
}
