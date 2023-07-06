namespace CarCare_Companion.Infrastructure.Data.Models.Records;

using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;


using CarCare_Companion.Infrastructure.Data.Models.BaseModels;
using CarCare_Companion.Infrastructure.Data.Models.Vehicle;

using static CarCare_Companion.Common.ValidationConstants.TripRecord;


public class TripRecord : BaseDeletableModel<TripRecord>
{
    [Comment("The start destination of the trip")]
    [Required]
    [MinLength(MinStartDestinationNameLength)]
    [MaxLength(MaxStartDestinationNameLength)]
    public string StartDestination { get; set; } = null!;

    [Comment("The end destination of the trip")]
    [Required]
    [MinLength(MinEndDestinationNameLength)]
    [MaxLength(MaxEndDestinationNameLength)]
    public string EndDestination { get; set; } = null!;

    [Comment("The travelled destination on the trip")]
    [Required]
    [Range(MinTravelledRange,MaxTravelledRange)]
    public double MileageTravelled { get; set; }

    [Comment("The used destination on the trip")]
    [Range(MinUsedFuel, MaxUsedFuel)]
    public double? UsedFuel { get; set; }


    [Comment("The price of the fuel during the trip")]
    [Range(MinFuelPrice,MaxFuelPrice)]
    [Precision(18,2)]
    public decimal? FuelPrice { get; set; }

    [Comment("The used vehicle on the trip")]
    [Required]
    public Vehicle Vehicle { get; set; } = null!;

    [Comment("The vehicle identifier")]
    [Required]
    public Guid VehicleId { get; set; }
}
