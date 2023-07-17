namespace CarCare_Companion.Core.Models.Trip;

using System.ComponentModel.DataAnnotations;

using static CarCare_Companion.Common.ValidationConstants.TripRecord;

public class TripFormRequestModel
{
    [Required]
    [MinLength(MinStartDestinationNameLength)]
    [MaxLength(MaxStartDestinationNameLength)]
    public string StartDestination { get; set; } = null!;

    [Required]
    [MinLength(MinEndDestinationNameLength)]
    [MaxLength(MaxEndDestinationNameLength)]
    public string EndDestination { get; set; } = null!;

    [Required]
    [Range(MinTravelledRange, MaxTravelledRange)]
    public double MileageTravelled { get; set; }

    [Range(MinFuelPrice, MaxFuelPrice)]
    public double? UsedFuel { get; set; }

    [Range(MinUsedFuel,MaxUsedFuel)]
    public decimal? FuelPrice { get; set; }

    public string VehicleId { get; set; } = null!;

}
