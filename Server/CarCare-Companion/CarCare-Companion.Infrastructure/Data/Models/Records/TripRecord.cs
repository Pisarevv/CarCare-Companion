namespace CarCare_Companion.Infrastructure.Data.Models.Records;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using CarCare_Companion.Infrastructure.Data.Models.BaseModels;
using CarCare_Companion.Infrastructure.Data.Models.Vehicle;
using CarCare_Companion.Infrastructure.Data.Models.Identity;
using CarCare_Companion.Infrastructure.Data.Models.Contracts;

using static CarCare_Companion.Common.ValidationConstants.TripRecord;

public class TripRecord : BaseDeletableModel<TripRecord>, IOptionalCostable
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


    [Comment("The price of the fuel during the trip in levs")]
    [Range(MinFuelPrice,MaxFuelPrice)]
    [Precision(18,2)]
    public decimal? FuelPrice { get; set; }

    [Comment("The cost of the trip")]
    [Precision(18, 2)]
    public decimal? Cost { get; set; }

    [Comment("The used vehicle on the trip")]
    [Required]
    public Vehicle Vehicle { get; set; } = null!;

    [Comment("The vehicle identifier")]
    [Required]
    public Guid VehicleId { get; set; }

    [Comment("Vehicle owner identifier")]
    [Required]
    [ForeignKey(nameof(Owner))]
    public Guid OwnerId { get; set; }

    [Required]
    public ApplicationUser Owner { get; set; } = null!;


}
