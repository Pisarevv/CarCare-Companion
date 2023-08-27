namespace CarCare_Companion.Tests.Integration_Tests.SeedData;

using System;
using System.Collections.Generic;

using CarCare_Companion.Infrastructure.Data.Models.Records;

using static Integration_Tests.Common.TestDataConstants;

public static class ServiceRecordsData
{
    public static ICollection<ServiceRecord> serviceRecords = new HashSet<ServiceRecord>()
    {
        new ServiceRecord()
        {
            Title = "Oil Change",
            Description = "Changed engine oil and oil filter.",
            Cost = 50.00M,
            Mileage = 15000,
            PerformedOn = new DateTime(2023, 8, 1, 10, 0, 0, DateTimeKind.Utc),
            CreatedOn = new DateTime(2023, 8, 1, 9, 0, 0, DateTimeKind.Utc),
            VehicleId = Guid.Parse(User1VehicleId),
            OwnerId = Guid.Parse(User1Id)
        },
        new ServiceRecord
        {
            Title = "Tire Rotation",
            Description = "Rotated all four tires.",
            Cost = 30.00M,
            Mileage = 20000,
            PerformedOn = new DateTime(2023, 8, 15, 11, 0, 0, DateTimeKind.Utc),
            CreatedOn = new DateTime(2023, 8, 15, 10, 0, 0, DateTimeKind.Utc),
            VehicleId = Guid.Parse(User2VehicleId),
            OwnerId = Guid.Parse(User2Id)
        },
        new ServiceRecord
        {
            Title = "Brake Pad Replacement",
            Description = "Replaced front brake pads.",
            Cost = 120.00M,
            Mileage = 25000,
            PerformedOn = new DateTime(2023, 9, 1, 12, 0, 0, DateTimeKind.Utc),
            CreatedOn = new DateTime(2023, 9, 1, 11, 0, 0, DateTimeKind.Utc),
            VehicleId = Guid.Parse(User3VehicleId),
            OwnerId = Guid.Parse(User3Id)
        },
        new ServiceRecord
        {
            Title = "Air Filter Replacement",
            Description = "Replaced air filter.",
            Cost = 20.00M,
            Mileage = 30000,
            PerformedOn = new DateTime(2023, 9, 15, 13, 0, 0, DateTimeKind.Utc),
            CreatedOn = new DateTime(2023, 9, 15, 12, 0, 0, DateTimeKind.Utc),
            VehicleId = Guid.Parse(User4VehicleId),
            OwnerId = Guid.Parse(User4Id)
        }
    };
}
