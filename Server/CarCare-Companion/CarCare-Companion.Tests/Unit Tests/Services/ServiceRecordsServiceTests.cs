namespace CarCare_Companion.Tests.Unit_Tests.Services;

using System.Linq;
using System.Collections.Generic;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

using Moq;

using CarCare_Companion.Core.Models.ServiceRecords;
using CarCare_Companion.Core.Services;
using CarCare_Companion.Infrastructure.Data;
using CarCare_Companion.Infrastructure.Data.Common;
using CarCare_Companion.Infrastructure.Data.Models.Records;
using CarCare_Companion.Infrastructure.Data.Models.Vehicle;



[TestFixture]
public class ServiceRecordsServiceTests
{
    private IRepository repository;
    private ServiceRecordsService serviceRecordsService;
    private Mock<IMemoryCache> mockMemoryCache;
    private CarCareCompanionDbContext applicationDbContext;


    //Identifiers
    string userId = "0cda5780-8fa7-4e53-99b5-93c31c26f6ec";
    string vehicleId = "adc05780-8fa7-4e53-99b5-93c31c26f6ec";
    string serviceRecordId = "77c05780-8fa7-4e53-99b5-93c31c26f6ec";


    [SetUp]
    public void Setup()
    {
        var contextOptions = new DbContextOptionsBuilder<CarCareCompanionDbContext>()
            .UseInMemoryDatabase("ServiceRecordsDb")
            .Options;
        applicationDbContext = new CarCareCompanionDbContext(contextOptions);

        this.mockMemoryCache = new Mock<IMemoryCache>();
        this.repository = new Repository(applicationDbContext);
        this.serviceRecordsService = new ServiceRecordsService(repository, mockMemoryCache.Object);

        //Used for mocking the Set method
        var cacheEntry = Mock.Of<ICacheEntry>();
        mockMemoryCache
       .Setup(x => x.CreateEntry(It.IsAny<Object>()))
       .Returns(cacheEntry);

        applicationDbContext.Database.EnsureDeleted();
        applicationDbContext.Database.EnsureCreated();
    }

    private async Task<ServiceRecord> SeedServiceRecord()
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

        ServiceRecord serviceRecord = new ServiceRecord()
        {
            Id = Guid.Parse(serviceRecordId),
            Title = "Test",
            Description = "Test2",
            Cost = 150,
            Mileage = 1500,
            VehicleId = Guid.Parse(vehicleId),
            OwnerId = Guid.Parse(userId),
            PerformedOn = DateTime.UtcNow,
            CreatedOn = DateTime.UtcNow

        };

        await repository.AddAsync(serviceRecord);
        await repository.SaveChangesAsync();

        return serviceRecord;
    }


    /// <summary>
    /// Tests the service record adding functionality with correct data
    /// </summary>
    [Test]
    public async Task CreateAsync_ShouldAddANewServiceRecord()
    {
        //Arrange
        ServiceRecordFormRequestModel serviceRecordToCreate = new ServiceRecordFormRequestModel()
        {
            Title = "Title",
            PerformedOn = DateTime.UtcNow,
            Mileage = 25,
            Cost = 100,
            Description = "Description",
            VehicleId = vehicleId,
        };

        //Act
        ServiceRecordResponseModel createdTrip = await serviceRecordsService.CreateAsync(userId, serviceRecordToCreate);

        //Assert
        Assert.IsNotNull(createdTrip);
        Assert.AreEqual(serviceRecordToCreate.Title, createdTrip.Title);
        Assert.AreEqual(serviceRecordToCreate.PerformedOn, createdTrip.PerformedOn);
        Assert.AreEqual(serviceRecordToCreate.Mileage, createdTrip.Mileage);
        Assert.AreEqual(serviceRecordToCreate.Description, createdTrip.Description);
        Assert.AreEqual(serviceRecordToCreate.VehicleId, createdTrip.VehicleId);

    }

    /// <summary>
    /// Tests the service record editing functionality with correct data
    /// </summary>
    [Test]
    public async Task EditAsync_WhenCalled_EditsAVehicle()
    {
        //Arrange
        await SeedServiceRecord();

        ServiceRecordFormRequestModel serviceRecordToEdit = new ServiceRecordFormRequestModel()
        {
            Title = "Test3333",
            Description = "Test233333",
            Cost = 1560,
            VehicleId = vehicleId,
            Mileage = 17000,
            PerformedOn = DateTime.UtcNow.AddDays(1),
        };

        //Act
        ServiceRecordResponseModel editedServiceRecord = await serviceRecordsService.EditAsync(serviceRecordId, userId, serviceRecordToEdit);

        //Assert
        Assert.IsNotNull(editedServiceRecord);
        Assert.AreEqual(serviceRecordToEdit.Title, editedServiceRecord.Title);
        Assert.AreEqual(serviceRecordToEdit.Description, editedServiceRecord.Description);
        Assert.AreEqual(serviceRecordToEdit.Mileage, editedServiceRecord.Mileage);
        Assert.AreEqual(serviceRecordToEdit.Cost, editedServiceRecord.Cost);
        Assert.AreEqual(serviceRecordToEdit.PerformedOn, editedServiceRecord.PerformedOn);
        Assert.AreEqual(serviceRecordToEdit.VehicleId.ToString(), editedServiceRecord.VehicleId);

    }

    /// <summary>
    /// Tests if service record deleting functionality works
    /// </summary>
    [Test]
    public async Task DeleteAsync_ShouldDeleteAEntity()
    {
        //Assign   
        await SeedServiceRecord();

        //Act 
        await serviceRecordsService.DeleteAsync(serviceRecordId, userId);

        ServiceRecord deletedServiceRecord = await repository.GetByIdAsync<ServiceRecord>(Guid.Parse(serviceRecordId));

        //Assert
        Assert.IsTrue(deletedServiceRecord.IsDeleted);
    }

    /// <summary>
    /// Tests if retrieving all user service records from repository works
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

        List<ServiceRecord> serviceRecords = new List<ServiceRecord>()
        {
            new ServiceRecord
            {
               Id = Guid.NewGuid(),
               Title = "Test3",
               Description = "Test2",
               Cost = 150,
               Mileage = 1500,
               VehicleId = Guid.Parse(vehicleId),
               OwnerId = Guid.Parse(userId),
               PerformedOn = DateTime.UtcNow.AddDays(2),
               CreatedOn = DateTime.UtcNow.AddDays(2)
            },
             new ServiceRecord
            {
               Id = Guid.NewGuid(),
               Title = "Test3444",
               Description = "Test2344",
               Cost = 1530,
               Mileage = 15005,
               VehicleId = Guid.Parse(vehicleId),
               OwnerId = Guid.Parse(userId),
               PerformedOn = DateTime.UtcNow,
               CreatedOn = DateTime.UtcNow
            }
        };

        await repository.AddRangeAsync(serviceRecords);
        await repository.SaveChangesAsync();

        //Act 
        var result = (List<ServiceRecordDetailsResponseModel>)await serviceRecordsService.GetAllByUserIdAsync(userId);

        //Assert
        Assert.AreEqual(serviceRecords[0].Id.ToString(), result[0].Id);
        Assert.AreEqual(serviceRecords[0].Title, result[0].Title);
        Assert.AreEqual(serviceRecords[0].Mileage, result[0].Mileage);
        Assert.AreEqual(serviceRecords[0].Cost, result[0].Cost);
        Assert.AreEqual(serviceRecords[0].Description, result[0].Description);
        Assert.AreEqual(vehicle.Make, result[0].VehicleMake);
        Assert.AreEqual(vehicle.Model, result[0].VehicleModel);
    }

    /// <summary>
    /// Tests if retrieving all user service records from cache works
    /// </summary>
    [Test]
    public async Task GetAllByUserIdAsync_ReturnsDataFromCache_IfPresent()
    {
        //Assign   
        object testData = new List<ServiceRecordDetailsResponseModel>()
        {
            new ServiceRecordDetailsResponseModel
            {
                Id = Guid.NewGuid().ToString(),
                Title = "Test3",
                Description = "Test2",
                Cost = 150,
                Mileage = 1500,
                VehicleMake = "BMW",
                VehicleModel = "M5 CS",
                PerformedOn = DateTime.UtcNow.AddDays(2)
            },
             new ServiceRecordDetailsResponseModel
            {
                Id = Guid.NewGuid().ToString(),
                Title = "Test345",
                Description = "Test1122",
                Cost = 1507,
                Mileage = 15500,
                VehicleMake = "VW",
                VehicleModel = "Passat TDI",
                PerformedOn = DateTime.UtcNow
            }
        };

        mockMemoryCache
        .Setup(x => x.TryGetValue(It.IsAny<object>(), out testData))
        .Returns(true);

        //Act 
        var result = await serviceRecordsService.GetAllByUserIdAsync(userId);

        //Assert
        Assert.AreEqual(testData, result);
    }

    /// <summary>
    /// Tests if retrieving service records details functionality works
    /// </summary>
    [Test]
    public async Task GetEditDetailsByIdAsync_ShouldReturnServiceRecordDetails()
    {
        //Assign
        ServiceRecord serviceRecord = await SeedServiceRecord();

        //Act
        ServiceRecordEditDetailsResponseModel result = await serviceRecordsService.GetEditDetailsByIdAsync(serviceRecordId);

        //Assert
        Assert.AreEqual(serviceRecord.Id.ToString(), result.Id);
        Assert.AreEqual(serviceRecord.Title, result.Title);
        Assert.AreEqual(serviceRecord.Cost, result.Cost);
        Assert.AreEqual(serviceRecord.Mileage, result.Mileage);
        Assert.AreEqual(serviceRecord.PerformedOn, result.PerformedOn);
        Assert.AreEqual(serviceRecord.Description, result.Description);
        Assert.AreEqual(serviceRecord.VehicleId.ToString(), result.VehicleId);

    }

    /// <summary>
    /// Tests if a service record exist by id
    /// </summary>
    [Test]
    public async Task DoesRecordExistByIdAsync_ReturnTrue_WhenRecordExists()
    {
        //Assign   
        await SeedServiceRecord();

        //Act
        bool doesServiceRecordExists = await serviceRecordsService.DoesRecordExistByIdAsync(serviceRecordId);

        //Assert
        Assert.IsTrue(doesServiceRecordExists);
    }

    /// <summary>
    /// Tests if a service record exist by id
    /// </summary>
    [Test]
    public async Task DoesRecordExistByIdAsync_ReturnFalse_WhenRecordDoesntExists()
    {
        //Assign   
        string nonExistingServiceRecordId = "000a5780-8fa7-4e53-99b5-93c31c26f6ec";

        //Act
        bool doesServiceRecordExists = await serviceRecordsService.DoesRecordExistByIdAsync(nonExistingServiceRecordId);

        //Assert
        Assert.IsFalse(doesServiceRecordExists);
    }

    /// <summary>
    /// Tests if a user is a service record creator
    /// </summary>
    [Test]
    public async Task IsUserRecordCreatorAsync_ShouldReturnTrue_WhenUserIsServiceRecordCreator()
    {
        //Assing
        ServiceRecord serviceRecord = await SeedServiceRecord();

        //Act
        bool isUserServiceRecordCreator = await serviceRecordsService.IsUserRecordCreatorAsync(userId, serviceRecordId);

        //Assert
        Assert.IsTrue(isUserServiceRecordCreator);
    }

    /// <summary>
    /// Tests if a user is a service record creator
    /// </summary>
    [Test]
    public async Task IsUserRecordCreatorAsync_ShouldReturnFalse_WhenUserIsNotServiceRecordCreator()
    {
        //Assing
        string differentUserId = "000a5780-8fa7-4e53-99b5-93c31c26f6ec";

        //Act
        bool isUserServiceRecordCreator = await serviceRecordsService.IsUserRecordCreatorAsync(differentUserId, serviceRecordId);

        //Assert
        Assert.IsFalse(isUserServiceRecordCreator);
    }

    /// <summary>
    /// Tests if retrieving service records count from cache works
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
        int result = await serviceRecordsService.GetAllUserServiceRecordsCountAsync(userId);

        //Assert
        Assert.AreEqual(expectedCount, result);
    }


    /// <summary>
    /// Tests if retrieving service records count from repository works
    /// </summary>
    [Test]
    public async Task GetAllUserTripsCountAsync_RetrievesFromRepo_WhenCacheIsEmpty()
    {
        //Arrange
        List<ServiceRecord> serviceRecords = new List<ServiceRecord>()
        {
             new ServiceRecord
            {
               Id = Guid.NewGuid(),
               Title = "Test3",
               Description = "Test2",
               Cost = 150,
               Mileage = 1500,
               VehicleId = Guid.Parse(vehicleId),
               OwnerId = Guid.Parse(userId),
               PerformedOn = DateTime.UtcNow.AddDays(2),
               CreatedOn = DateTime.UtcNow.AddDays(2)
            },
             new ServiceRecord
            {
               Id = Guid.NewGuid(),
               Title = "Test3444",
               Description = "Test2344",
               Cost = 1530,
               Mileage = 15005,
               VehicleId = Guid.Parse(vehicleId),
               OwnerId = Guid.Parse(userId),
               PerformedOn = DateTime.UtcNow,
               CreatedOn = DateTime.UtcNow
            }
        };

        await repository.AddRangeAsync(serviceRecords);
        await repository.SaveChangesAsync();

        //Act
        int result = await serviceRecordsService.GetAllUserServiceRecordsCountAsync(userId);

        //Assert 
        Assert.AreEqual(serviceRecords.Count, result);
    }


    /// <summary>
    /// Tests if retrieving service records count from cache works
    /// </summary>
    [Test]
    public async Task GetAllUserServiceRecordsCostAsync_ReturnsDataFromCache_IfPresent()
    {
        //Arrange
        string userId = "0cda5780-8fa7-4e53-99b5-93c31c26f6ec";
        decimal expectedCost = 500;

        object testData = 500m;

        mockMemoryCache
        .Setup(x => x.TryGetValue(It.IsAny<object>(), out testData))
        .Returns(true);

        //Act 
        decimal result = await serviceRecordsService.GetAllUserServiceRecordsCostAsync(userId);

        //Assert
        Assert.AreEqual(expectedCost, result);
    }


    /// <summary>
    /// Tests if retrieving service records count details from repository works
    /// </summary>
    [Test]
    public async Task GetAllUserTripsCostAsync_RetrievesFromRepo_WhenCacheIsEmpty()
    {
        //Arrange
        List<ServiceRecord> serviceRecords = new List<ServiceRecord>()
        {
             new ServiceRecord
            {
               Id = Guid.NewGuid(),
               Title = "Test3",
               Description = "Test2",
               Cost = 150,
               Mileage = 1500,
               VehicleId = Guid.Parse(vehicleId),
               OwnerId = Guid.Parse(userId),
               PerformedOn = DateTime.UtcNow.AddDays(2),
               CreatedOn = DateTime.UtcNow.AddDays(2)
            },
             new ServiceRecord
            {
               Id = Guid.NewGuid(),
               Title = "Test3444",
               Description = "Test2344",
               Cost = 1530,
               Mileage = 15005,
               VehicleId = Guid.Parse(vehicleId),
               OwnerId = Guid.Parse(userId),
               PerformedOn = DateTime.UtcNow,
               CreatedOn = DateTime.UtcNow
            }
        };

        await repository.AddRangeAsync(serviceRecords);
        await repository.SaveChangesAsync();

        //Act
        decimal result = await serviceRecordsService.GetAllUserServiceRecordsCostAsync(userId);

        //Assert 
        Assert.AreEqual(serviceRecords.Sum(x => x.Cost), result);
    }

    /// <summary>
    /// Tests if retrieving N service records from cache works
    /// </summary>
    [Test]
    public async Task GetLastNCountAsync_ReturnsDataFromCache_IfPresent()
    {
        //Arrange
        object testData = new List<ServiceRecordBasicInformationResponseModel>()
        {
            new ServiceRecordBasicInformationResponseModel
            {
                Id = Guid.NewGuid().ToString(),
                Title = "Test3",
                VehicleMake = "BMW",
                VehicleModel = "M5 CS",
                PerformedOn = DateTime.UtcNow.AddDays(2)
            },
             new ServiceRecordBasicInformationResponseModel
            {
                Id = Guid.NewGuid().ToString(),
                Title = "Test345",
                VehicleMake = "VW",
                VehicleModel = "Passat TDI",
                PerformedOn = DateTime.UtcNow,
            }
        };

        mockMemoryCache
        .Setup(x => x.TryGetValue(It.IsAny<object>(), out testData))
        .Returns(true);

        //Act
        var result = await serviceRecordsService.GetLastNCountAsync(userId, 2);

        //Assert
        Assert.AreEqual(testData, result);
    }

    /// <summary>
    /// Tests if retrieving N service records from repository works
    /// </summary>
    [Test]
    public async Task GetLastNCountAsync_RetrievesFromRepo_WhenCacheIsEmpty()
    {
        //Arrange
        int expectedCount = 2;

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

        List<ServiceRecord> serviceRecords = new List<ServiceRecord>()
        {
             new ServiceRecord
            {
               Id = Guid.NewGuid(),
               Title = "Test1",
               Description = "Test1",
               Cost = 150,
               Mileage = 1500,
               VehicleId = Guid.Parse(vehicleId),
               OwnerId = Guid.Parse(userId),
               PerformedOn = DateTime.UtcNow.AddDays(2),
               CreatedOn = DateTime.UtcNow.AddDays(2)
            },
             new ServiceRecord
            {
               Id = Guid.NewGuid(),
               Title = "Test2",
               Description = "Test2",
               Cost = 1530,
               Mileage = 15005,
               VehicleId = Guid.Parse(vehicleId),
               OwnerId = Guid.Parse(userId),
               PerformedOn = DateTime.UtcNow,
               CreatedOn = DateTime.UtcNow
            },
             new ServiceRecord
            {
               Id = Guid.NewGuid(),
               Title = "Test3",
               Description = "Test3",
               Cost = 1530,
               Mileage = 15005,
               VehicleId = Guid.Parse(vehicleId),
               OwnerId = Guid.Parse(userId),
               PerformedOn = DateTime.UtcNow,
               CreatedOn = DateTime.UtcNow
            }
        };

        await repository.AddRangeAsync(serviceRecords);
        await repository.SaveChangesAsync();

        //Act
        var result = await serviceRecordsService.GetLastNCountAsync(userId, expectedCount);

        //Assert
        Assert.AreEqual(expectedCount, result.Count);
    }

    /// <summary>
    /// Tests if retrieving recent service records for a vehicle work
    /// </summary>
    [Test]
    public async Task GetRecentByVehicleId_RetrievesRecentServiceRecordForVehicle()
    {
        //Arange
        int expectedCount = 2;
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

        List<ServiceRecord> serviceRecords = new List<ServiceRecord>()
        {
             new ServiceRecord
            {
               Id = Guid.NewGuid(),
               Title = "Test1",
               Description = "Test1",
               Cost = 150,
               Mileage = 1500,
               VehicleId = Guid.Parse(vehicleId),
               OwnerId = Guid.Parse(userId),
               PerformedOn = DateTime.UtcNow,
               CreatedOn = DateTime.UtcNow
            },
             new ServiceRecord
            {
               Id = Guid.NewGuid(),
               Title = "Test2",
               Description = "Test2",
               Cost = 1530,
               Mileage = 15005,
               VehicleId = Guid.Parse(vehicleId),
               OwnerId = Guid.Parse(userId),
               PerformedOn = DateTime.UtcNow.AddDays(-1),
               CreatedOn = DateTime.UtcNow.AddDays(-1)
            },
             new ServiceRecord
            {
               Id = Guid.NewGuid(),
               Title = "Test3",
               Description = "Test3",
               Cost = 1530,
               Mileage = 15005,
               VehicleId = Guid.Parse(vehicleId),
               OwnerId = Guid.Parse(userId),
               PerformedOn = DateTime.UtcNow.AddDays(-2),
               CreatedOn = DateTime.UtcNow.AddDays(-2)
            }
        };

        await repository.AddRangeAsync(serviceRecords);
        await repository.SaveChangesAsync();

        //Act
        var result = (List<ServiceRecordBasicInformationResponseModel>)await serviceRecordsService.GetRecentByVehicleIdAsync(vehicleId, expectedCount);

        //Assert
        Assert.AreEqual(expectedCount, result.Count);
        Assert.AreEqual(serviceRecords[0].Id.ToString(), result[0].Id);
        Assert.AreEqual(serviceRecords[0].Title, result[0].Title);
        Assert.AreEqual(serviceRecords[0].Vehicle.Make, result[0].VehicleMake);
        Assert.AreEqual(serviceRecords[0].Vehicle.Model, result[0].VehicleModel);
        Assert.AreEqual(serviceRecords[0].PerformedOn, result[0].PerformedOn);
        Assert.AreEqual(serviceRecords[1].Id.ToString(), result[1].Id);
        Assert.AreEqual(serviceRecords[1].Title, result[1].Title);
        Assert.AreEqual(serviceRecords[1].Vehicle.Make, result[1].VehicleMake);
        Assert.AreEqual(serviceRecords[1].Vehicle.Model, result[1].VehicleModel);
        Assert.AreEqual(serviceRecords[1].PerformedOn, result[1].PerformedOn);

    }

    /// <summary>
    /// Tests the retrieving of user service records as queryable
    /// </summary>
    /// <returns></returns>
    [Test]
    public async Task GetAllByUserIdForSearchAsync_RetrievesUserRecords_AsQueryable()
    {
        //Arrange
        ICollection<ServiceRecord> serviceRecords = await GenerateServiceRecords();

        //Act
        var result = await serviceRecordsService.GetAllByUserIdAsQueryableAsync(userId);

        //Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(result.Count(), serviceRecords.Count());
    }

    /// <summary>
    /// Tests the retrieving of user service records by page when serviceRecords contains records, currentPage and recordPerPage parameters are valid.
    /// </summary>
    /// Disclaimer:
    ///The service method "GetAllByUserIdAsQueryableAsync" is used to retrieve the IQueryable collection because the standard IQueryable interface does not implement IAsyncEnumerable which is required for asynchronous operations in Entity Framework Core.
    [Test]
    public async Task RetrieveServiceRecordsByPageAsync_RetrievesCorrectAmountOfRecords()
    {
        //Arrange
        ICollection<ServiceRecord> serviceRecords = await GenerateServiceRecords();

        var currentPage = 2;
        var recordPerPage = 3;
        var expectedRecords = serviceRecords.Skip((currentPage - 1) * recordPerPage).Take(recordPerPage).ToList();

        //Act
        var quaryableRecords = await serviceRecordsService.GetAllByUserIdAsQueryableAsync(userId);
        var result = await serviceRecordsService.RetrieveServiceRecordsByPageAsync(quaryableRecords, currentPage, recordPerPage);

        //Assert
        Assert.NotNull(result);
        Assert.AreEqual(expectedRecords.Count, result.Count);
        Assert.AreEqual(expectedRecords[0].Title, result[0].Title);
        Assert.AreEqual(expectedRecords[1].Title, result[1].Title);
        Assert.AreEqual(expectedRecords[2].Title, result[2].Title);
    }

    /// Tests the retrieving of user service records by page when there are no service records, currentPage and recordPerPage parameters are valid.
    [Test]
    public async Task RetrieveServiceRecordsByPageAsync_EmptyList_ReturnsEmptyList()
    {
        // Arrange
        var currentPage = 2;
        var recordPerPage = 3;

        // Act
        var quaryableRecords = await serviceRecordsService.GetAllByUserIdAsQueryableAsync(userId);
        var result = await serviceRecordsService.RetrieveServiceRecordsByPageAsync(quaryableRecords, currentPage, recordPerPage);

        // Assert
        Assert.NotNull(result);
        Assert.IsEmpty(result);
    }

    /// Tests the retrieving of user service records by page when there are service records but current page is out of bounds.
    [Test]
    public async Task RetrieveServiceRecordsByPageAsync_CurrentPageOutOfBounds_ReturnsEmptyList()
    {
        //Arrange
        await GenerateServiceRecords();

        var currentPage = 5;
        var recordPerPage = 3;


        // Act
        var quaryableRecords = await serviceRecordsService.GetAllByUserIdAsQueryableAsync(userId);
        var result = await serviceRecordsService.RetrieveServiceRecordsByPageAsync(quaryableRecords, currentPage, recordPerPage);

        // Assert
        Assert.NotNull(result);
        Assert.IsEmpty(result);

    }


    [TearDown]
    public void TearDown()
    {
        applicationDbContext.Dispose();
    }


    /// <summary>
    /// Generates service records in the database.
    /// </summary>
    /// <returns>A collection of service records</returns>
    private async Task<ICollection<ServiceRecord>> GenerateServiceRecords()
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

        List<ServiceRecord> serviceRecords = new List<ServiceRecord>()
        {
             new ServiceRecord
            {
               Id = Guid.NewGuid(),
               Title = "Test1",
               Description = "Test1",
               Cost = 150,
               Mileage = 1500,
               VehicleId = Guid.Parse(vehicleId),
               OwnerId = Guid.Parse(userId),
               PerformedOn = DateTime.UtcNow,
               CreatedOn = DateTime.UtcNow
            },
             new ServiceRecord
            {
               Id = Guid.NewGuid(),
               Title = "Test2",
               Description = "Test2",
               Cost = 1530,
               Mileage = 15005,
               VehicleId = Guid.Parse(vehicleId),
               OwnerId = Guid.Parse(userId),
               PerformedOn = DateTime.UtcNow.AddDays(-1),
               CreatedOn = DateTime.UtcNow.AddDays(-1)
            },
             new ServiceRecord
            {
               Id = Guid.NewGuid(),
               Title = "Test3",
               Description = "Test3",
               Cost = 1530,
               Mileage = 15005,
               VehicleId = Guid.Parse(vehicleId),
               OwnerId = Guid.Parse(userId),
               PerformedOn = DateTime.UtcNow.AddDays(-1),
               CreatedOn = DateTime.UtcNow.AddDays(-1)
            },
             new ServiceRecord
            {
               Id = Guid.NewGuid(),
               Title = "Test4",
               Description = "Test4",
               Cost = 1530,
               Mileage = 15005,
               VehicleId = Guid.Parse(vehicleId),
               OwnerId = Guid.Parse(userId),
               PerformedOn = DateTime.UtcNow.AddDays(-2),
               CreatedOn = DateTime.UtcNow.AddDays(-2)
            },
              new ServiceRecord
            {
               Id = Guid.NewGuid(),
               Title = "Test5",
               Description = "Test5",
               Cost = 150,
               Mileage = 1500,
               VehicleId = Guid.Parse(vehicleId),
               OwnerId = Guid.Parse(userId),
               PerformedOn = DateTime.UtcNow,
               CreatedOn = DateTime.UtcNow
            },
             new ServiceRecord
            {
               Id = Guid.NewGuid(),
               Title = "Test6",
               Description = "Test6",
               Cost = 1530,
               Mileage = 15005,
               VehicleId = Guid.Parse(vehicleId),
               OwnerId = Guid.Parse(userId),
               PerformedOn = DateTime.UtcNow.AddDays(-1),
               CreatedOn = DateTime.UtcNow.AddDays(-1)
            },
             new ServiceRecord
            {
               Id = Guid.NewGuid(),
               Title = "Test7",
               Description = "Test7",
               Cost = 1530,
               Mileage = 15005,
               VehicleId = Guid.Parse(vehicleId),
               OwnerId = Guid.Parse(userId),
               PerformedOn = DateTime.UtcNow.AddDays(-2),
               CreatedOn = DateTime.UtcNow.AddDays(-2)
            },
             new ServiceRecord
            {
               Id = Guid.NewGuid(),
               Title = "Test9",
               Description = "Test9",
               Cost = 1530,
               Mileage = 15005,
               VehicleId = Guid.Parse(vehicleId),
               OwnerId = Guid.Parse(userId),
               PerformedOn = DateTime.UtcNow.AddDays(-1),
               CreatedOn = DateTime.UtcNow.AddDays(-1)
            },
             new ServiceRecord
            {
               Id = Guid.NewGuid(),
               Title = "Test10",
               Description = "Test10",
               Cost = 1530,
               Mileage = 15005,
               VehicleId = Guid.Parse(vehicleId),
               OwnerId = Guid.Parse(userId),
               PerformedOn = DateTime.UtcNow.AddDays(-1),
               CreatedOn = DateTime.UtcNow.AddDays(-1)
            },

        };

        await repository.AddRangeAsync(serviceRecords);
        await repository.SaveChangesAsync();

        return serviceRecords;
    }
}
