namespace CarCare_Companion.Tests.Services;

using System.Collections.Generic;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

using Moq;

using CarCare_Companion.Core.Models.TaxRecords;
using CarCare_Companion.Infrastructure.Data.Models.Records;
using CarCare_Companion.Infrastructure.Data.Models.Vehicle;
using CarCare_Companion.Infrastructure.Data.Models.Identity;
using CarCare_Companion.Infrastructure.Data.Common;
using CarCare_Companion.Infrastructure.Data;
using CarCare_Companion.Core.Services;


[TestFixture]
public class TaxRecordsServiceTests
{
    private IRepository repository;
    private TaxRecordsService taxRecordsService;
    private Mock<IMemoryCache> mockMemoryCache;
    private CarCareCompanionDbContext applicationDbContext;


    //Identifiers
    string userId = "0cda5780-8fa7-4e53-99b5-93c31c26f6ec";
    string vehicleId = "adc05780-8fa7-4e53-99b5-93c31c26f6ec";
    string taxRecordId = "77c05780-8fa7-4e53-99b5-93c31c26f6ec";


    [SetUp]
    public void Setup()
    {
        var contextOptions = new DbContextOptionsBuilder<CarCareCompanionDbContext>()
            .UseInMemoryDatabase("TaxRecordsDb")
            .Options;
        applicationDbContext = new CarCareCompanionDbContext(contextOptions);

        this.mockMemoryCache = new Mock<IMemoryCache>();
        this.repository = new Repository(applicationDbContext);
        this.taxRecordsService = new TaxRecordsService(repository, mockMemoryCache.Object);

        //Used for mocking the Set method
        var cacheEntry = Mock.Of<ICacheEntry>();
        mockMemoryCache
       .Setup(x => x.CreateEntry(It.IsAny<Object>()))
       .Returns(cacheEntry);

        applicationDbContext.Database.EnsureDeleted();
        applicationDbContext.Database.EnsureCreated();
    }

 
    private async Task<TaxRecord> SeedTaxRecord()
    {
        Vehicle vehicle = new Vehicle
        {
            Id = Guid.Parse(vehicleId),
            Make = "BMW",
            Model = "M5 CS",
            Year = 2022,
            FuelTypeId = 1,
            VehicleTypeId = 1,
            Mileage = 12000,
            OwnerId = Guid.Parse(userId),
            CreatedOn = DateTime.UtcNow,
            VehicleImageKey = Guid.NewGuid()
        };

        await repository.AddAsync(vehicle);
        await repository.SaveChangesAsync();

        TaxRecord taxRecord = new TaxRecord
        {
            Id = Guid.Parse(taxRecordId),
            Title = "Test",
            ValidFrom = DateTime.UtcNow,
            ValidTo = DateTime.UtcNow,
            Cost = 25,
            Description = "TestTest",
            VehicleId = Guid.Parse(vehicleId),
            OwnerId = Guid.Parse(userId),
            CreatedOn = DateTime.UtcNow.AddMinutes(1)
        };

        await repository.AddAsync(taxRecord);
        await repository.SaveChangesAsync();

        return taxRecord;
    }

    /// <summary>
    /// Tests if retrieving all user tax records from cache works
    /// </summary>
    [Test]
    public async Task GetAllByUserIdAsync_ReturnsDataFromCache_IfPresent()
    {
        //Assign   
        object testData = new List<TaxRecordDetailsResponseModel>()
        {
            new TaxRecordDetailsResponseModel 
            {
                Id = Guid.NewGuid().ToString(), 
                Title = " Test", 
                ValidFrom = DateTime.UtcNow, 
                ValidTo = DateTime.UtcNow, 
                Cost = 10, 
                VehicleMake = "BMW", 
                VehicleModel = "M5", 
                Description = "Test"
            },
             new TaxRecordDetailsResponseModel
            {
                Id = Guid.NewGuid().ToString(),
                Title = "TestTestTest",
                ValidFrom = DateTime.UtcNow,
                ValidTo = DateTime.UtcNow,
                Cost = 10,
                VehicleMake = "BMW",
                VehicleModel = "M3",
                Description = "TestTest"
            }
        };

        mockMemoryCache
        .Setup(x => x.TryGetValue(It.IsAny<object>(), out testData))
        .Returns(true);

        //Act 
        var result = await taxRecordsService.GetAllByUserIdAsync(userId);

        //Assert
        Assert.AreEqual(testData, result);
    }

    /// <summary>
    /// Tests if retrieving all user tax records from repository works
    /// </summary>
    [Test]
    public async Task GetAllByUserIdAsync_RetrievesFromRepo_WhenCacheIsEmpty()
    {
        //Assign          
        Vehicle vehicle = new Vehicle
        {
            Id = Guid.Parse(vehicleId),
            Make = "BMW",
            Model = "M5 CS",
            Year = 2022,
            FuelTypeId = 1,
            VehicleTypeId = 1,
            Mileage = 12000,
            OwnerId = Guid.Parse(userId),
            CreatedOn = DateTime.UtcNow,
            VehicleImageKey = Guid.NewGuid()
        };

        await repository.AddAsync(vehicle);
        await repository.SaveChangesAsync();

        List<TaxRecord> taxRecords = new List<TaxRecord>()
        {
            new TaxRecord
            {
                Title = "Test",
                ValidFrom = DateTime.UtcNow,
                ValidTo = DateTime.UtcNow,
                Cost = 25,
                Description = "TestTest",
                VehicleId = Guid.Parse(vehicleId),
                OwnerId = Guid.Parse(userId),
                CreatedOn = DateTime.UtcNow.AddMinutes(1)
            },
             new TaxRecord
            {
                Title = "Tes2t",
                ValidFrom = DateTime.UtcNow,
                ValidTo = DateTime.UtcNow,
                Cost = 25,
                Description = "TestTest2",
                VehicleId = Guid.Parse(vehicleId),
                OwnerId = Guid.Parse(userId),
                CreatedOn = DateTime.UtcNow
            }
        };

        await repository.AddRangeAsync(taxRecords);
        await repository.SaveChangesAsync();

        //Act 
        var result =(List<TaxRecordDetailsResponseModel>) await taxRecordsService.GetAllByUserIdAsync(userId);

        //Assert
        Assert.AreEqual(taxRecords[0].Id.ToString(), result[0].Id);
        Assert.AreEqual(taxRecords[0].Title, result[0].Title);
        Assert.AreEqual(taxRecords[0].ValidTo, result[0].ValidTo);
        Assert.AreEqual(taxRecords[0].ValidFrom, result[0].ValidFrom);
        Assert.AreEqual(taxRecords[0].Cost, result[0].Cost);
        Assert.AreEqual(taxRecords[0].Description, result[0].Description);
        Assert.AreEqual(vehicle.Make, result[0].VehicleMake);
        Assert.AreEqual(vehicle.Model, result[0].VehicleModel);
    }

    /// <summary>
    /// Tests if the tax record adding functionality works
    /// </summary>
    [Test]
    public async Task CreateAsync_ShouldAddATaxRecord()
    {
        //Assign          
        Vehicle vehicle = new Vehicle
        {
            Id = Guid.Parse(vehicleId),
            Make = "BMW",
            Model = "M5 CS",
            Year = 2022,
            FuelTypeId = 1,
            VehicleTypeId = 1,
            Mileage = 12000,
            OwnerId = Guid.Parse(userId),
            CreatedOn = DateTime.UtcNow,
            VehicleImageKey = Guid.NewGuid()
        };

        await repository.AddAsync(vehicle);
        await repository.SaveChangesAsync();

        TaxRecordFormRequestModel taxRecordToAdd = new TaxRecordFormRequestModel
        {
            Title = "Title",
            ValidFrom = DateTime.UtcNow,
            ValidTo = DateTime.UtcNow,
            Cost = 1,
            VehicleId = vehicleId,
            Description = "Description",
        };

        //Act
        var result = await taxRecordsService.CreateAsync(userId, taxRecordToAdd);

        //Asseert
        Assert.AreEqual(taxRecordToAdd.Title, result.Title);
        Assert.AreEqual(taxRecordToAdd.ValidTo, result.ValidTo);
        Assert.AreEqual(taxRecordToAdd.ValidFrom, result.ValidFrom);
        Assert.AreEqual(taxRecordToAdd.Cost, result.Cost);
        Assert.AreEqual(taxRecordToAdd.Description, result.Description);
    }

    /// <summary>
    /// Tests if the tax record editing functionality works
    /// </summary>
    [Test]
    public async Task EditAsync_WhenCalled_EditsAVehicle()
    {
        //Assign   
        SeedTaxRecord();

        TaxRecordFormRequestModel taxRecordToEdit = new TaxRecordFormRequestModel
        {
            Title = "Title",
            ValidFrom = DateTime.UtcNow,
            ValidTo = DateTime.UtcNow,
            Cost = 1,
            VehicleId = vehicleId,
            Description = "Description",
        };

        //Act
        var result = await taxRecordsService.EditAsync(taxRecordId, userId, taxRecordToEdit);

        //Asseert
        Assert.AreEqual(taxRecordToEdit.Title, result.Title);
        Assert.AreEqual(taxRecordToEdit.ValidTo, result.ValidTo);
        Assert.AreEqual(taxRecordToEdit.ValidFrom, result.ValidFrom);
        Assert.AreEqual(taxRecordToEdit.Cost, result.Cost);
        Assert.AreEqual(taxRecordToEdit.Description, result.Description);
        Assert.AreEqual(taxRecordToEdit.VehicleId, result.VehicleId);
    }

    /// <summary>
    /// Tests if tax record deleting functionality works
    /// </summary>
    [Test]
    public async Task DeleteAsync_ShouldDeleteAEntity()
    {
        //Assign   
        SeedTaxRecord();

        //Act 
        await taxRecordsService.DeleteAsync(taxRecordId, userId);

        TaxRecord deletedTaxRecord = await repository.GetByIdAsync<TaxRecord>(Guid.Parse(taxRecordId));

        //Assert
        Assert.IsTrue(deletedTaxRecord.IsDeleted);
    }

    /// <summary>
    /// Tests if a tax record exist by id
    /// </summary>
    [Test]
    public async Task DoesRecordExistByIdAsync_ReturnTrue_WhenRecordExists()
    {
        //Assign   
        SeedTaxRecord();

        //Act
        bool doesTaxRecordExists = await taxRecordsService.DoesRecordExistByIdAsync(taxRecordId);

        //Assert
        Assert.IsTrue(doesTaxRecordExists);
    }

    /// <summary>
    /// Tests if a tax record exist by id
    /// </summary>
    [Test]
    public async Task DoesRecordExistByIdAsync_ReturnFalse_WhenRecordDoesntExists()
    {
        //Assign   
        string nonExistingTaxRecordId = "000a5780-8fa7-4e53-99b5-93c31c26f6ec";

        //Act
        bool doesTaxRecordExists = await taxRecordsService.DoesRecordExistByIdAsync(nonExistingTaxRecordId);

        //Assert
        Assert.IsFalse(doesTaxRecordExists);
    }

    /// <summary>
    /// Tests if a user is a tax record creator
    /// </summary>
    [Test]
    public async Task IsUserRecordCreatorAsync_ShouldReturnTrue_WhenUserIsTaxRecordCreator()
    {
        //Assing
        TaxRecord taxRecord = await SeedTaxRecord();

        //Act
        bool isUserTaxRecordCreator = await taxRecordsService.IsUserRecordCreatorAsync(userId, taxRecordId);

        //Assert
        Assert.IsTrue(isUserTaxRecordCreator);
    }

    /// <summary>
    /// Tests if a user is a tax record creator
    /// </summary>
    [Test]
    public async Task IsUserRecordCreatorAsync_ShouldReturnFalse_WhenUserIsNotTaxRecordCreator()
    {
        //Assing
        string differentUserId = "000a5780-8fa7-4e53-99b5-93c31c26f6ec";

        //Act
        bool isUserTaxRecordCreator = await taxRecordsService.IsUserRecordCreatorAsync(differentUserId, taxRecordId);

        //Assert
        Assert.IsFalse(isUserTaxRecordCreator);
    }


    /// <summary>
    /// Tests if retrieving tax records details functionality works
    /// </summary>
    [Test]
    public async Task GetEditDetailsByIdAsync_ShouldReturnTaxRecordDetails()
    {
        //Assign
        TaxRecord taxRecord = await SeedTaxRecord();

        //Act
        TaxRecordEditDetailsResponseModel result = await taxRecordsService.GetEditDetailsByIdAsync(taxRecordId);

        //Assert
        Assert.AreEqual(taxRecord.Id.ToString(), result.Id);
        Assert.AreEqual(taxRecord.Title, result.Title);
        Assert.AreEqual(taxRecord.Cost, result.Cost);
        Assert.AreEqual(taxRecord.ValidFrom, result.ValidFrom);
        Assert.AreEqual(taxRecord.ValidTo, result.ValidTo);
        Assert.AreEqual(taxRecord.Description, result.Description);
        Assert.AreEqual(taxRecord.VehicleId.ToString(), result.VehicleId);

    }

    /// <summary>
    /// Tests if retrieving tax records count from cache works
    /// </summary>
    [Test]
    public async Task GetAllUserTripsCountAsync_ReturnsDataFromCache_IfPresent()
    {
        //Arrange
        int expectedCount = 5;

        object testData = 5;

        mockMemoryCache
        .Setup(x => x.TryGetValue(It.IsAny<object>(), out testData))
        .Returns(true);

        //Act 
        int result = await taxRecordsService.GetAllUserTaxRecordsCountAsync(userId);

        //Assert
        Assert.AreEqual(expectedCount, result);
    }


    /// <summary>
    /// Tests if retrieving tax records count from repository works
    /// </summary>
    [Test]
    public async Task GetAllUserTripsCountAsync_RetrievesFromRepo_WhenCacheIsEmpty()
    {
        //Arrange
        List<TaxRecord> taxRecords = new List<TaxRecord>()
        {
            new TaxRecord
            {
                Title = "Test",
                ValidFrom = DateTime.UtcNow,
                ValidTo = DateTime.UtcNow,
                Cost = 25,
                Description = "TestTest",
                VehicleId = Guid.Parse(vehicleId),
                OwnerId = Guid.Parse(userId),
                CreatedOn = DateTime.UtcNow.AddMinutes(1)
            },
             new TaxRecord
            {
                Title = "Tes2t",
                ValidFrom = DateTime.UtcNow,
                ValidTo = DateTime.UtcNow,
                Cost = 25,
                Description = "TestTest2",
                VehicleId = Guid.Parse(vehicleId),
                OwnerId = Guid.Parse(userId),
                CreatedOn = DateTime.UtcNow
            }
        };

        await repository.AddRangeAsync(taxRecords);
        await repository.SaveChangesAsync();

        //Act
        int result = await taxRecordsService.GetAllUserTaxRecordsCountAsync(userId);

        //Assert 
        Assert.AreEqual(taxRecords.Count, result);
    }


    /// <summary>
    /// Tests if retrieving tax records count from cache works
    /// </summary>
    [Test]
    public async Task GetAllUserTaxRecordsCostAsync_ReturnsDataFromCache_IfPresent()
    {
        //Arrange
        string userId = "0cda5780-8fa7-4e53-99b5-93c31c26f6ec";
        decimal expectedCost = 500;

        object testData = 500m;

        mockMemoryCache
        .Setup(x => x.TryGetValue(It.IsAny<object>(), out testData))
        .Returns(true);

        //Act 
        decimal result = await taxRecordsService.GetAllUserTaxRecordsCostAsync(userId);

        //Assert
        Assert.AreEqual(expectedCost, result);
    }


    /// <summary>
    /// Tests if retrieving tax records count details from repository works
    /// </summary>
    [Test]
    public async Task GetAllUserTripsCostAsync_RetrievesFromRepo_WhenCacheIsEmpty()
    {
        //Arrange
        List<TaxRecord> taxRecords = new List<TaxRecord>()
        {
            new TaxRecord
            {
                Title = "Test",
                ValidFrom = DateTime.UtcNow,
                ValidTo = DateTime.UtcNow,
                Cost = 25,
                Description = "TestTest",
                VehicleId = Guid.Parse(vehicleId),
                OwnerId = Guid.Parse(userId),
                CreatedOn = DateTime.UtcNow.AddMinutes(1)
            },
             new TaxRecord
            {
                Title = "Tes2t",
                ValidFrom = DateTime.UtcNow,
                ValidTo = DateTime.UtcNow,
                Cost = 25,
                Description = "TestTest2",
                VehicleId = Guid.Parse(vehicleId),
                OwnerId = Guid.Parse(userId),
                CreatedOn = DateTime.UtcNow
            }
        };

        await repository.AddRangeAsync(taxRecords);
        await repository.SaveChangesAsync();

        //Act
        decimal result = await taxRecordsService.GetAllUserTaxRecordsCostAsync(userId);

        //Assert 
        Assert.AreEqual(taxRecords.Sum(x => x.Cost), result);
    }


    /// <summary>
    /// Tests if the retrieved tax records are in the next two months from cache
    /// </summary>
    [Test]
    public async Task GetUpcomingTaxesAsync_ReturnsDataFromCache_IfPresent()
    {
        //Arrange
        int expectedRecordsCount = 2;

        string firstRecordId = "00005780-8fa7-4e53-99b5-93c31c26f6ec";
        string secondRecordId = "12305780-8fa7-4e53-99b5-93c31c26f6ec";

        object testData = new List<UpcomingTaxRecordResponseModel>()
        {
            new UpcomingTaxRecordResponseModel
            {
                Id = firstRecordId,
                Title = "Test",
                ValidTo = DateTime.UtcNow.AddMonths(1),
                VehicleMake = "BMW",
                VehicleModel = "M5"

            },
             new UpcomingTaxRecordResponseModel
            {
                Id = secondRecordId,
                Title = "Tes2t",
                ValidTo =  DateTime.UtcNow.AddMonths(2),
                VehicleMake = "Porsche",
                VehicleModel = "GT3RS"

            }
        };

        mockMemoryCache
        .Setup(x => x.TryGetValue(It.IsAny<object>(), out testData))
        .Returns(true);

        //Act
        var result = await taxRecordsService.GetUpcomingTaxesAsync(userId, 2);

        //Assert
        Assert.AreEqual(testData, result);
    }

    /// <summary>
    /// Tests if the retrieved tax records are in the next two months from repository
    /// </summary>
    [Test]
    public async Task GetUpcomingTaxesAsync_RetrievesFromRepo_WhenCacheIsEmpty()
    {
        //Arrange
        int expectedRecordsCount = 2;

        string firstRecordId = "00005780-8fa7-4e53-99b5-93c31c26f6ec";
        string secondRecordId = "12305780-8fa7-4e53-99b5-93c31c26f6ec";

        Vehicle vehicle = new Vehicle
        {
            Id = Guid.Parse(vehicleId),
            Make = "BMW",
            Model = "M5 CS",
            Year = 2022,
            FuelTypeId = 1,
            VehicleTypeId = 1,
            Mileage = 12000,
            OwnerId = Guid.Parse(userId),
            CreatedOn = DateTime.UtcNow,
            VehicleImageKey = Guid.NewGuid()
        };

        await repository.AddAsync(vehicle);
        await repository.SaveChangesAsync();


        List<TaxRecord> taxRecords = new List<TaxRecord>()
        {
            new TaxRecord
            {
                Id = Guid.Parse(firstRecordId),
                Title = "Test",
                ValidFrom = DateTime.UtcNow,
                ValidTo = DateTime.UtcNow.AddMonths(1),
                Cost = 25,
                Description = "TestTest",
                VehicleId = Guid.Parse(vehicleId),
                OwnerId = Guid.Parse(userId),
                CreatedOn = DateTime.UtcNow
            },
             new TaxRecord
            {
                Id = Guid.Parse(secondRecordId),
                Title = "Tes2t",
                ValidFrom = DateTime.UtcNow,
                ValidTo =  DateTime.UtcNow.AddMonths(2),
                Cost = 25,
                Description = "TestTest2",
                VehicleId = Guid.Parse(vehicleId),
                OwnerId = Guid.Parse(userId),
                CreatedOn = DateTime.UtcNow
            },
              new TaxRecord
            {
                Title = "Tes23",
                ValidFrom = DateTime.UtcNow,
                ValidTo =  DateTime.UtcNow.AddMonths(3),
                Cost = 25,
                Description = "TestTest2",
                VehicleId = Guid.Parse(vehicleId),
                OwnerId = Guid.Parse(userId),
                CreatedOn = DateTime.UtcNow
            }
        };

        await repository.AddRangeAsync(taxRecords);
        await repository.SaveChangesAsync();

        //Act
        var result =(List<UpcomingTaxRecordResponseModel>) await taxRecordsService.GetUpcomingTaxesAsync(userId, 3);

        //Assert
        Assert.AreEqual(expectedRecordsCount, result.Count);
        Assert.AreEqual(taxRecords[0].Id.ToString(), result[0].Id);
        Assert.AreEqual(taxRecords[0].Title.ToString(), result[0].Title);
        Assert.AreEqual(taxRecords[0].ValidTo, result[0].ValidTo);
        Assert.AreEqual(taxRecords[0].Vehicle.Make, result[0].VehicleMake);
        Assert.AreEqual(taxRecords[0].Vehicle.Model, result[0].VehicleModel);
        Assert.AreEqual(taxRecords[1].Id.ToString(), result[1].Id);
        Assert.AreEqual(taxRecords[1].Title.ToString(), result[1].Title);
        Assert.AreEqual(taxRecords[1].ValidTo, result[1].ValidTo);
        Assert.AreEqual(taxRecords[1].Vehicle.Make, result[1].VehicleMake);
        Assert.AreEqual(taxRecords[1].Vehicle.Model, result[1].VehicleModel);
    }

    /// <summary>
    /// Tests if the retrieved tax records are for the next day
    /// </summary>
    [Test]
    public async Task GetUpcomingUsersTaxesAsync_RetrievesUserUpcomingTrips()
    {
        //Arrange
        int expectedRecordsCount = 2;

        string firstRecordId = "00005780-8fa7-4e53-99b5-93c31c26f6ec";
        string secondRecordId = "12305780-8fa7-4e53-99b5-93c31c26f6ec";
        string firstRecordUserId = "12f05780-8fa7-4e53-99b5-93c31c26f6ec";
        string secondRecordUserId = "f2f05780-8fa7-4e53-99b5-93c31c26f6ec";

        List<ApplicationUser> users = new List<ApplicationUser>()
        {
         new ApplicationUser
        {
            Id = Guid.Parse(firstRecordUserId),
            FirstName = "Test",
            LastName = "Test",
        },

        new ApplicationUser
        {
            Id = Guid.Parse(secondRecordUserId),
            FirstName = "Test2",
            LastName = "Test2",
        }
        };


        await repository.AddRangeAsync(users);
        await repository.SaveChangesAsync();

        Vehicle vehicle = new Vehicle
        {
            Id = Guid.Parse(vehicleId),
            Make = "BMW",
            Model = "M5 CS",
            Year = 2022,
            FuelTypeId = 1,
            VehicleTypeId = 1,
            Mileage = 12000,
            OwnerId = Guid.Parse(userId),
            CreatedOn = DateTime.UtcNow,
            VehicleImageKey = Guid.NewGuid()
        };

        await repository.AddAsync(vehicle);
        await repository.SaveChangesAsync();

        List<TaxRecord> taxRecords = new List<TaxRecord>()
        {
            new TaxRecord
            {
                Id = Guid.Parse(firstRecordId),
                Title = "Test",
                ValidFrom = DateTime.UtcNow,
                ValidTo = DateTime.UtcNow.AddDays(1),
                Cost = 25,
                Description = "TestTest",
                VehicleId = Guid.Parse(vehicleId),
                OwnerId = Guid.Parse(firstRecordUserId),
                CreatedOn = DateTime.UtcNow
            },
             new TaxRecord
            {
                Id = Guid.Parse(secondRecordId),
                Title = "Tes2t",
                ValidFrom = DateTime.UtcNow,
                ValidTo =  DateTime.UtcNow.AddDays(1),
                Cost = 25,
                Description = "TestTest2",
                VehicleId = Guid.Parse(vehicleId),
                OwnerId = Guid.Parse(secondRecordUserId),
                CreatedOn = DateTime.UtcNow.AddMinutes(1)
            },
              new TaxRecord
            {
                Title = "Tes23",
                ValidFrom = DateTime.UtcNow,
                ValidTo =  DateTime.UtcNow.AddDays(3),
                Cost = 25,
                Description = "TestTest2",
                VehicleId = Guid.Parse(vehicleId),
                OwnerId = Guid.Parse(firstRecordUserId),
                CreatedOn = DateTime.UtcNow
            }
        };

        await repository.AddRangeAsync(taxRecords);
        await repository.SaveChangesAsync();

        //Act
        var result =(List<UpcomingUserTaxResponseModel>)await taxRecordsService.GetUpcomingUsersTaxesAsync();
        
        //Assert
        Assert.AreEqual(expectedRecordsCount, result.Count);
        Assert.AreEqual(taxRecords[0].Vehicle.Make, result[0].VehicleMake);
        Assert.AreEqual(taxRecords[0].Vehicle.Model, result[0].VehicleModel);
        Assert.AreEqual(users[0].FirstName, result[0].FirstName);
        Assert.AreEqual(users[0].LastName, result[0].LastName);
        Assert.AreEqual(users[0].Email, result[0].Email);
        Assert.AreEqual(taxRecords[1].Vehicle.Make, result[1].VehicleMake);
        Assert.AreEqual(taxRecords[1].Vehicle.Model, result[1].VehicleModel);
        Assert.AreEqual(users[1].FirstName, result[1].FirstName);
        Assert.AreEqual(users[1].LastName, result[1].LastName);
        Assert.AreEqual(users[1].Email, result[1].Email);
    }
}
