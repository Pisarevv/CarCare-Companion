namespace CarCare_Companion.Tests.Integration_Tests;

using CarCare_Companion.Api;
using CarCare_Companion.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage;

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[TestFixture]
public class ServiceRecordsControllerTests
{
    private TestWebApplicationFactory<Program> factory;
    private HttpClient client;
    private CarCareCompanionDbContext dbContext;
    private IDbContextTransaction transaction;
    private IServiceProvider serviceProvider;


    [OneTimeSetUp]
    public async Task SetupDatabase()
    {
        factory = new TestWebApplicationFactory<Program>();
        client = factory.CreateClient();

        using (var scope = factory.Services.CreateScope())
        {
            serviceProvider = scope.ServiceProvider;
            dbContext = serviceProvider.GetRequiredService<CarCareCompanionDbContext>();

            await dbContext.Database.EnsureDeletedAsync();
            await dbContext.Database.EnsureCreatedAsync();

            // Seed the database
            //await TestDataSeeder.SeedTestData(dbContext);
        }


    }

    [SetUp]
    public async Task Setup()
    {
        factory = new TestWebApplicationFactory<Program>();
        client = factory.CreateClient();

        using (var scope = factory.Services.CreateScope())
        {
            serviceProvider = scope.ServiceProvider;
            dbContext = serviceProvider.GetRequiredService<CarCareCompanionDbContext>();
            // Begin transaction
            transaction = dbContext.Database.BeginTransaction();
        }
    }


    [TearDown]
    public void TearDown()
    {
        using (var scope = factory.Services.CreateScope())
        {
            // Begin transaction
            transaction.Rollback();
            transaction.Dispose();
        }

        client.Dispose();
        factory.Dispose();
    }
}
