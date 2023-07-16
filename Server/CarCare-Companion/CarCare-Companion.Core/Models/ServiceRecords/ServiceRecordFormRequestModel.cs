namespace CarCare_Companion.Core.Models.ServiceRecords;

using Microsoft.EntityFrameworkCore;

using System.ComponentModel.DataAnnotations;

using static Common.ValidationConstants.ServiceRecord;

public class ServiceRecordFormRequestModel
{
    [Required]
    [MinLength(MinTitleLength)]
    [MaxLength(MaxTitleLength)]
    public string Title { get; set; } = null!;

    [Required]
    public string PerformedOn { get; set; } = null!;

    [Required]
    [Range(MinMileage, MaxMileage)]
    public double Mileage { get; set; }

    public string? Description { get; set; }

    [Required]
    [Range(MinCost, MaxCost)]
    [Precision(18, 2)]
    public decimal Cost { get; set; }

    [Required]
    public string VehicleId { get; set; } = null!;

}
