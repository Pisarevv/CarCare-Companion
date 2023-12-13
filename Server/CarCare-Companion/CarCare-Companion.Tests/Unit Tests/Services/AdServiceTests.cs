namespace CarCare_Companion.Tests.Unit_Tests.Services;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

using Moq;

using CarCare_Companion.Core.Contracts;
using CarCare_Companion.Infrastructure.Data;
using CarCare_Companion.Infrastructure.Data.Models.Ads;
using CarCare_Companion.Core.Services;
using CarCare_Companion.Core.Models.Ads;
using CarCare_Companion.Infrastructure.Data.Common;


[TestFixture]
[Category("Unit")]
public class AdServiceTests
{
    private IRepository repository;
    private IAdService adService;
    private Mock<IMemoryCache> mockMemoryCache;
    private CarCareCompanionDbContext applicationDbContext;


    [SetUp]
    public void Setup()
    {
        var contextOptions = new DbContextOptionsBuilder<CarCareCompanionDbContext>()
            .UseInMemoryDatabase("AdsDB")
            .Options;
        applicationDbContext = new CarCareCompanionDbContext(contextOptions);

        this.mockMemoryCache = new Mock<IMemoryCache>();
        this.repository = new Repository(applicationDbContext);
        this.adService = new AdService(repository, mockMemoryCache.Object);

        //Used for mocking the Set method
        var cacheEntry = Mock.Of<ICacheEntry>();
        mockMemoryCache
       .Setup(x => x.CreateEntry(It.IsAny<Object>()))
       .Returns(cacheEntry);

        applicationDbContext.Database.EnsureDeleted();
        applicationDbContext.Database.EnsureCreated();
    }

    [Test]
    public async Task GetAllAsync_ReturnsDataFromCache_IfPresent()
    {
        //Arrange
        object testData = new List<CarouselAdResponseModel>
        {
            new CarouselAdResponseModel { Id = "1", UserFirstName = "John", Description = "Desc1", StarsRating = 5 },
            new CarouselAdResponseModel { Id = "2", UserFirstName = "John1", Description = "Desc2", StarsRating = 4 }
        };

        mockMemoryCache
       .Setup(x => x.TryGetValue(It.IsAny<object>(), out testData))
       .Returns(true);

        //Act
        var result = await adService.GetAllAsync();

        //Assert
        Assert.AreEqual(testData, result);
    }

    [Test]
    public async Task GetAllAsync_RetrievesFromRepo_WhenCacheIsEmpty()
    {
        int expectedCount = 5;

        //Arrange
        ICollection<CarouselAdModel> repoData = new List<CarouselAdModel>
        {
            new CarouselAdModel { Id = Guid.NewGuid(), UserFirstName = "John", Description = "Desc1", StarsRating = 5 },
            new CarouselAdModel { Id = Guid.NewGuid(), UserFirstName = "John1", Description = "Desc2", StarsRating = 5 },
            new CarouselAdModel { Id = Guid.NewGuid(), UserFirstName = "John2", Description = "Desc3", StarsRating = 5 },
            new CarouselAdModel { Id = Guid.NewGuid(), UserFirstName = "John3", Description = "Desc4", StarsRating = 5 },
            new CarouselAdModel { Id = Guid.NewGuid(), UserFirstName = "John4", Description = "Desc5", StarsRating = 5 },
            new CarouselAdModel { Id = Guid.NewGuid(), UserFirstName = "John5", Description = "Desc6", StarsRating = 5 },
        };

        //Act
        await repository.AddRangeAsync(repoData);
        await repository.SaveChangesAsync();

        ICollection<CarouselAdResponseModel> result = await adService.GetFiveAsync();

        //Assert
        Assert.AreEqual(expectedCount, result.Count());
    }

    [Test]
    public async Task GetFiveAsync_ReturnsDataFromCache_IfPresent()
    {
        //Arrange

        object testData = new List<CarouselAdResponseModel>
        {
            new CarouselAdResponseModel { Id = "1", UserFirstName = "John", Description = "Desc1", StarsRating = 5 },
            new CarouselAdResponseModel { Id = "2", UserFirstName = "John1", Description = "Desc2", StarsRating = 5 },
            new CarouselAdResponseModel { Id = "3", UserFirstName = "John2", Description = "Desc3", StarsRating = 5 },
            new CarouselAdResponseModel { Id = "4", UserFirstName = "John3", Description = "Desc4", StarsRating = 5 },
            new CarouselAdResponseModel { Id = "5", UserFirstName = "John4", Description = "Desc5", StarsRating = 5 },
        };

        mockMemoryCache
       .Setup(x => x.TryGetValue(It.IsAny<object>(), out testData))
       .Returns(true);

        //Act
        ICollection<CarouselAdResponseModel> result = await adService.GetFiveAsync();

        //Assert
        Assert.AreEqual(testData, result);
    }

    [Test]
    public async Task GetFiveAsync_RetrievesFromRepo_WhenCacheIsEmpty()
    {
        //Arrange
        ICollection<CarouselAdModel> repoData = new List<CarouselAdModel>
        {
            new CarouselAdModel { Id = Guid.NewGuid(), UserFirstName = "John", Description = "Desc1", StarsRating = 5 },
            new CarouselAdModel { Id = Guid.NewGuid(), UserFirstName = "John1", Description = "Desc2", StarsRating = 5 },
        };

        //Act
        await repository.AddRangeAsync(repoData);
        await repository.SaveChangesAsync();

        ICollection<CarouselAdResponseModel> result = await adService.GetAllAsync();

        //Assert
        Assert.AreEqual(repoData.Last().UserFirstName, result.Last().UserFirstName);
    }

    [Test]
    public async Task DoesAdExistAsync_WhenAdExists_ReturnsTrue()
    {
        string testId = "7a3dcce3-1a68-49d2-84cc-f6636cd9c0ea";

        //Arrange
        ICollection<CarouselAdModel> repoData = new List<CarouselAdModel>
        {
            new CarouselAdModel { Id = Guid.Parse(testId), UserFirstName = "John", Description = "Desc1", StarsRating = 5 },
        };

        //Act
        await repository.AddRangeAsync(repoData);
        await repository.SaveChangesAsync();

        bool result = await adService.DoesAdExistAsync(testId);

        //Assert
        Assert.IsTrue(result);
    }

    [Test]
    public async Task DoesAdExistAsync_WhenAdExists_ReturnsFalse()
    {
        string addId = "333dcce3-1a68-49d2-84cc-f6636cd9c0ea";
        string testId = "7a3dcce3-1a68-49d2-84cc-f6636cd9c0ea";

        //Arrange
        ICollection<CarouselAdModel> repoData = new List<CarouselAdModel>
        {
            new CarouselAdModel { Id = Guid.Parse(addId), UserFirstName = "John", Description = "Desc1", StarsRating = 5 },
        };

        //Act
        await repository.AddRangeAsync(repoData);
        await repository.SaveChangesAsync();

        bool result = await adService.DoesAdExistAsync(testId);

        //Assert
        Assert.IsFalse(result);
    }

    [Test]
    public async Task GetDetailsAsync_WhenCalled_ReturnsExpectedCarouselAdResponseModel()
    {
        //Assert
        string addId = "333dcce3-1a68-49d2-84cc-f6636cd9c0ea";
        string firstName = "Michael";
        string description = "Test description";
        int starsRating = 5;

        CarouselAdModel model = new CarouselAdModel { Id = Guid.Parse(addId), UserFirstName = firstName, Description = description, StarsRating = starsRating };
        
        //Act
        await repository.AddAsync(model);
        await repository.SaveChangesAsync();

        CarouselAdResponseModel result = await adService.GetDetailsAsync(addId);

        //Assert
        Assert.AreEqual(addId, result.Id);
        Assert.AreEqual(firstName, result.UserFirstName);
        Assert.AreEqual(description, result.Description);
        Assert.AreEqual(starsRating, result.StarsRating);
    }

    [Test]
    public async Task EditAsync_WhenCalled_EditsACarouselAd()
    {
        //Assert
        string id = "333dcce3-1a68-49d2-84cc-f6636cd9c0ea";
        string firstName = "Michael";
        string description = "Test description";
        int starsRating = 5;

        CarouselAdModel model = new CarouselAdModel { Id = Guid.Parse(id), UserFirstName = "John", Description = "Desc1", StarsRating = 5 };

        CarouselAdFromRequestModel editModel = new CarouselAdFromRequestModel
        {
            UserFirstName = firstName,
            Description = description,
            StarsRating = starsRating
        };

        //Act

        await repository.AddAsync(model);
        await repository.SaveChangesAsync();

        CarouselAdResponseModel result = await adService.EditAsync(id, editModel);

        //Assert
        Assert.AreEqual(id, result.Id);
        Assert.AreEqual(firstName, result.UserFirstName);
        Assert.AreEqual(description, result.Description);
        Assert.AreEqual(starsRating, result.StarsRating);
    }


    [TearDown]
    public void TearDown()
    {
        applicationDbContext.Dispose();
    }
}