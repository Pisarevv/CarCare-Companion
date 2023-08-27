namespace CarCare_Companion.Tests.Integration_Tests;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using CarCare_Companion.Infrastructure.Data;


public class TestWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        builder.ConfigureServices((services) =>
        {
            var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.Test.json")
            .Build();

            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<CarCareCompanionDbContext>));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<CarCareCompanionDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("TestDatabaseConnection"));
            });

            var sp = services.BuildServiceProvider();

            //Create database if it doesn't exist
            using (var scope = sp.CreateScope())
            {

                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<CarCareCompanionDbContext>();
                db.Database.EnsureCreated();

            }
        });
    }


}
