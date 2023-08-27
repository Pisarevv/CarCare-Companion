namespace CarCare_Companion.Tests.Integration_Tests.Seeding.Data;

using System.Collections.Generic;

using CarCare_Companion.Infrastructure.Data.Models.Records;

using static Integration_Tests.Common.TestDataConstants;

public static class TaxRecordsData
{
    public static ICollection<TaxRecord> TaxRecords = new HashSet<TaxRecord>()
    {
        new TaxRecord
        {
            Title = "Vehicle Road Tax",
            ValidFrom = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            ValidTo = new DateTime(2023, 12, 31, 23, 59, 59, DateTimeKind.Utc),
            Cost = 200.00M,
            Description = "Annual road tax for vehicle",
            VehicleId = Guid.Parse(User1VehicleId),
            OwnerId = Guid.Parse(User1Id),
            CreatedOn = new DateTime(2023, 1, 1, 9, 0, 0, DateTimeKind.Utc)
        },
        new TaxRecord
        {
            Title = "Emission Tax",
            ValidFrom = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            ValidTo = new DateTime(2023, 12, 31, 23, 59, 59, DateTimeKind.Utc),
            Cost = 100.00M,
            Description = "Annual emission tax for vehicle",
            VehicleId = Guid.Parse(User2VehicleId),
            OwnerId = Guid.Parse(User2Id),
            CreatedOn = new DateTime(2023, 1, 1, 9, 0, 0, DateTimeKind.Utc)
        },
        new TaxRecord
        {
            Title = "Commercial Vehicle Tax",
            ValidFrom = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            ValidTo = new DateTime(2023, 12, 31, 23, 59, 59, DateTimeKind.Utc),
            Cost = 300.00M,
            Description = "Annual tax for commercial vehicles",
            VehicleId = Guid.Parse(User3VehicleId),
            OwnerId = Guid.Parse(User3Id),
            CreatedOn = new DateTime(2023, 1, 1, 9, 0, 0, DateTimeKind.Utc)
        },
        new TaxRecord
        {
            Title = "Commercial Tax",
            ValidFrom = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            ValidTo = new DateTime(2023, 12, 31, 23, 59, 59, DateTimeKind.Utc),
            Cost = 300.00M,
            Description = "Annual tax for vehicles",
            VehicleId = Guid.Parse(User4VehicleId),
            OwnerId = Guid.Parse(User4Id),
            CreatedOn = new DateTime(2023, 1, 1, 9, 0, 0, DateTimeKind.Utc)
        }
    };
}
