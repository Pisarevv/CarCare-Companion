namespace CarCare_Companion.Core.Models.TaxRecords;

using System.ComponentModel.DataAnnotations;

using Microsoft.EntityFrameworkCore;


using static Common.ValidationConstants.TaxRecord;

public class TaxRecordFormRequestModel
{

    [Required]
    [MinLength(MinTitleLength)]
    [MaxLength(MaxTitleLength)]
    public string Title { get; set; } = null!;

    [Required]
    public DateTime ValidFrom { get; set; }

    [Required]
    public DateTime ValidTo { get; set; }

    public string? Description { get; set; }

    [Required]
    [Range(MinCost, MaxCost)]
    [Precision(18, 2)]
    public decimal Cost { get; set; }

    [Required]
    public string VehicleId { get; set; } = null!;
}
