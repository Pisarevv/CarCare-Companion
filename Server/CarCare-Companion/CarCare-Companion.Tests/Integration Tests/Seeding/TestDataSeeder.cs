namespace CarCare_Companion.Tests.Integration_Tests.Seeding;

using CarCare_Companion.Infrastructure.Data;
using CarCare_Companion.Infrastructure.Data.Seeding;
using CarCare_Companion.Tests.Integration_Tests.Seeding.Seeders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;


public class TestDataSeeder : ISeeder
{
    public async Task SeedAsync(CarCareCompanionDbContext dbContext, IServiceProvider serviceProvider)
    {
        if (dbContext == null)
        {
            throw new ArgumentNullException(nameof(dbContext));
        }

        if (serviceProvider == null)
        {
            throw new ArgumentNullException(nameof(serviceProvider));
        }

        var logger = serviceProvider.GetService<ILoggerFactory>().CreateLogger(typeof(TestDataSeeder));

        var seeders = new List<ISeeder>
                          {
                              new TestApplicationUsersSeeder(),
                              new TestVehicleSeeder(),
                              new TestTaxRecordSeeder(),
                              new TestTripRecordSeeder(),
                              new TestServiceRecordsSeeder(),
                          };

        foreach (var seeder in seeders)
        {
            await seeder.SeedAsync(dbContext, serviceProvider);
            await dbContext.SaveChangesAsync();
            logger.LogInformation($"Seeder {seeder.GetType().Name} done.");
        }
    }
}
