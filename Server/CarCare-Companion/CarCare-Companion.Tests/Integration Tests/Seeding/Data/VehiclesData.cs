namespace CarCare_Companion.Tests.Integration_Tests.SeedData;

using System.Collections.Generic;

using CarCare_Companion.Infrastructure.Data.Models.Vehicle;

using static Integration_Tests.Common.TestDataConstants;

public static class VehiclesData
{
    public static ICollection<Vehicle> Vehicles = new HashSet<Vehicle>()
    {
        new Vehicle()
        {
            Id = Guid.Parse(User1VehicleId),
            Make = "BMW",
            Model = "M5",
            Mileage = 10000,
            FuelTypeId = 1,
            VehicleTypeId = 1,
            Year = 2000,
            OwnerId = Guid.Parse(User1Id),
            CreatedOn = DateTime.UtcNow
        },
        new Vehicle()
        {
            Id = Guid.Parse(User2VehicleId),
            Make = "BMW",
            Model = "M3",
            Mileage = 40000,
            FuelTypeId = 1,
            VehicleTypeId = 1,
            Year = 2005,
            OwnerId = Guid.Parse(User2Id),
            CreatedOn = DateTime.UtcNow
        },
        new Vehicle()
        {
            Id = Guid.Parse(User3VehicleId),
            Make = "BMW",
            Model = "M1",
            Mileage = 30000,
            FuelTypeId = 1,
            VehicleTypeId = 1,
            Year = 2013,
            OwnerId = Guid.Parse(User3Id),
            CreatedOn = DateTime.UtcNow
        },
        new Vehicle()
        {
            Id = Guid.Parse(User4VehicleId),
            Make = "BMW",
            Model = "M4",
            Mileage = 60000,
            FuelTypeId = 1,
            VehicleTypeId = 1,
            Year = 2022,
            OwnerId = Guid.Parse(User4Id),
            CreatedOn = DateTime.UtcNow
        },
    };
}
