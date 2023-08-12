namespace CarCare_Companion.Tests.Services;

using System;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

using Moq;

using CarCare_Companion.Core.Models.Trip;
using CarCare_Companion.Core.Models.TripRecords;
using CarCare_Companion.Core.Services;
using CarCare_Companion.Infrastructure.Data;
using CarCare_Companion.Infrastructure.Data.Common;
using CarCare_Companion.Infrastructure.Data.Models.Records;
using CarCare_Companion.Infrastructure.Data.Models.Vehicle;


[TestFixture]
public class TripRecordsServiceTests
{
    private IRepository repository;
    private TripRecordsService tripRecordsService;
    private Mock<IMemoryCache> mockMemoryCache;
    private CarCareCompanionDbContext applicationDbContext;

    [SetUp]
    public void Setup()
    {
        var contextOptions = new DbContextOptionsBuilder<CarCareCompanionDbContext>()
            .UseInMemoryDatabase("TripRecordsDb")
            .Options;
        applicationDbContext = new CarCareCompanionDbContext(contextOptions);

        this.mockMemoryCache = new Mock<IMemoryCache>();
        this.repository = new Repository(applicationDbContext);
        this.tripRecordsService = new TripRecordsService(repository, mockMemoryCache.Object);

        //Used for mocking the Set method
        var cacheEntry = Mock.Of<ICacheEntry>();
        mockMemoryCache
       .Setup(x => x.CreateEntry(It.IsAny<Object>()))
       .Returns(cacheEntry);

        applicationDbContext.Database.EnsureDeleted();
        applicationDbContext.Database.EnsureCreated();
    }


    /// <summary>
    /// Tests the trip record adding functionality with correct data
    /// </summary>
    [Test]
    public async Task CreateAsync_ShouldAddANewTrip()
    {
        //Arrange
        string userId = "0cda5780-8fa7-4e53-99b5-93c31c26f6ec";
        string vehicleId = "dc0a5780-8fa7-4e53-99b5-93c31c26f6ec";

        TripFormRequestModel tripToCreate = new TripFormRequestModel()
        {
            StartDestination = "Test",
            EndDestination = "Test2",
            MileageTravelled = 150,
            VehicleId = vehicleId,
            FuelPrice = 2.5m,
            UsedFuel = 25
        };

        //Act
        TripResponseModel createdTrip = await tripRecordsService.CreateAsync(userId, tripToCreate);

        //Assert
        Assert.IsNotNull(createdTrip);
        Assert.AreEqual(tripToCreate.StartDestination, createdTrip.StartDestination);
        Assert.AreEqual(tripToCreate.EndDestination, createdTrip.EndDestination);
        Assert.AreEqual(tripToCreate.MileageTravelled, createdTrip.MileageTravelled);
        Assert.AreEqual(tripToCreate.FuelPrice, createdTrip.FuelPrice);
        Assert.AreEqual(tripToCreate.UsedFuel, createdTrip.UsedFuel);

    }

    /// <summary>
    /// Tests the trip record editing functionality with correct data
    /// </summary>
    [Test]
    public async Task EditAsync_WhenCalled_EditsAVehicle()
    {
        //Arrange
        string tripId = "adc05780-8fa7-4e53-99b5-93c31c26f6ec";
        string userId = "0cda5780-8fa7-4e53-99b5-93c31c26f6ec";
        string vehicleId = "dc0a5780-8fa7-4e53-99b5-93c31c26f6ec";

        TripRecord trip = new TripRecord()
        {
            Id = Guid.Parse(tripId),
            StartDestination = "Test",
            EndDestination = "Test2",
            MileageTravelled = 150,
            VehicleId = Guid.Parse(vehicleId),
            OwnerId = Guid.Parse(userId),
            FuelPrice = 2.5m,
            UsedFuel = 25,
            CreatedOn = DateTime.UtcNow
        };

        await repository.AddAsync(trip);
        await repository.SaveChangesAsync();

        TripFormRequestModel tripToEdit= new TripFormRequestModel()
        {
            StartDestination = "Test3333",
            EndDestination = "Test233333",
            MileageTravelled = 1560,
            VehicleId = vehicleId,
            FuelPrice = 2.57m,
            UsedFuel = 25.5
        };

        //Act
        TripResponseModel editedTrip = await tripRecordsService.EditAsync(tripId, userId, tripToEdit);

        //Assert
        Assert.IsNotNull(editedTrip);
        Assert.AreEqual(trip.StartDestination, editedTrip.StartDestination);
        Assert.AreEqual(trip.EndDestination, editedTrip.EndDestination);
        Assert.AreEqual(trip.MileageTravelled, editedTrip.MileageTravelled);
        Assert.AreEqual(trip.FuelPrice, editedTrip.FuelPrice);
        Assert.AreEqual(trip.UsedFuel, editedTrip.UsedFuel);

    }

    /// <summary>
    /// Tests the trip record deleting 
    /// </summary>
    [Test]
    public async Task DeleteAsync_ShouldDeleteAEntity()
    {
        //Arrange
        string tripId = "adc05780-8fa7-4e53-99b5-93c31c26f6ec";
        string userId = "0cda5780-8fa7-4e53-99b5-93c31c26f6ec";
        string vehicleId = "dc0a5780-8fa7-4e53-99b5-93c31c26f6ec";

        TripRecord trip = new TripRecord()
        {
            Id = Guid.Parse(tripId),
            StartDestination = "Test",
            EndDestination = "Test2",
            MileageTravelled = 150,
            VehicleId = Guid.Parse(vehicleId),
            OwnerId = Guid.Parse(userId),
            FuelPrice = 2.5m,
            UsedFuel = 25,
            CreatedOn = DateTime.UtcNow
        };

        await repository.AddAsync(trip);
        await repository.SaveChangesAsync();

        //Act
        await tripRecordsService.DeleteAsync(tripId, userId);

        TripRecord deletedTripRecord = await repository.GetByIdAsync<TripRecord>(Guid.Parse(tripId));

        //Assert
        Assert.IsTrue(deletedTripRecord.IsDeleted);
    }

    /// <summary>
    /// Tests if a trip exist by id
    /// </summary>
    [Test]
    public async Task DoesTripExistByIdAsync_ShouldReturnTrue_WhenTripExists()
    {
        //Arrange
        string tripId = "adc05780-8fa7-4e53-99b5-93c31c26f6ec";
        string userId = "0cda5780-8fa7-4e53-99b5-93c31c26f6ec";
        string vehicleId = "dc0a5780-8fa7-4e53-99b5-93c31c26f6ec";

        TripRecord trip = new TripRecord()
        {
            Id = Guid.Parse(tripId),
            StartDestination = "Test",
            EndDestination = "Test2",
            MileageTravelled = 150,
            VehicleId = Guid.Parse(vehicleId),
            OwnerId = Guid.Parse(userId),
            FuelPrice = 2.5m,
            UsedFuel = 25,
            CreatedOn = DateTime.UtcNow
        };

        await repository.AddAsync(trip);
        await repository.SaveChangesAsync();

        //Act
        bool doesTripExist = await tripRecordsService.DoesTripExistByIdAsync(tripId);

        //Assert
        Assert.IsTrue(doesTripExist);
    }

    /// <summary>
    /// Tests if a trip exist by id
    /// </summary>
    [Test]
    public async Task DoesTripExistByIdAsync_ShouldReturnFalse_WhenTripDoesntExists()
    {
        //Arrange
        string tripId = "adc05780-8fa7-4e53-99b5-93c31c26f6ec";

        //Act
        bool doesTripExist = await tripRecordsService.DoesTripExistByIdAsync(tripId);

        //Assert
        Assert.IsFalse(doesTripExist);
    }

    /// <summary>
    /// Tests if retrieving all user trips from cache works
    /// </summary>
    [Test]
    public async Task GetAllTripsByUsedIdAsync_ReturnsDataFromCache_IfPresent()
    {
        //Arrange
        string userId = "0cda5780-8fa7-4e53-99b5-93c31c26f6ec";
        string vehicleId = "dc0a5780-8fa7-4e53-99b5-93c31c26f6ec";

        object testData = new List<TripDetailsByUserResponseModel>()
        {
            new TripDetailsByUserResponseModel() {
                Id = Guid.NewGuid().ToString(),
                VehicleMake = "BMW", 
                VehicleModel = "M5 CS", 
                StartDestination = "Test",
                EndDestination = "TestTest",
                MileageTravelled  = 53,
                DateCreated = DateTime.UtcNow,
                FuelPrice = 2.5m,
                UsedFuel = 25,
                TripCost = 100
            },
            new TripDetailsByUserResponseModel() {
                Id = Guid.NewGuid().ToString(),
                VehicleMake = "BMW",
                VehicleModel = "M3 CS",
                StartDestination = "TestDestination",
                EndDestination = "TestDestinationTest",
                MileageTravelled  = 153,
                DateCreated = DateTime.UtcNow,
                FuelPrice = 2.8m,
                UsedFuel = 35,
                TripCost = 200
            }
        };

        mockMemoryCache
        .Setup(x => x.TryGetValue(It.IsAny<object>(), out testData))
        .Returns(true);

        //Act
        var result = await tripRecordsService.GetAllTripsByUsedIdAsync(userId);

        //Assert
        Assert.AreEqual(testData, result);
    }


    /// <summary>
    /// Tests if retrieving all user trips from repository works
    /// </summary>
    [Test]
    public async Task GetAllTripsByUsedIdAsync_RetrievesFromRepo_WhenCacheIsEmpty()
    {
        //Arrange
        string userId = "0cda5780-8fa7-4e53-99b5-93c31c26f6ec";
        string vehicleId = "dc0a5780-8fa7-4e53-99b5-93c31c26f6ec";

        Vehicle vehicle = new Vehicle()
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

        TripRecord trip = new TripRecord()
        {
            StartDestination = "TestTest",
            EndDestination = "TestTest",
            MileageTravelled = 15,
            UsedFuel = 15,
            FuelPrice = 2.6m,
            CreatedOn = DateTime.UtcNow,
            VehicleId = Guid.Parse(vehicleId),
            OwnerId = Guid.Parse(userId),
            Cost = 100
        };

        await repository.AddAsync(trip);
        await repository.SaveChangesAsync();

        //Act
        var result =(List<TripDetailsByUserResponseModel>)await tripRecordsService.GetAllTripsByUsedIdAsync(userId);

        //Assert
        Assert.AreEqual(trip.StartDestination, result[0].StartDestination);
        Assert.AreEqual(trip.EndDestination, result[0].EndDestination);
        Assert.AreEqual(trip.MileageTravelled, result[0].MileageTravelled);
        Assert.AreEqual(trip.UsedFuel, result[0].UsedFuel);
        Assert.AreEqual(trip.FuelPrice, result[0].FuelPrice);
        Assert.AreEqual(trip.Cost, result[0].TripCost);
    }

    /// <summary>
    /// Tests if user is a trip record creator
    /// </summary>
    [Test]
    public async Task IsUserCreatorOfTripAsync_ShouldReturnTrue_WhenUserIsCreator()
    {
        //Arrange
        string tripId = "adc05780-8fa7-4e53-99b5-93c31c26f6ec";
        string userId = "0cda5780-8fa7-4e53-99b5-93c31c26f6ec";
        string vehicleId = "dc0a5780-8fa7-4e53-99b5-93c31c26f6ec";

        TripRecord trip = new TripRecord()
        {
            Id = Guid.Parse(tripId),
            StartDestination = "Test",
            EndDestination = "Test2",
            MileageTravelled = 150,
            VehicleId = Guid.Parse(vehicleId),
            OwnerId = Guid.Parse(userId),
            FuelPrice = 2.5m,
            UsedFuel = 25,
            CreatedOn = DateTime.UtcNow
        };

        await repository.AddAsync(trip);
        await repository.SaveChangesAsync();

        //Act
        bool isUserTripCrator = await tripRecordsService.IsUserCreatorOfTripAsync(userId,tripId);

        //Assert
        Assert.IsTrue(isUserTripCrator);
    }

    /// <summary>
    /// Tests if user is a trip record creator
    /// </summary>
    [Test]
    public async Task IsUserCreatorOfTripAsync_ShouldReturnFalse_WhenUserIsNotCreator()
    {
        //Arrange
        string tripId = "adc05780-8fa7-4e53-99b5-93c31c26f6ec";
        string userId = "0cda5780-8fa7-4e53-99b5-93c31c26f6ec";
        string vehicleId = "dc0a5780-8fa7-4e53-99b5-93c31c26f6ec";
        string otherUserId = "aaaa5780-8fa7-4e53-99b5-93c31c26f6ec";

        TripRecord trip = new TripRecord()
        {
            Id = Guid.Parse(tripId),
            StartDestination = "Test",
            EndDestination = "Test2",
            MileageTravelled = 150,
            VehicleId = Guid.Parse(vehicleId),
            OwnerId = Guid.Parse(userId),
            FuelPrice = 2.5m,
            UsedFuel = 25,
            CreatedOn = DateTime.UtcNow
        };

        await repository.AddAsync(trip);
        await repository.SaveChangesAsync();

        //Act
        bool isUserTripCrator = await tripRecordsService.IsUserCreatorOfTripAsync(otherUserId, tripId);

        //Assert
        Assert.IsFalse(isUserTripCrator);
    }

    /// <summary>
    /// Tests retrieving of details for a trip record
    /// </summary>
    [Test]
    public async Task GetTripDetailsByIdAsync_ShouldRetrieveTripDetails()
    {
        //Arrange
        string tripId = "adc05780-8fa7-4e53-99b5-93c31c26f6ec";
        string userId = "0cda5780-8fa7-4e53-99b5-93c31c26f6ec";
        string vehicleId = "dc0a5780-8fa7-4e53-99b5-93c31c26f6ec";


        Vehicle vehicle = new Vehicle()
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

        TripRecord trip = new TripRecord()
        {
            Id = Guid.Parse(tripId),
            StartDestination = "Test",
            EndDestination = "Test2",
            MileageTravelled = 150,
            VehicleId = Guid.Parse(vehicleId),
            OwnerId = Guid.Parse(userId),
            FuelPrice = 2.5m,
            UsedFuel = 25,
            CreatedOn = DateTime.UtcNow
        };

        await repository.AddAsync(trip);
        await repository.SaveChangesAsync();

        //Act
        TripEditDetailsResponseModel  result = await tripRecordsService.GetTripDetailsByIdAsync(tripId);

        //Assert
        Assert.AreEqual(trip.Id.ToString(), result.Id);
        Assert.AreEqual(trip.StartDestination, result.StartDestination);
        Assert.AreEqual(trip.EndDestination, result.EndDestination);
        Assert.AreEqual(trip.MileageTravelled, result.MileageTravelled);
        Assert.AreEqual(trip.UsedFuel, result.UsedFuel);
        Assert.AreEqual(trip.FuelPrice, result.FuelPrice);
        Assert.AreEqual(trip.Vehicle.Id.ToString(), result.Vehicle);
     
    }

    /// <summary>
    /// Tests retrieving of trip records count from cache
    /// </summary>
    [Test]
    public async Task GetAllUserTripsCountAsync_ReturnsDataFromCache_IfPresent()
    {
        //Arrange
        string userId = "0cda5780-8fa7-4e53-99b5-93c31c26f6ec";
        int expectedCount = 5;

        object testData = 5;

        mockMemoryCache
        .Setup(x => x.TryGetValue(It.IsAny<object>(), out testData))
        .Returns(true);

        //Act 
        int result = await tripRecordsService.GetAllUserTripsCountAsync(userId);

        //Assert
        Assert.AreEqual(expectedCount, result);
    }

    /// <summary>
    /// Tests retrieving of trip records count from repository
    /// </summary>
    [Test]
    public async Task GetAllUserTripsCountAsync_RetrievesFromRepo_WhenCacheIsEmpty()
    {
        //Arrange
        string userId = "0cda5780-8fa7-4e53-99b5-93c31c26f6ec";
        string vehicleId = "dc0a5780-8fa7-4e53-99b5-93c31c26f6ec";

        ICollection<TripRecord> tripRecords = new List<TripRecord>()
        {
             new TripRecord()
        {
            StartDestination = "Test",
            EndDestination = "Test2",
            MileageTravelled = 150,
            VehicleId = Guid.Parse(vehicleId),
            OwnerId = Guid.Parse(userId),
            FuelPrice = 2.5m,
            UsedFuel = 25,
            CreatedOn = DateTime.UtcNow,
            Cost = 500
        },
             new TripRecord()
        {
            StartDestination = "Test333",
            EndDestination = "Test2555",
            MileageTravelled = 1550,
            VehicleId = Guid.Parse(vehicleId),
            OwnerId = Guid.Parse(userId),
            FuelPrice = 2.53m,
            UsedFuel = 25,
            CreatedOn = DateTime.UtcNow,
            Cost = 500
        }
        };

        await repository.AddRangeAsync(tripRecords);
        await repository.SaveChangesAsync();

        //Act
        decimal? result = await tripRecordsService.GetAllUserTripsCountAsync(userId);

        //Assert
        Assert.AreEqual(tripRecords.Count, result);
    }

    /// <summary>
    /// Tests retrieving of trip records cost from cache
    /// </summary>
    [Test]
    public async Task GetAllUserTripsCostAsync_ReturnsDataFromCache_IfPresent()
    {
        //Arrange
        string userId = "0cda5780-8fa7-4e53-99b5-93c31c26f6ec";
        decimal? expectedCost = 500;

        object testData = 500m;

        mockMemoryCache
        .Setup(x => x.TryGetValue(It.IsAny<object>(), out testData))
        .Returns(true);

        //Act 
        decimal? result = await tripRecordsService.GetAllUserTripsCostAsync(userId);

        //Assert
        Assert.AreEqual(expectedCost, result);
    }

    /// <summary>
    /// Tests retrieving of trip records cost from repository
    /// </summary>
    [Test]
    public async Task GetAllUserTripsCostAsync_RetrievesFromRepo_WhenCacheIsEmpty()
    {
        //Arrange
        string userId = "0cda5780-8fa7-4e53-99b5-93c31c26f6ec";
        string vehicleId = "dc0a5780-8fa7-4e53-99b5-93c31c26f6ec";

        ICollection<TripRecord> tripRecords = new List<TripRecord>()
        {
             new TripRecord()
        {
            StartDestination = "Test",
            EndDestination = "Test2",
            MileageTravelled = 150,
            VehicleId = Guid.Parse(vehicleId),
            OwnerId = Guid.Parse(userId),
            FuelPrice = 2.5m,
            UsedFuel = 25,
            CreatedOn = DateTime.UtcNow,
            Cost = 500
        },
             new TripRecord()
        {
            StartDestination = "Test333",
            EndDestination = "Test2555",
            MileageTravelled = 1550,
            VehicleId = Guid.Parse(vehicleId),
            OwnerId = Guid.Parse(userId),
            FuelPrice = 2.53m,
            UsedFuel = 25,
            CreatedOn = DateTime.UtcNow,
            Cost = 500
        }
        };

        await repository.AddRangeAsync(tripRecords);
        await repository.SaveChangesAsync();

        //Act
        decimal? result = await tripRecordsService.GetAllUserTripsCostAsync(userId);

        //Assert
        Assert.AreEqual(tripRecords.Sum(x => x.Cost), result);
    }

    /// <summary>
    /// Tests retrieving of latest N count records works
    /// </summary>
    [Test]
    public async Task GetLastNCountAsync_ShouldRetrieveNRecords()
    {
        //Arrange
        string userId = "0cda5780-8fa7-4e53-99b5-93c31c26f6ec";
        string vehicleId = "dc0a5780-8fa7-4e53-99b5-93c31c26f6ec";

        Vehicle vehicle = new Vehicle()
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

        List<TripRecord> tripRecords = new List<TripRecord>()
        {
             new TripRecord()
        {
            Id = Guid.NewGuid(),
            StartDestination = "Test",
            EndDestination = "Test2",
            MileageTravelled = 150,
            VehicleId = Guid.Parse(vehicleId),
            OwnerId = Guid.Parse(userId),
            FuelPrice = 2.5m,
            UsedFuel = 25,
            CreatedOn = DateTime.UtcNow.AddMinutes(5),
            Cost = 500
        },
             new TripRecord()
        {
            StartDestination = "Test333",
            EndDestination = "Test2555",
            MileageTravelled = 1550,
            VehicleId = Guid.Parse(vehicleId),
            OwnerId = Guid.Parse(userId),
            FuelPrice = 2.53m,
            UsedFuel = 25,
            CreatedOn = DateTime.UtcNow.AddMinutes(3),
            Cost = 500
        },
             new TripRecord()
        {
            StartDestination = "Test333",
            EndDestination = "Test2555",
            MileageTravelled = 1550,
            VehicleId = Guid.Parse(vehicleId),
            OwnerId = Guid.Parse(userId),
            FuelPrice = 2.53m,
            UsedFuel = 25,
            CreatedOn = DateTime.UtcNow,
            Cost = 500
        }
        };

        await repository.AddRangeAsync(tripRecords);
        await repository.SaveChangesAsync();

        //Act
        List<TripBasicInformationByUserResponseModel> result =(List<TripBasicInformationByUserResponseModel>) await tripRecordsService.GetLastNCountAsync(userId, 2);

        //Assert
        Assert.AreEqual(tripRecords[0].Id.ToString(), result[0].Id);
        Assert.AreEqual(tripRecords[1].Id.ToString(), result[1].Id);


    }

    [TearDown]
    public void TearDown()
    {
        applicationDbContext.Dispose();
    }
}
