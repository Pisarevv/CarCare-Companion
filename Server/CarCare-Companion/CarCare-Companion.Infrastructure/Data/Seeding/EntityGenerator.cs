using CarCare_Companion.Infrastructure.Data.Models.Vehicle;

namespace CarCare_Companion.Infrastructure.Data.Seeding;

public static class EntityGenerator 
{
    public static ICollection<FuelType> GenerateFuelTypes()
    {
        return new HashSet<FuelType>()
        {
            new FuelType()
            {
                Id = 1,
                Name = "Petrol"
            },
             new FuelType()
            {
                Id = 2,
                Name = "Diesel"
            },
              new FuelType()
            {
                Id = 3,
                Name = "Electric"
            },
              new FuelType()
            {
                Id = 4,
                Name = "Hybrid"
            },
              new FuelType()
            {
                Id = 5,
                Name = "Petrol/LPG"
            }

        };
    }

    public static ICollection<VehicleType> GenerateVehicleTypes()
    {
        return new HashSet<VehicleType>()
        {
            new VehicleType
            { 
                Id = 1, 
                Name = "Sedan"
            },
            new VehicleType
            {
                Id = 2, 
                Name = "Hatchback"
            },
            new VehicleType 
            { 
                Id = 3,
                Name = "SUV"
            },
            new VehicleType 
            {
                Id = 4, 
                Name = "Crossover" 
            },
            new VehicleType 
            { 
                Id = 5, 
                Name = "Coupe"
            },
            new VehicleType 
            { 
                Id = 6, 
                Name = "Convertible" 
            },
            new VehicleType 
            { 
                Id = 7,
                Name = "Minivan"
            },
            new VehicleType 
            { 
                Id = 8,
                Name = "Pickup Truck"
            },
            new VehicleType 
            { 
                Id = 9,
                Name = "Van" 
            },
            new VehicleType 
            { 
                Id = 10, 
                Name = "Truck"
            },
           
        };
    }
}
