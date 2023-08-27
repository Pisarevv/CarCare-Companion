namespace CarCare_Companion.Tests.Integration_Tests.Seeding.Seeders;

using CarCare_Companion.Infrastructure.Data;
using CarCare_Companion.Infrastructure.Data.Common;
using CarCare_Companion.Infrastructure.Data.Seeding;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

using static Integration_Tests.Seeding.Data.ServiceRecordsData;

public class TestServiceRecordsSeeder : ISeeder
{
    public async Task SeedAsync(CarCareCompanionDbContext dbContext, IServiceProvider serviceProvider)
    {
        var repository = serviceProvider.GetRequiredService<IRepository>();
        await repository.AddRangeAsync(ServiceRecords);
    }
}
