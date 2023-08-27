namespace CarCare_Companion.Tests.Integration_Tests.Seeding.Data;

using System;
using System.Collections.Generic;

using CarCare_Companion.Infrastructure.Data.Models.Records;

using static Integration_Tests.Common.TestDataConstants;

public static class TripRecordsData
{
    public static ICollection<TripRecord> TripRecords = new HashSet<TripRecord>()
    {
        new TripRecord
            {
                StartDestination = "Los Angeles, CA",
                EndDestination = "San Francisco, CA",
                MileageTravelled = 380,
                UsedFuel = 15,
                FuelPrice = 4.00M,
                CreatedOn = new DateTime(2023, 8, 1, 9, 0, 0, DateTimeKind.Utc),
                VehicleId = Guid.Parse(User1VehicleId),
                OwnerId = Guid.Parse(User1Id),
                Cost = 100m
            },
            new TripRecord
            {
                StartDestination = "New York, NY",
                EndDestination = "Washington, DC",
                MileageTravelled = 225,
                UsedFuel = 10,
                FuelPrice = 3.50M,
                CreatedOn = new DateTime(2023, 8, 15, 10, 0, 0, DateTimeKind.Utc),
                VehicleId = Guid.Parse(User2VehicleId),
                OwnerId = Guid.Parse(User2Id),
                Cost = 200m
            },
            new TripRecord
            {
                StartDestination = "Chicago, IL",
                EndDestination = "Detroit, MI",
                MileageTravelled = 280,
                UsedFuel = 12,
                FuelPrice = 3.75M,
                CreatedOn = new DateTime(2023, 9, 1, 11, 0, 0, DateTimeKind.Utc),
                VehicleId = Guid.Parse(User3VehicleId),
                OwnerId = Guid.Parse(User3Id),
                Cost = 300m
            },
            new TripRecord
            {
                StartDestination = "Dallas, TX",
                EndDestination = "Houston, TX",
                MileageTravelled = 240,
                UsedFuel = 11,
                FuelPrice = 3.25M,
                CreatedOn = new DateTime(2023, 9, 15, 12, 0, 0, DateTimeKind.Utc),
                VehicleId = Guid.Parse(User4VehicleId),
                OwnerId = Guid.Parse(User4Id),
                Cost = 400m
            }
    };
}
