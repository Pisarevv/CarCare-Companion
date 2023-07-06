namespace CarCare_Companion.Infrastructure.Data.Models.Records;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

using CarCare_Companion.Infrastructure.Data.Models.BaseModels;
using CarCare_Companion.Infrastructure.Data.Models.Vehicle;

using static CarCare_Companion.Common.ValidationConstants.ServiceRecord;

public class ServiceRecord : BaseDeletableModel<ServiceRecord>
{
    [Comment("The title of the record")]
    [Required]
    [MinLength(MinTitleLength)]
    [MaxLength(MaxTitleLength)]
    public string Title { get; set; } = null!;

    [Comment("Date of the performed service")]
    public DateTime PerformedOn { get; set; }

    [Comment("Vehicle mileage")]
    [Range(MinMileage,MaxMileage)]
    [Required]
    public double Mileage { get; set; }

    [Comment("Description of the service")]
    public string? Description { get; set; }

    [Comment("Cost of the service")]
    [Range(MinCost, MaxCost)]
    [Precision(18, 2)]
    public decimal Cost { get; set; }

    [Comment("The vehicle identifier")]
    [ForeignKey(nameof(Vehicle))]
    [Required]
    public Guid VehicleId { get; set; }

    [Comment("The vehicle that the service is performed")]
    [Required]
    public Vehicle Vehicle { get; set; } = null!;

}
