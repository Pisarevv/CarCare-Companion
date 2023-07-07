namespace CarCare_Companion.Infrastructure.Data.Models.Records;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

using Microsoft.EntityFrameworkCore;

using CarCare_Companion.Infrastructure.Data.Models.BaseModels;

using Models.Vehicle;

using static CarCare_Companion.Common.ValidationConstants.TaxRecord;
using CarCare_Companion.Infrastructure.Data.Models.Identity;

public class TaxRecord : BaseDeletableModel<TaxRecord>
{
    [Comment("The title of the record")]
    [Required]
    [MinLength(MinTitleLength)]
    [MaxLength(MaxTitleLength)]
    public string Title { get; set; } = null!;

    [Comment("Date of tax validity")]
    public DateTime ValidFrom { get; set; }

    [Comment("Date of tax validity end")]
    public DateTime ValidTo { get; set; }

    [Comment("Description of the tax")]
    public string? Description { get; set; }

    [Comment("Cost of the tax")]
    [Range(MinCost,MaxCost)]
    [Precision(18, 2)]
    public decimal Cost { get; set; }

    [Comment("The vehicle identifier")]
    [ForeignKey(nameof(Vehicle))]
    [Required]
    public Guid VehicleId { get; set; }

    [Comment("The vehicle that the tax is payed")]
    [Required]
    public Vehicle Vehicle { get; set; } = null!;

    [Comment("Vehicle owner identifier")]
    [Required]
    [ForeignKey(nameof(Owner))]
    public Guid OwnerId { get; set; }

    [Required]
    public ApplicationUser Owner { get; set; } = null!;
}
