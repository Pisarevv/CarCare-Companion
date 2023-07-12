namespace CarCare_Companion.Infrastructure.Data.Models.Vehicle;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

using CarCare_Companion.Infrastructure.Data.Models.BaseModels;
using CarCare_Companion.Infrastructure.Data.Models.Identity;
using CarCare_Companion.Infrastructure.Data.Models.Records;

using static CarCare_Companion.Common.ValidationConstants.Vehicle;

public class Vehicle : BaseDeletableModel<Vehicle>
{
    public Vehicle()
    {    
        this.ServiceRecords = new HashSet<ServiceRecord>();
        this.TripRecords = new HashSet<TripRecord>();
        this.TaxRecords = new HashSet<TaxRecord>();
    }

    [Comment("Make name")]
    [Required]
    [MinLength(MinMakeNameLength)]
    [MaxLength(MaxMakeNameLength)]
    public string Make { get; set; } = null!;

    [Comment("Model name")]
    [Required]
    [MinLength(MinModelNameLength)]
    [MaxLength(MaxModelNameLength)]
    public string Model { get; set; } = null!;

    [Comment("Mileage of the vehicle")]
    [Required]
    [Range(MinMileage,MaxMileage)]
    public double Mileage { get; set; }

    [Comment("Production year of the vehicle")]
    [Required]
    [Range(MinProductionYear,MaxProductionYear)]
    public int Year { get; set; }

    [Comment("Used fuel type identifier")]
    [Required]
    [ForeignKey(nameof(FuelType))]
    public int FuelTypeId { get; set; }

    [Required]
    public FuelType FuelType { get; set; } = null!;

    [Comment("Vehicle owner identifier")]
    [Required]
    [ForeignKey(nameof(Owner))]
    public Guid OwnerId { get; set; }

    [Required]
    public ApplicationUser Owner { get; set; } = null!;

    [Comment("Vehicle type identifier")]
    [Required]
    [ForeignKey(nameof(VehicleType))]
    public int VehicleTypeId { get; set; }

    [Comment("Type of vehicle")]
    [Required]
    public VehicleType VehicleType { get; set; }

    [Comment("Image key for vehicle picture")]
    public Guid? VehicleImageKey { get; set; }

    [Comment("Vehicle service records")]
    public ICollection<ServiceRecord> ServiceRecords { get; set; }

    [Comment("Vehicle trip records")]
    public ICollection<TripRecord> TripRecords { get; set; }

    [Comment("Vehicle tax records")]
    public ICollection<TaxRecord> TaxRecords { get; set; }

}
