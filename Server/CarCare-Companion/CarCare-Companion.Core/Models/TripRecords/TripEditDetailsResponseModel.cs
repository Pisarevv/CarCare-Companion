namespace CarCare_Companion.Core.Models.Trip;

public class TripEditDetailsResponseModel
{
    public string Id { get; set; } = null!;

    public string StartDestination { get; set; } = null!;

    public string EndDestination { get; set; } = null!;

    public double MileageTravelled { get; set; }

    public double? UsedFuel { get; set; }

    public decimal? FuelPrice { get; set; }

    public string Vehicle{ get; set; } = null!;
}
