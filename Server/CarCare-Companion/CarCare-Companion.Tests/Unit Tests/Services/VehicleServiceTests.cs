namespace CarCare_Companion.Tests.Unit_Tests.Services;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

using Moq;

using CarCare_Companion.Core.Contracts;
using CarCare_Companion.Core.Models.Vehicle;
using CarCare_Companion.Core.Services;
using CarCare_Companion.Infrastructure.Data;
using CarCare_Companion.Infrastructure.Data.Common;
using CarCare_Companion.Infrastructure.Data.Models.Vehicle;
using CarCare_Companion.Infrastructure.Data.Models.Records;


[TestFixture]
public class VehicleServiceTests
{
    private IRepository repository;
    private IVehicleService vehicleService;

    private Mock<IImageService> mockImageService;
    private Mock<IMemoryCache> mockMemoryCache;

    private CarCareCompanionDbContext applicationDbContext;

    [SetUp]
    public void Setup()
    {
        var contextOptions = new DbContextOptionsBuilder<CarCareCompanionDbContext>()
            .UseInMemoryDatabase("CarCareDB")
            .Options;

        applicationDbContext = new CarCareCompanionDbContext(contextOptions);

        this.mockMemoryCache = new Mock<IMemoryCache>();
        this.mockImageService = new Mock<IImageService>();

        this.repository = new Repository(applicationDbContext);
        this.vehicleService = new VehicleService(repository, mockImageService.Object, mockMemoryCache.Object);

        //Used for mocking the Set method
        var cacheEntry = Mock.Of<ICacheEntry>();
        mockMemoryCache
       .Setup(x => x.CreateEntry(It.IsAny<Object>()))
       .Returns(cacheEntry);

        applicationDbContext.Database.EnsureDeleted();
        applicationDbContext.Database.EnsureCreated();
    }

    /// <summary>
    /// Tests the vehicle adding functionality with correct data
    /// </summary>
    [Test]
    public async Task CreateAsync_ShouldAddANewVehicle()
    {
        //Arrange
        string OwnerId = "0cda5780-8fa7-4e53-99b5-93c31c26f6ec";

        VehicleFormRequestModel vehicleToAdd = new VehicleFormRequestModel()
        {
            Make = "BMW",
            Model = "M5 CS",
            Year = 2022,
            FuelTypeId = 1,
            VehicleTypeId = 1,
            Mileage = 12000,

        };

        //Act
        VehicleResponseModel createdVehicle = await vehicleService.CreateAsync(OwnerId, vehicleToAdd);

        //Assert
        Assert.IsNotNull(createdVehicle);
        Assert.AreEqual(vehicleToAdd.Make, createdVehicle.Make);
        Assert.AreEqual(vehicleToAdd.Model, createdVehicle.Model);
        Assert.AreEqual(vehicleToAdd.Year, createdVehicle.Year);
        Assert.AreEqual(vehicleToAdd.FuelTypeId, createdVehicle.FuelTypeId);
        Assert.AreEqual(vehicleToAdd.VehicleTypeId, createdVehicle.VehicleTypeId);

    }

    /// <summary>
    /// Tests the image adding to vehicle functionality with an existing vehicle
    /// </summary>
    [Test]
    public async Task AddImageToVehicle_WithExistingVehicle_ReturnsTrue()
    {
        //Arrange
        string imageId = "123a5780-8fa7-4e53-99b5-93c31c26f6ec";
        string userId = "0cda5780-8fa7-4e53-99b5-93c31c26f6ec";
        string vehicleId = "123a5780-8fa7-4e53-99b5-93c31c26f6ec";

        Vehicle vehicleToAdd = new Vehicle()
        {
            Id = Guid.Parse(vehicleId),
            Make = "BMW",
            Model = "M5 CS",
            Year = 2022,
            FuelTypeId = 1,
            VehicleTypeId = 1,
            Mileage = 12000,
            OwnerId = Guid.Parse(userId),
            CreatedOn = DateTime.UtcNow
        };

        //Act
        await repository.AddAsync(vehicleToAdd);
        bool isAddedSuccessful = await vehicleService.AddImageToVehicle(vehicleId, userId , imageId);

        //Assert
        Assert.IsTrue(isAddedSuccessful);

    }


    /// <summary>
    /// Tests the image adding to vehicle functionality with a non existing vehicle
    /// </summary>
    [Test]
    public async Task AddImageToVehicle_WithNonExistingVehicle_ReturnsFalse()
    {
        //Arrange
        string imageId = "123a5780-8fa7-4e53-99b5-93c31c26f6ec";
        string userId = "0cda5780-8fa7-4e53-99b5-93c31c26f6ec";
        string vehicleId = "333a5780-8fa7-4e53-99b5-93c31c26f6ec";

        //Act
        bool isAddedSuccessful = await vehicleService.AddImageToVehicle(vehicleId, userId, imageId);

        //Assert
        Assert.IsFalse(isAddedSuccessful);

    }

    /// <summary>
    /// Test if the edit method works correctly
    /// </summary>
    [Test]
    public async Task EditAsync_WhenCalled_EditsAVehicle()
    {
        //Arrange
        string userId = "0cda5780-8fa7-4e53-99b5-93c31c26f6ec";
        string vehicleId = "123a5780-8fa7-4e53-99b5-93c31c26f6ec";

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
            CreatedOn = DateTime.UtcNow
        };

        VehicleFormRequestModel vehicleToEdit = new VehicleFormRequestModel()
        {
            Make = "BMW",
            Model = "M3 CS",
            Year = 2023,
            FuelTypeId = 1,
            VehicleTypeId = 1,
            Mileage = 125000,
        };

        //Act
        await repository.AddAsync(vehicle);
        await repository.SaveChangesAsync();

        VehicleResponseModel editedVehicle = await vehicleService.EditAsync(vehicleId, userId, vehicleToEdit);

        //Assert
        Assert.AreEqual(vehicle.Make, editedVehicle.Make);
        Assert.AreEqual(vehicle.Model, editedVehicle.Model);
        Assert.AreEqual(vehicle.Year, editedVehicle.Year);
        Assert.AreEqual(vehicle.FuelTypeId, editedVehicle.FuelTypeId);
        Assert.AreEqual(vehicle.VehicleTypeId, editedVehicle.VehicleTypeId);
        Assert.AreEqual(vehicle.Mileage, editedVehicle.Mileage);
    }

    /// <summary>
    /// Tests if a fuel type exists
    /// </summary>
    [Test]
    public async Task DoesFuelTypeExistAsync_ShouldReturnTrue_WhenFuelTypeExists()
    {
        //Assign
        int fuelTypeId = 10;

        FuelType fuelType = new FuelType()
        {
            Id = fuelTypeId,
            Name = "Test",
        };

        //Act
        await repository.AddAsync<FuelType>(fuelType);
        await repository.SaveChangesAsync();

        bool doesFuelTypeExist = await vehicleService.DoesFuelTypeExistAsync(fuelTypeId);

        //Assert
        Assert.IsTrue(doesFuelTypeExist);
    }

    /// <summary>
    /// Tests if a fuel type exists
    /// </summary>
    [Test]
    public async Task DoesFuelTypeExistAsync_ShouldReturnFalse_WhenFuelTypeDoesNotExists()
    {
        //Assign
        int fuelTypeId = 10;
        int nonExistingFuelTypeId = 160;

        FuelType fuelType = new FuelType()
        {
            Id = fuelTypeId,
            Name = "Test",
        };

        //Act
        await repository.AddAsync<FuelType>(fuelType);
        await repository.SaveChangesAsync();

        bool doesFuelTypeExist = await vehicleService.DoesFuelTypeExistAsync(nonExistingFuelTypeId);

        //Assert
        Assert.IsFalse(doesFuelTypeExist);
    }

    /// <summary>
    /// Tests if a vehicle type exists
    /// </summary>
    [Test]
    public async Task DoesVehicleTypeExistAsync_ShouldReturnTrue_WhenVehicleTypeExists()
    {
        //Assign
        int vehicleTypeId = 150;

        VehicleType vehicleType = new VehicleType()
        {
            Id = vehicleTypeId,
            Name = "Test",
        };

        //Act
        await repository.AddAsync<VehicleType>(vehicleType);
        await repository.SaveChangesAsync();

        bool doesVehicleTypeExist = await vehicleService.DoesVehicleTypeExistAsync(vehicleTypeId);

        //Assert
        Assert.IsTrue(doesVehicleTypeExist);
    }

    /// <summary>
    /// Tests if a vehicle type exists
    /// </summary>
    [Test]
    public async Task DoesVehicleTypeExistAsync_ShouldReturnFalse_WhenVehicleTypeDoesNotExists()
    {
        //Assign
        int vehicleTypeId = 150;
        int nonExistingvehicleTypeId = 160;

        VehicleType vehicleType = new VehicleType()
        {
            Id = vehicleTypeId,
            Name = "Test",
        };

        //Act
        await repository.AddAsync<VehicleType>(vehicleType);
        await repository.SaveChangesAsync();

        bool doesVehicleTypeExist = await vehicleService.DoesVehicleTypeExistAsync(nonExistingvehicleTypeId);

        //Assert
        Assert.IsFalse(doesVehicleTypeExist);
    }

    /// <summary>
    /// Tests retrieving all fuel types from cache
    /// </summary>
    [Test]
    public async Task AllFuelTypesAsync_ReturnsDataFromCache_IfPresent()
    {
        //Assign
        object testData = new List<FuelTypeResponseModel>
        {
            new FuelTypeResponseModel { Id = 1, Name = "Test",},
            new FuelTypeResponseModel { Id = 2, Name = "Test2",}
        };

        mockMemoryCache
       .Setup(x => x.TryGetValue(It.IsAny<object>(), out testData))
       .Returns(true);

        //Act
        var result = await vehicleService.AllFuelTypesAsync();

        //Assert
        Assert.AreEqual(testData, result);
    }

    /// <summary>
    /// Tests retrieving all fuel types from repository
    /// </summary>
    [Test]
    public async Task AllFuelTypesAsync_RetrievesFromRepo_WhenCacheIsEmpty()
    {
        //Assign
        int expectedCount = 5;
        //Act
        // Fuel types are seeded on database creation and they are always 5
        ICollection<FuelTypeResponseModel> result = await vehicleService.AllFuelTypesAsync();

        //Assert
        Assert.AreEqual(expectedCount, result.Count());

    }

    /// <summary>
    /// Tests retrieving all vehicle types from cache
    /// </summary>
    [Test]
    public async Task AllVehicleTypesAsync_ReturnsDataFromCache_IfPresent()
    {
        //Assign
        object testData = new List<VehicleTypeResponseModel>
        {
            new VehicleTypeResponseModel { Id = 1, Name = "Test",},
            new VehicleTypeResponseModel { Id = 2, Name = "Test2",}
        };

        mockMemoryCache
       .Setup(x => x.TryGetValue(It.IsAny<object>(), out testData))
       .Returns(true);

        //Act
        var result = await vehicleService.AllVehicleTypesAsync();

        //Assert
        Assert.AreEqual(testData, result);
    }

    /// <summary>
    /// Tests retrieving all vehicle types from repository
    /// </summary>
    [Test]
    public async Task AllVehicleTypesAsync_RetrievesFromRepo_WhenCacheIsEmpty()
    {
        //Assign
        int expectedCount = 10;
        //Act
        // Vehicle types are seeded on database creation and they are always 10
        ICollection<VehicleTypeResponseModel> result = await vehicleService.AllVehicleTypesAsync();

        //Assert
        Assert.AreEqual(expectedCount, result.Count());

    }

    /// <summary>
    /// Tests retrieving all user vehicles from cache
    /// </summary>
    [Test]
    public async Task AllUserVehiclesByIdAsync_ReturnsDataFromCache_IfPresent()
    {
        //Assign
        string userId = "0cda5780-8fa7-4e53-99b5-93c31c26f6ec";

        object testData = new List<VehicleBasicInfoResponseModel>
        {
            new VehicleBasicInfoResponseModel{ Id = Guid.NewGuid().ToString(), ImageUrl = Guid.NewGuid().ToString(), Make = "BMW", Model = "M5"  },
            new VehicleBasicInfoResponseModel{ Id = Guid.NewGuid().ToString(), ImageUrl = Guid.NewGuid().ToString(), Make = "BMW", Model = "M3"  },
        };

        mockMemoryCache
        .Setup(x => x.TryGetValue(It.IsAny<object>(), out testData))
        .Returns(true);

        //Act
        var result = await vehicleService.AllUserVehiclesByIdAsync(userId);

        //Assert
        Assert.AreEqual(testData,result);
    }

    /// <summary>
    /// Tests retrieving all user vehicles from repository
    /// </summary>
    [Test]
    public async Task AllUserVehiclesByIdAsync_RetrievesFromRepo_WhenCacheIsEmpty()
    {
        //Arrange
        string userId = "0cda5780-8fa7-4e53-99b5-93c31c26f6ec";
        string vehicleId = "123a5780-8fa7-4e53-99b5-93c31c26f6ec";
        string imageId = "a5555780-8fa7-4e53-99b5-93c31c26f6ec";

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
            VehicleImageKey = Guid.Parse(imageId)
        };

        await repository.AddAsync(vehicle);
        await repository.SaveChangesAsync();

        mockImageService
        .Setup(x => x.GetImageUrlAsync(It.IsAny<string>()))
        .Returns(Task<string>.FromResult(imageId));

        //Act
        var result =(List<VehicleBasicInfoResponseModel>)await vehicleService.AllUserVehiclesByIdAsync(userId);

        //Assert
        Assert.AreEqual(vehicle.Make, result[0].Make);
        Assert.AreEqual(vehicle.Model, result[0].Model);
        Assert.AreEqual(vehicle.Id.ToString(), result[0].Id);
        Assert.AreEqual(vehicle.VehicleImageKey.ToString(), result[0].ImageUrl);
    }

    /// <summary>
    /// Tests retrieving vehicles details from cache
    /// </summary>
    [Test]
    public async Task GetVehicleDetailsByIdAsync_ReturnsDataFromCache_IfPresent()
    {
        //Assign
        string vehicleId = "0cda5780-8fa7-4e53-99b5-93c31c26f6ec";
        string imageUrl = "a5555780-8fa7-4e53-99b5-93c31c26f6ec";

        object testData = new VehicleDetailsResponseModel
        {
            Id = vehicleId,
            Make = "BMW",
            Model = "M5 CS",
            FuelType = "Petrol",
            VehicleType = "Sedan",
            Mileage = 25000,
            Year = 2023,
            ImageUrl = imageUrl
        };

        mockImageService
       .Setup(x => x.GetImageUrlAsync(It.IsAny<string>()))
       .Returns(Task<string>.FromResult(imageUrl));

        mockMemoryCache
       .Setup(x => x.TryGetValue(It.IsAny<object>(), out testData))
       .Returns(true);

        //Act
        var result = await vehicleService.GetVehicleDetailsByIdAsync(vehicleId);

        //Assert
        Assert.AreEqual(testData, result);

    }

    /// <summary>
    /// Tests retrieving vehicles details from repository
    /// </summary>
    [Test]
    public async Task GetVehicleDetailsByIdAsync_RetrievesFromRepo_WhenCacheIsEmpty()
    {
        //Arrange
        string userId = "0cda5780-8fa7-4e53-99b5-93c31c26f6ec";
        string vehicleId = "123a5780-8fa7-4e53-99b5-93c31c26f6ec";
        string imageId = "a5555780-8fa7-4e53-99b5-93c31c26f6ec";

        string fuelType = "Petrol";
        string vehicleType = "Sedan";

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
            VehicleImageKey = Guid.Parse(imageId)
        };

        await repository.AddAsync(vehicle);
        await repository.SaveChangesAsync();

        mockImageService
        .Setup(x => x.GetImageUrlAsync(It.IsAny<string>()))
        .Returns(Task<string>.FromResult(imageId));

        //Act
        var result = await vehicleService.GetVehicleDetailsByIdAsync(vehicleId);

        //Assert
        Assert.AreEqual(vehicle.Id.ToString(), result.Id);
        Assert.AreEqual(vehicle.Make, result.Make);
        Assert.AreEqual(vehicle.Model, result.Model);
        Assert.AreEqual(vehicle.Mileage, result.Mileage);
        Assert.AreEqual(fuelType, result.FuelType);
        Assert.AreEqual(vehicleType, result.VehicleType);
        Assert.AreEqual(vehicle.Year, result.Year);


    }

    /// <summary>
    /// Tests retrieving vehicles edit details from cache
    /// </summary>
    [Test]
    public async Task GetVehicleEditDetailsAsync_WhenCalled_RetrievesDetailsAboutAVehicle()
    {
        //Arrange
        string userId = "0cda5780-8fa7-4e53-99b5-93c31c26f6ec";
        string vehicleId = "123a5780-8fa7-4e53-99b5-93c31c26f6ec";
        string imageId = "a5555780-8fa7-4e53-99b5-93c31c26f6ec";

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
            VehicleImageKey = Guid.Parse(imageId)
        };

        await repository.AddAsync(vehicle);
        await repository.SaveChangesAsync();

        //Act
        var result = await vehicleService.GetVehicleEditDetailsAsync(vehicleId);

        //Assert
        Assert.AreEqual(vehicle.Id.ToString(), result.Id);
        Assert.AreEqual(vehicle.Make, result.Make);
        Assert.AreEqual(vehicle.Model, result.Model);
        Assert.AreEqual(vehicle.Mileage, result.Mileage);
        Assert.AreEqual(vehicle.FuelTypeId, result.FuelTypeId);
        Assert.AreEqual(vehicle.VehicleTypeId, result.VehicleTypeId);
        Assert.AreEqual(vehicle.Year, result.Year);
    }

    /// <summary>
    /// Tests if a vehicle exists by id
    /// </summary>
    [Test]
    public async Task DoesVehicleExistByIdAsync_ShouldReturnTrue_WhenVehicleExists()
    {
        //Arrange
        string userId = "0cda5780-8fa7-4e53-99b5-93c31c26f6ec";
        string vehicleId = "123a5780-8fa7-4e53-99b5-93c31c26f6ec";
        string imageId = "a5555780-8fa7-4e53-99b5-93c31c26f6ec";

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
            VehicleImageKey = Guid.Parse(imageId)
        };

        await repository.AddAsync(vehicle);
        await repository.SaveChangesAsync();

        //Act
        bool doesVehicleExist = await vehicleService.DoesVehicleExistByIdAsync(vehicleId);

        //Assert
        Assert.IsTrue(doesVehicleExist);
    }

    /// <summary>
    /// Tests if a vehicle exists by id
    /// </summary>
    [Test]
    public async Task DoesVehicleExistByIdAsync_ShouldReturnFalse_WhenVehicleDoesNotExists()
    {
        //Arrange
        string vehicleId = "123a5780-8fa7-4e53-99b5-93c31c26f6ec";  

        //Act
        bool doesVehicleExist = await vehicleService.DoesVehicleExistByIdAsync(vehicleId);

        //Assert
        Assert.IsFalse(doesVehicleExist);
    }

    /// <summary>
    /// Tests if a user is vehicle owner
    /// </summary>
    [Test]
    public async Task IsUserOwnerOfVehicleAsync_ShouldReturnTrue_WhenUserIsVehicleOwner()
    {
        //Arrange
        string userId = "0cda5780-8fa7-4e53-99b5-93c31c26f6ec";
        string vehicleId = "123a5780-8fa7-4e53-99b5-93c31c26f6ec";
        string imageId = "a5555780-8fa7-4e53-99b5-93c31c26f6ec";

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
            VehicleImageKey = Guid.Parse(imageId)
        };

        await repository.AddAsync(vehicle);
        await repository.SaveChangesAsync();

        //Act
        bool isUserVehicleOwner = await vehicleService.IsUserOwnerOfVehicleAsync(userId,vehicleId);

        //Assert
        Assert.IsTrue(isUserVehicleOwner);
    }

    /// <summary>
    /// Tests if a user is vehicle owner
    /// </summary>
    [Test]
    public async Task IsUserOwnerOfVehicleAsync_ShouldReturnFalse_WhenUserIsNotVehicleOwner()
    {
        //Arrange
        string userId = "0cda5780-8fa7-4e53-99b5-93c31c26f6ec";
        string differentUserId = "765a5780-8fa7-4e53-99b5-93c31c26f6ec";
        string vehicleId = "123a5780-8fa7-4e53-99b5-93c31c26f6ec";
        string imageId = "a5555780-8fa7-4e53-99b5-93c31c26f6ec";

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
            VehicleImageKey = Guid.Parse(imageId)
        };

        await repository.AddAsync(vehicle);
        await repository.SaveChangesAsync();

        //Act
        bool isUserVehicleOwner = await vehicleService.IsUserOwnerOfVehicleAsync(differentUserId, vehicleId);

        //Assert
        Assert.IsFalse(isUserVehicleOwner);
    }


    [Test]
    public async Task DeleteAsync_ShouldDeleteAEntity()
    {
        //Arrange
        Guid userId = Guid.Parse("0cda5780-8fa7-4e53-99b5-93c31c26f6ec");
        Guid vehicleId = Guid.Parse("123a5780-8fa7-4e53-99b5-93c31c26f6ec");
        Guid imageId = Guid.Parse("a5555780-8fa7-4e53-99b5-93c31c26f6ec");

        Guid tripRecordId = Guid.Parse("76555780-8fa7-4e53-99b5-93c31c26f6ec");
        Guid taxRecordId = Guid.Parse("8fa55780-8fa7-4e53-99b5-93c31c26f6ec");
        Guid serviceRecordId = Guid.Parse("fa555780-8fa7-4e53-99b5-93c31c26f6ec");

        Vehicle vehicle = new Vehicle()
        {
            Id = vehicleId,
            Make = "BMW",
            Model = "M5 CS",
            Year = 2022,
            FuelTypeId = 1,
            VehicleTypeId = 1,
            Mileage = 12000,
            OwnerId = userId,
            CreatedOn = DateTime.UtcNow,
            VehicleImageKey = imageId
        };

        await repository.AddAsync(vehicle);


        TripRecord tripRecord = new TripRecord()
        {
            Id = tripRecordId,
            StartDestination = "Test",
            EndDestination = "TestTest",
            VehicleId = vehicleId,
            MileageTravelled = 1,
            OwnerId = userId
        };

        await repository.AddAsync(tripRecord);

        ServiceRecord serviceRecord = new ServiceRecord()
        {
            Id = serviceRecordId,
            Title = "Testt",
            PerformedOn = DateTime.UtcNow,
            Mileage = 15,
            Cost = 157,
            VehicleId = vehicleId
        };
        await repository.AddAsync(serviceRecord);

        TaxRecord taxRecord = new TaxRecord()
        {
            Id = taxRecordId,
            Title = "Test",
            ValidFrom = DateTime.UtcNow,
            ValidTo = DateTime.UtcNow,
            Cost = 15,
            VehicleId = vehicleId
        };

        await repository.AddAsync(taxRecord);
        await repository.SaveChangesAsync();

        //Act
        await vehicleService.DeleteAsync(vehicleId.ToString(), userId.ToString());

        Vehicle deletedVehicleRecord = await repository.GetByIdAsync<Vehicle>(vehicleId);
        ServiceRecord deletedServiceRecord = await repository.GetByIdAsync<ServiceRecord>(serviceRecordId);
        TripRecord deletedTripRecord = await repository.GetByIdAsync<TripRecord>(tripRecordId);
        TaxRecord deletedTaxRecord = await repository.GetByIdAsync<TaxRecord>(taxRecordId);

        //Assert
        Assert.IsTrue(deletedVehicleRecord.IsDeleted);
        Assert.IsTrue(deletedServiceRecord.IsDeleted);
        Assert.IsTrue(deletedTripRecord.IsDeleted);
        Assert.IsTrue(deletedTaxRecord.IsDeleted);
    }

    [TearDown]
    public void TearDown()
    {
        applicationDbContext.Dispose();
    }
}
