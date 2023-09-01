namespace CarCare_Companion.Tests.Integration_Tests.Seeding.Data;

using System;
using System.Collections.Generic;

using CarCare_Companion.Infrastructure.Data.Models.Records;

using static Integration_Tests.Common.TestDataConstants;

public static class ServiceRecordsData
{
    public static ICollection<ServiceRecord> ServiceRecords = new HashSet<ServiceRecord>()
    {
        new ServiceRecord()
        {
            Id = Guid.Parse("48736248-6983-4f3f-a912-518effece9e1"),
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
            Id = Guid.Parse("188f7b9f-5c91-474d-b8ea-95ac1a03f4c6"),
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
            Id = Guid.Parse("c8730cd6-6543-4745-9977-308462648e9d"),
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
            Id = Guid.Parse("5af2d69f-bad8-490c-8562-2e9abcac7242"),
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
