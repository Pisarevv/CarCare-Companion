﻿namespace CarCare_Companion.Infrastructure.Data.Models.Vehicle;

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

    [Comment("Vehicle service records")]
    public ICollection<ServiceRecord> ServiceRecords { get; set; }

    [Comment("Vehicle trip records")]
    public ICollection<TripRecord> TripRecords { get; set; }
    
}
