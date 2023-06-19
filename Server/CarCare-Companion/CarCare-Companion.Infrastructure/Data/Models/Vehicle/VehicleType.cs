namespace CarCare_Companion.Infrastructure.Data.Models.Vehicle;

using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

using static CarCare_Companion.Common.ValidationConstants.VehicleType;

public class VehicleType
{
    public VehicleType()
    {
        this.Vehicles = new HashSet<Vehicle>();
    }

    [Comment("Type id")]
    [Key]
    public int Id { get; set; }

    [Comment("Name of the type")]
    [Required]
    [MinLength(MinNameLength)]
    [MaxLength(MaxNameLength)]
    public string Name { get; set; } = null!;

    [Comment("Collection of vehicles")]
    public ICollection<Vehicle> Vehicles { get; set; }
}

