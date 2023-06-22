using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CarCare_Companion.Infrastructure.Data.Seeding;

public class CarCareCompanionDbContextSeeder : ISeeder
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

        var logger = serviceProvider.GetService<ILoggerFactory>().CreateLogger(typeof(CarCareCompanionDbContextSeeder));

        var seeders = new List<ISeeder>
                          {
                              new RoleSeeder(),
                          };

        foreach (var seeder in seeders)
        {
            await seeder.SeedAsync(dbContext, serviceProvider);
            await dbContext.SaveChangesAsync();
            logger.LogInformation($"Seeder {seeder.GetType().Name} done.");
        }
    }
}
