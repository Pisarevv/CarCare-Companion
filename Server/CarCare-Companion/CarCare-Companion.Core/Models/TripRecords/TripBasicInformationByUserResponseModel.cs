namespace CarCare_Companion.Core.Models.Trip;

public class TripBasicInformationByUserResponseModel
{
    public string Id { get; set; } = null!;

    public string StartDestination { get; set; } = null!;

    public string EndDestination { get; set; } = null!;

    public double MileageTravelled { get; set; }

    public string Vehicle { get; set; } = null!;
}
