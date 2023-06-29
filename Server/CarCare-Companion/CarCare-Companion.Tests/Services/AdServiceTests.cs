namespace CarCare_Companion.Tests.Services;

using System.Collections.Generic;

using Microsoft.EntityFrameworkCore;

using CarCare_Companion.Core.Contracts;
using CarCare_Companion.Infrastructure.Data;
using CarCare_Companion.Infrastructure.Data.Common;
using CarCare_Companion.Infrastructure.Data.Models.Ads;
using CarCare_Companion.Core.Services;

[TestFixture]
public class AdServiceTests
{
    private IRepository repository;
    private IAdService adService;
    private CarCareCompanionDbContext carCareCompanionDbContext;

    [SetUp]
    public void Setup()
    {
        var contextOptions = new DbContextOptionsBuilder<CarCareCompanionDbContext>()
            .UseInMemoryDatabase("CarCareDB")
            .Options;

        carCareCompanionDbContext = new CarCareCompanionDbContext(contextOptions);

        carCareCompanionDbContext.Database.EnsureDeleted();
        carCareCompanionDbContext.Database.EnsureCreated();
    }


    /// <summary>
    /// Tests if the correct entities count is taken from the database
    /// </summary>
    /// <returns></returns>
    [Test]
    public async Task TestAdsGet()
    {
        //Arrange
        int entitiesCount = 5;

        repository = new Repository(carCareCompanionDbContext);
        adService = new AdService(repository);

        IEnumerable<CarouselAdModel> carousleAds = new List<CarouselAdModel>()
        {
            new CarouselAdModel
            {
                Id = Guid.NewGuid(),
                UserFirstName = "David",
                Description = "The car maintenance and service management website is user-friendly, offers a wide service network, effective communication, and robust record-keeping capabilities for a seamless experience.",
                StarsRating = 5,
            },

             new CarouselAdModel
            {
                Id = Guid.NewGuid(),
                UserFirstName = "Peter",
                Description = "With its intuitive interface, extensive service network, detailed record-keeping the car maintenance and service management website ensures a top-notch experience for all users.",
                StarsRating = 5,
            }
        };

        await repository.AddRangeAsync(carousleAds);
        await repository.SaveChangesAsync();

        //Act
        var adEntities = await adService.GetFiveAsync();

        //Assert
        Assert.That(adEntities.Count, Is.EqualTo(entitiesCount));
    }
}
