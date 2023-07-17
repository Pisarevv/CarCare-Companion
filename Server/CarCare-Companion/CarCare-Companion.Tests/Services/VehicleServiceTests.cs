namespace CarCare_Companion.Tests.Services;

using Microsoft.EntityFrameworkCore;

using CarCare_Companion.Core.Contracts;
using CarCare_Companion.Core.Models.Vehicle;
using CarCare_Companion.Core.Services;
using CarCare_Companion.Infrastructure.Data;
using CarCare_Companion.Infrastructure.Data.Common;
using Amazon.S3.Model;


[TestFixture]
public class VehicleServiceTests
{
    private IRepository repository;
    private IVehicleService vehicleService;
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
    /// Tests the vehicle adding functionality with correct data
    /// </summary>
    [Test]
    public async Task AddVehicle()
    {
        //Arrange
        repository = new Repository(carCareCompanionDbContext);
        vehicleService = new VehicleService(repository);
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
        string responseId = await vehicleService.CreateAsync(OwnerId,vehicleToAdd);

        bool isCarCreated = await carCareCompanionDbContext.Vehicles.AnyAsync(v => v.Id == Guid.Parse(responseId));

        //Assert
        Assert.IsTrue(isCarCreated);


    }

    /// <summary>
    /// Tests the image adding to vehicle functionality with an existing vehicle
    /// </summary>
    [Test]
    public async Task AddImageToVehicle()
    {
        //Arrange
        string imageId = "123a5780-8fa7-4e53-99b5-93c31c26f6ec";
        string OwnerId = "0cda5780-8fa7-4e53-99b5-93c31c26f6ec";
        repository = new Repository(carCareCompanionDbContext);
        vehicleService = new VehicleService(repository);

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
        string vehicleId = await vehicleService.CreateAsync(OwnerId, vehicleToAdd);

        bool isAddedSuccessful = await vehicleService.AddImageToVehicle(vehicleId, imageId);

        //Assert
        Assert.IsTrue(isAddedSuccessful);

    }


    /// <summary>
    /// Tests the image adding to vehicle functionality with a non existing vehicle
    /// </summary>
    [Test]
    public async Task AddImageToVehicleWithNonExistingVehicle()
    {
        //Arrange
        string imageId = "123a5780-8fa7-4e53-99b5-93c31c26f6ec";
        repository = new Repository(carCareCompanionDbContext);
        vehicleService = new VehicleService(repository);

        //Act
        string vehicleId = "333a5780-8fa7-4e53-99b5-93c31c26f6ec";

        bool isAddedSuccessful = await vehicleService.AddImageToVehicle(vehicleId, imageId);

        //Assert
        Assert.IsFalse(isAddedSuccessful);

    }

    [TearDown]
    public void TearDown()
    {
        carCareCompanionDbContext.Dispose();
    }
}
