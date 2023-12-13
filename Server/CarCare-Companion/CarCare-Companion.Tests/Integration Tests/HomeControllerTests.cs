namespace CarCare_Companion.Tests.Integration_Tests;

using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json;

using CarCare_Companion.Api;
using CarCare_Companion.Infrastructure.Data;
using CarCare_Companion.Infrastructure.Data.Models.Ads;

using static Infrastructure.Data.Seeding.EntityGenerator;

[TestFixture]
[Category("Integration")]
public class HomeControllerTests
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

    [Test]
    public async Task GetEndpoint_ReturnsSuccessAndCorrectContentType()
    {
        //Arrange
        ICollection<CarouselAdModel> carouselAds = GenerateCarouselAdModels();
        //Act
        var response = await client.GetAsync("/Home");

        var data = await response.Content.ReadAsStringAsync();
        ICollection<CarouselAdModel> resposeData = JsonConvert.DeserializeObject<ICollection<CarouselAdModel>>(data);

        //Assert
        Assert.That(response.IsSuccessStatusCode, Is.True);
        Assert.That(response.Content.Headers.ContentType.ToString(), Is.EqualTo("application/json; charset=utf-8"));
        Assert.IsNotNull(data);  
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
