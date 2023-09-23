namespace CarCare_Companion.Tests.Unit_Tests.Services;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.EntityFrameworkCore;

using Moq;

using CarCare_Companion.Core.Services;
using CarCare_Companion.Infrastructure.Data.Common;
using CarCare_Companion.Infrastructure.Data;
using CarCare_Companion.Core.Contracts;
using CarCare_Companion.Core.Models.Admin.Users;
using CarCare_Companion.Infrastructure.Data.Models.Identity;
using CarCare_Companion.Infrastructure.Data.Models.Vehicle;
using CarCare_Companion.Infrastructure.Data.Models.Records;

using static Common.GlobalConstants;


[TestFixture]
public class UserServiceTests
{
    private IRepository repository;
    private UserService userService;
    private Mock<IMemoryCache> mockMemoryCache;
    private Mock<IIdentityService> mockIdentityService;
    private CarCareCompanionDbContext applicationDbContext;

    //Identifiers
    string userId = "0cda5780-8fa7-4e53-99b5-93c31c26f6ec";

    [SetUp]
    public void Setup()
    {
        var contextOptions = new DbContextOptionsBuilder<CarCareCompanionDbContext>()
           .UseInMemoryDatabase("ServiceRecordsDb")
           .Options;
        applicationDbContext = new CarCareCompanionDbContext(contextOptions);

        this.mockMemoryCache = new Mock<IMemoryCache>();
        this.mockIdentityService = new Mock<IIdentityService>();
        this.repository = new Repository(applicationDbContext);
        this.userService = new UserService(repository, mockIdentityService.Object, mockMemoryCache.Object);

        //Used for mocking the Set method
        var cacheEntry = Mock.Of<ICacheEntry>();
        mockMemoryCache
       .Setup(x => x.CreateEntry(It.IsAny<Object>()))
       .Returns(cacheEntry);

        applicationDbContext.Database.EnsureDeleted();
        applicationDbContext.Database.EnsureCreated();
    }


    [Test]
    public async Task GetAllByUserIdAsync_ReturnsDataFromCache_IfPresent()
    {
        //Arrange   
        object testData = new List<UserInformationResponseModel>()
        {
            new UserInformationResponseModel
            {
                UserId = Guid.NewGuid().ToString(),
                Username = "Test1",
            },
             new UserInformationResponseModel
            {
                UserId = Guid.NewGuid().ToString(),
                Username = "Test2",
            }
        };

        mockMemoryCache
        .Setup(x => x.TryGetValue(It.IsAny<object>(), out testData))
        .Returns(true);

        //Act 
        var result = await userService.GetAllUsersAsync();

        //Assert
        Assert.AreEqual(testData, result);
    }

    [Test]
    public async Task GetAllByUserIdAsync_RetrievesFromRepo_WhenCacheIsEmpty()
    {
        //Arrange
        ICollection<ApplicationUser> applicationUsers = new List<ApplicationUser>()
        {
            new ApplicationUser
            {
                Id = Guid.NewGuid(),
                UserName = "Test1",
            },
             new ApplicationUser
            {
                Id = Guid.NewGuid(),
                UserName = "Test1",
            }
        };

        await repository.AddRangeAsync(applicationUsers);
        await repository.SaveChangesAsync();

        //Act
        var result = (List<UserInformationResponseModel>)await userService.GetAllUsersAsync();

        //Assert
        Assert.AreEqual(applicationUsers.Count, result.Count);

    }

    [Test]
    public async Task GetUserDetailsByIdAsync_ShoudlRetrieve_UserDetails()
    {
        //Arrange
        string vehicleId = "00aa5780-8fa7-4e53-99b5-93c31c26f6ec";
        int expectedAllTypeOfRecordsCount = 1;
        bool isUserAdmin = true;

        ApplicationUser user = new ApplicationUser
        {
            Id = Guid.Parse(userId),
            FirstName = "Mike",
            LastName = "Tyson",
            UserName = "bigMike@mail.com",
        };

        await repository.AddAsync(user);

        Vehicle userVehicle = new Vehicle
        {
            Id = Guid.Parse(vehicleId),
            OwnerId = Guid.Parse(userId),
            Make = "BMW",
            Model = "535D"
        };

        await repository.AddAsync(userVehicle);

        ServiceRecord userServiceRecord = new ServiceRecord
        {
            Id = Guid.NewGuid(),
            Title = "Test",
            Cost = 1,
            Mileage = 5,
            VehicleId = Guid.Parse(vehicleId),
            OwnerId = Guid.Parse(userId)
        };

        await repository.AddAsync(userServiceRecord);

        TaxRecord userTaxRecord = new TaxRecord
        {
            Id = Guid.NewGuid(),
            Title = "Test",
            ValidFrom = DateTime.Now,
            ValidTo = DateTime.Now,
            VehicleId = Guid.Parse(vehicleId),
            Cost = 1,
            OwnerId = Guid.Parse(userId)
        };

        await repository.AddAsync(userTaxRecord);

        TripRecord userTripRecord = new TripRecord
        {
            Id = Guid.NewGuid(),
            StartDestination = "Test",
            EndDestination = "TestTest",
            VehicleId = Guid.Parse(vehicleId),
            OwnerId = Guid.Parse(userId)
        };

        await repository.AddAsync(userTripRecord);
        await repository.SaveChangesAsync();

        mockIdentityService
        .Setup(x => x.IsUserInRoleAsync(It.IsAny<string>(), AdministratorRoleName))
        .ReturnsAsync(isUserAdmin);
        //Act
        var result = await userService.GetUserDetailsByIdAsync(userId);

        //Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(user.FirstName, result.FirstName);
        Assert.AreEqual(user.LastName, result.LastName);
        Assert.AreEqual(user.Id.ToString(), result.UserId);
        Assert.AreEqual(user.FirstName, result.FirstName);
        Assert.AreEqual(expectedAllTypeOfRecordsCount, result.VehiclesCount);
        Assert.AreEqual(expectedAllTypeOfRecordsCount, result.TaxRecordsCount);
        Assert.AreEqual(expectedAllTypeOfRecordsCount, result.ServiceRecordsCount);
        Assert.AreEqual(expectedAllTypeOfRecordsCount, result.TripsCount);
        Assert.AreEqual(isUserAdmin, result.IsAdmin);

    }


    [TearDown]
    public void TearDown()
    {
        applicationDbContext.Dispose();
    }
}
