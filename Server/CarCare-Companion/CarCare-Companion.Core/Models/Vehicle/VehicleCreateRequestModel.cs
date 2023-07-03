namespace CarCare_Companion.Core.Models.Vehicle;

using System.ComponentModel.DataAnnotations;

using static CarCare_Companion.Common.ValidationConstants.Vehicle;

public class VehicleCreateRequestModel
{
    [Required]
    [MinLength(MinMakeNameLength)]
    [MaxLength(MaxMakeNameLength)]
    public string Make { get; set; } = null!;

    [Required]
    [MinLength(MinModelNameLength)]
    [MaxLength(MaxModelNameLength)]
    public string Model { get; set; } = null!;

    [Required]
    [Range(MinMileage, MaxMileage)]
    public double Mileage { get; set; }

    [Required]
    [Range(MinProductionYear, MaxProductionYear)]
    public int Year { get; set; }

    [Required]
    public int FuelTypeId { get; set; }

    [Required]
    public Guid OwnerId { get; set; }

    [Required]
    public int VehicleTypeId { get; set; }

}
