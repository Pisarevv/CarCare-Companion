namespace CarCare_Companion.Infrastructure.Data.Seeding;

public interface ISeeder
{
    Task SeedAsync(CarCareCompanionDbContext dbContext, IServiceProvider serviceProvider);
}
