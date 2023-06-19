namespace CarCare_Companion.Infrastructure.Data.Models.Vehicle;

using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

using static CarCare_Companion.Common.ValidationConstants.FuelType;

public class FuelType
{
    public FuelType()
    {
       this.Vehicles = new HashSet<Vehicle>(); 
    }

    [Comment("Fuel id")]
    [Key]
    public int Id { get; set; }

    [Comment("Name of the fuel")]
    [Required]
    [MinLength(MinNameLength)]
    [MaxLength(MaxNameLength)]
    public string Name { get; set; } = null!;

    [Comment("Collection of vehicles")]
    public ICollection<Vehicle> Vehicles { get; set; }
}
