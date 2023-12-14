namespace CarCare_Companion.Tests.Integration_Tests;

using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json;

using CarCare_Companion.Api;
using CarCare_Companion.Core.Contracts;
using CarCare_Companion.Core.Models.ServiceRecords;
using CarCare_Companion.Infrastructure.Data;
using CarCare_Companion.Infrastructure.Data.Models.Identity;
using CarCare_Companion.Tests.Integration_Tests.Seeding;

using static Seeding.Data.ApplicationUserData;
using static Seeding.Data.ServiceRecordsData;
using static Seeding.Data.VehiclesData;

using System.Net;
using CarCare_Companion.Infrastructure.Data.Models.Records;
using CarCare_Companion.Infrastructure.Data.Models.Vehicle;
using System.Text;

[TestFixture]
[Category("Integration")]
public class ServiceRecordsControllerTests
{
    private TestWebApplicationFactory<Program> factory;
    private HttpClient client;
    private CarCareCompanionDbContext dbContext;
    private IDbContextTransaction transaction;
    private IServiceProvider serviceProvider;

    private IJWTService jwtService;

    private List<ApplicationUser> users;


    [OneTimeSetUp]
    public async Task InitialSetup()
    {
        

        factory = new TestWebApplicationFactory<Program>();
        client = factory.CreateClient();

        using (var scope = factory.Services.CreateScope())
        {
            serviceProvider = scope.ServiceProvider;
            dbContext = serviceProvider.GetRequiredService<CarCareCompanionDbContext>();

            await dbContext.Database.EnsureDeletedAsync();
            await dbContext.Database.EnsureCreatedAsync();

            jwtService = serviceProvider.GetService<IJWTService>();

            // Seed the database
            await new TestDataSeeder().SeedAsync(dbContext, serviceProvider);
        }

        users = Users;
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

    /// <summary>
    /// Tests the GET endpoint of the ServiceRecords Controller with valid user claims
    /// </summary>
    [Test]
    public async Task GET_ReturnsSuccessAndCorrectContentType_WithData_WhenUserIsValid()
    {
        //Assert
        ICollection<string> userRoles = new HashSet<string>() { "User" };
        ICollection<Claim> claims = jwtService.GenerateUserAuthClaims(users[0], userRoles);

        var rawToken = jwtService.GenerateJwtToken(claims);
        string token = new JwtSecurityTokenHandler().WriteToken(rawToken);

        var request = new HttpRequestMessage(HttpMethod.Get, "/ServiceRecords");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        //Act
        var response = await client.SendAsync(request);

        var data = await response.Content.ReadAsStringAsync();
        ICollection<ServiceRecordDetailsResponseModel> resposeData = JsonConvert.DeserializeObject<ICollection<ServiceRecordDetailsResponseModel>>(data);

        //Assert
        Assert.That(response.IsSuccessStatusCode, Is.True);
        Assert.That(response.Content.Headers.ContentType.ToString(), Is.EqualTo("application/json; charset=utf-8"));
        Assert.IsNotNull(data);
    }

    /// <summary>
    /// Tests the GET endpoint of the ServiceRecords Controller with valid missing user claims 
    /// </summary>
    [Test]
    public async Task GET_ReturnsStatusCode403_WhenUserClaims_AreMissing()
    {
        //Assert
        ICollection<string> userRoles = new HashSet<string>();
        ICollection<Claim> claims = new List<Claim>();
   
        var rawToken = jwtService.GenerateJwtToken(claims);
        string token = new JwtSecurityTokenHandler().WriteToken(rawToken);

        var request = new HttpRequestMessage(HttpMethod.Get, "/ServiceRecords");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        //Act
        var response = await client.SendAsync(request);

        //Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
    }

    //Test the GET end point for record details of ServiceRecords Controller with valid user and record
    [Test]
    public async Task GET_ServiceRecordDetails_ShouldReturnRecord_WhenUserIsCreator_AndRecordExists()
    {
        //Assert 
        ICollection<string> userRoles = new HashSet<string>();
        ICollection<Claim> claims = jwtService.GenerateUserAuthClaims(users[0], userRoles);
        ServiceRecord serviceRecordData = ServiceRecords.Where(sr => sr.OwnerId == users[0].Id).First();

        var rawToken = jwtService.GenerateJwtToken(claims);
        string token = new JwtSecurityTokenHandler().WriteToken(rawToken);

        //Act
        var request = new HttpRequestMessage(HttpMethod.Get, $"/ServiceRecords/Details/{serviceRecordData.Id.ToString()}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        //Act
        var response = await client.SendAsync(request);

        var data = await response.Content.ReadAsStringAsync();
        ServiceRecordDetailsResponseModel responseData = JsonConvert.DeserializeObject<ServiceRecordDetailsResponseModel>(data);

        //Assert
        Assert.That(response.IsSuccessStatusCode, Is.True);
        Assert.That(response.Content.Headers.ContentType.ToString(), Is.EqualTo("application/json; charset=utf-8"));

        Assert.AreEqual(serviceRecordData.Id.ToString().ToUpper(), responseData.Id);
        Assert.AreEqual(serviceRecordData.Title, responseData.Title );
        Assert.AreEqual(serviceRecordData.Mileage, responseData.Mileage);
        Assert.AreEqual(serviceRecordData.PerformedOn, responseData.PerformedOn);
        Assert.AreEqual(serviceRecordData.Cost, responseData.Cost);
        Assert.AreEqual(serviceRecordData.Description, responseData.Description);

    }

    //Test the GET end point for record details of ServiceRecords Controller with invalid user claims 
    [Test]
    public async Task GET_ServiceRecordDetails_ReturnsStatusCode403_WhenUserClaims_AreMissing()
    {
     
        //Assert
        ICollection<string> userRoles = new HashSet<string>();
        ICollection<Claim> claims = new List<Claim>();

        var rawToken = jwtService.GenerateJwtToken(claims);
        string token = new JwtSecurityTokenHandler().WriteToken(rawToken);

        ServiceRecord serviceRecordData = ServiceRecords.Where(sr => sr.OwnerId == users[0].Id).First();

        //Act
        var request = new HttpRequestMessage(HttpMethod.Get, $"/ServiceRecords/Details/{serviceRecordData.Id.ToString()}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        //Act
        var response = await client.SendAsync(request);

        //Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));

    }

    //Test the GET end point for record details of ServiceRecords Controller with valid user and record
    [Test]
    public async Task GET_ServiceRecordDetails_ReturnsStatusCode403_WhenRecordDoesntExist()
    {
        //Assert 
        ICollection<string> userRoles = new HashSet<string>();
        ICollection<Claim> claims = jwtService.GenerateUserAuthClaims(users[0], userRoles);
        string nonExistingServiceRecordId = Guid.NewGuid().ToString();

        var rawToken = jwtService.GenerateJwtToken(claims);
        string token = new JwtSecurityTokenHandler().WriteToken(rawToken);

        var request = new HttpRequestMessage(HttpMethod.Get, $"/ServiceRecords/Details/{nonExistingServiceRecordId}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        //Act
        var response = await client.SendAsync(request);

        //Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));

    }

    //Test the GET end point for record details of ServiceRecords Controller with valid user but the not owner of the record
    [Test]
    public async Task GET_ServiceRecordDetails_ReturnsStatusCode403_WhenUserIsNotRecordOwner()
    {
        //Assert 
        ICollection<string> userRoles = new HashSet<string>();
        ICollection<Claim> claims = jwtService.GenerateUserAuthClaims(users[0], userRoles);
        ServiceRecord serviceRecordData = ServiceRecords.Where(sr => sr.OwnerId != users[0].Id).First();

        var rawToken = jwtService.GenerateJwtToken(claims);
        string token = new JwtSecurityTokenHandler().WriteToken(rawToken);

        //Act
        var request = new HttpRequestMessage(HttpMethod.Get, $"/ServiceRecords/Details/{serviceRecordData.Id.ToString()}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        //Act
        var response = await client.SendAsync(request);


        //Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
    }

    //Test the POST end point for creating a service record with valid data
    [Test]
    public async Task POST_Create_ReturnsStatusCode201_AndRecordData_WhenCreating_IsSuccessful()
    {
        //Assert 
        ICollection<string> userRoles = new HashSet<string>();
        ICollection<Claim> claims = jwtService.GenerateUserAuthClaims(users[0], userRoles);
        Vehicle userVehicle = Vehicles.Where(v => v.OwnerId == users[0].Id).First();

        var rawToken = jwtService.GenerateJwtToken(claims);
        string token = new JwtSecurityTokenHandler().WriteToken(rawToken);

        ServiceRecordFormRequestModel recordToCreate = new ServiceRecordFormRequestModel()
        {
            Title = "Title",
            Cost = 10,
            Description = "Description",
            Mileage = 1506,
            PerformedOn = DateTime.Now,
            VehicleId = userVehicle.Id.ToString(),
        };

        var recordJson = JsonConvert.SerializeObject(recordToCreate);

        var request = new HttpRequestMessage(HttpMethod.Post, "/ServiceRecords");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        request.Content = new StringContent(recordJson, Encoding.UTF8, "application/json");

        //Act
        var response = await client.SendAsync(request);

        var data = await response.Content.ReadAsStringAsync();
        ServiceRecordResponseModel responseData = JsonConvert.DeserializeObject<ServiceRecordResponseModel>(data);


        //Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));

        Assert.That(responseData.Title, Is.EqualTo(recordToCreate.Title));
        Assert.That(responseData.Cost, Is.EqualTo(recordToCreate.Cost));
        Assert.That(responseData.VehicleId, Is.EqualTo(recordToCreate.VehicleId));
        Assert.That(responseData.Description, Is.EqualTo(recordToCreate.Description));
        Assert.That(responseData.Mileage, Is.EqualTo(recordToCreate.Mileage));
        Assert.That(responseData.PerformedOn, Is.EqualTo(recordToCreate.PerformedOn));

    }

    //Test the POST end point for creating a service record with valid data
    [Test]
    public async Task POST_Create_ReturnsStatusCode400_WhenInputData_IsInvalid()
    {
        //Assert 
        ICollection<string> userRoles = new HashSet<string>();
        ICollection<Claim> claims = jwtService.GenerateUserAuthClaims(users[0], userRoles);
        Vehicle userVehicle = Vehicles.Where(v => v.OwnerId == users[0].Id).First();

        var rawToken = jwtService.GenerateJwtToken(claims);
        string token = new JwtSecurityTokenHandler().WriteToken(rawToken);

        ServiceRecordFormRequestModel recordToCreate = new ServiceRecordFormRequestModel()
        {
            Title = "",
            Cost = 10,
            Description = "Description",
            Mileage = 1506,
            PerformedOn = DateTime.Now,
            VehicleId = userVehicle.Id.ToString(),
        };

        var recordJson = JsonConvert.SerializeObject(recordToCreate);

        var request = new HttpRequestMessage(HttpMethod.Post, "/ServiceRecords");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        request.Content = new StringContent(recordJson, Encoding.UTF8, "application/json");

        //Act
        var response = await client.SendAsync(request);

        var data = await response.Content.ReadAsStringAsync();
        ServiceRecordResponseModel responseData = JsonConvert.DeserializeObject<ServiceRecordResponseModel>(data);


        //Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }


    /// <summary>
    /// Tests the POST endpoint for creating service record of the ServiceRecords Controller with missing user claims 
    /// </summary>
    [Test]
    public async Task POST_ReturnsStatusCode403_WhenUserClaims_AreMissing()
    {
        //Assert
        ICollection<string> userRoles = new HashSet<string>();
        ICollection<Claim> claims = new List<Claim>();
        Vehicle userVehicle = Vehicles.Where(v => v.OwnerId == users[0].Id).First();

        var rawToken = jwtService.GenerateJwtToken(claims);
        string token = new JwtSecurityTokenHandler().WriteToken(rawToken);

        ServiceRecordFormRequestModel recordToCreate = new ServiceRecordFormRequestModel()
        {
            Title = "Title",
            Cost = 10,
            Description = "Description",
            Mileage = 1506,
            PerformedOn = DateTime.Now,
            VehicleId = userVehicle.Id.ToString(),
        };

        var recordJson = JsonConvert.SerializeObject(recordToCreate);

        var request = new HttpRequestMessage(HttpMethod.Post, "/ServiceRecords");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        request.Content = new StringContent(recordJson, Encoding.UTF8, "application/json");

        //Act
        var response = await client.SendAsync(request);

        //Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
    }


    //Test the POST end point for creating a service record with valid data but non existing vehicle
    [Test]
    public async Task POST_Create_ReturnsStatusCode403_WhenVehicle_DoesntExist()
    {
        //Assert 
        ICollection<string> userRoles = new HashSet<string>();
        ICollection<Claim> claims = jwtService.GenerateUserAuthClaims(users[0], userRoles);
        string vehicleId = "879335f9-d671-46a0-bbe3-a8e2eb9089f9";

        var rawToken = jwtService.GenerateJwtToken(claims);
        string token = new JwtSecurityTokenHandler().WriteToken(rawToken);

        ServiceRecordFormRequestModel recordToCreate = new ServiceRecordFormRequestModel()
        {
            Title = "Test",
            Cost = 10,
            Description = "Description",
            Mileage = 1506,
            PerformedOn = DateTime.Now,
            VehicleId = vehicleId
        };

        var recordJson = JsonConvert.SerializeObject(recordToCreate);

        var request = new HttpRequestMessage(HttpMethod.Post, "/ServiceRecords");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        request.Content = new StringContent(recordJson, Encoding.UTF8, "application/json");

        //Act
        var response = await client.SendAsync(request);


        //Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
    }

    //Test the PATCH end point for editing a service record with valid data
    [Test]
    public async Task PATCH_Edit_ReturnsStatusCode200_AndEditedRecordData_WhenEditing_IsSuccessful()
    {
        //Assert 
        ICollection<string> userRoles = new HashSet<string>();
        ICollection<Claim> claims = jwtService.GenerateUserAuthClaims(users[0], userRoles);
        Vehicle userVehicle = Vehicles.Where(v => v.OwnerId == users[0].Id).First();
        string serviceRecordToEditId = ServiceRecords.Where(sr => sr.OwnerId == users[0].Id && sr.VehicleId == userVehicle.Id).First().Id.ToString();

        var rawToken = jwtService.GenerateJwtToken(claims);
        string token = new JwtSecurityTokenHandler().WriteToken(rawToken);

        ServiceRecordFormRequestModel editedRecord = new ServiceRecordFormRequestModel()
        {
            Title = "EditedTitle",
            Cost = 10,
            Description = "EditedDescription",
            Mileage = 1506,
            PerformedOn = DateTime.Now,
            VehicleId = userVehicle.Id.ToString(),
        };

        var recordJson = JsonConvert.SerializeObject(editedRecord);

        var request = new HttpRequestMessage(HttpMethod.Patch, $"/ServiceRecords/Edit/{serviceRecordToEditId}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        request.Content = new StringContent(recordJson, Encoding.UTF8, "application/json");

        //Act
        var response = await client.SendAsync(request);

        var data = await response.Content.ReadAsStringAsync();
        ServiceRecordResponseModel responseData = JsonConvert.DeserializeObject<ServiceRecordResponseModel>(data);


        ////Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        Assert.That(responseData.Title, Is.EqualTo(editedRecord.Title));
        Assert.That(responseData.Cost, Is.EqualTo(editedRecord.Cost));
        Assert.That(responseData.VehicleId, Is.EqualTo(editedRecord.VehicleId));
        Assert.That(responseData.Description, Is.EqualTo(editedRecord.Description));
        Assert.That(responseData.Mileage, Is.EqualTo(editedRecord.Mileage));
        Assert.That(responseData.PerformedOn, Is.EqualTo(editedRecord.PerformedOn));

    }

    //Test the PATCH end point for editing a service record with invalid data
    [Test]
    public async Task PATCH_Edit_ReturnsStatusCode400_WhenInputData_IsInvalid()
    {
        //Assert 
        ICollection<string> userRoles = new HashSet<string>();
        ICollection<Claim> claims = jwtService.GenerateUserAuthClaims(users[0], userRoles);
        Vehicle userVehicle = Vehicles.Where(v => v.OwnerId == users[0].Id).First();
        string serviceRecordToEditId = ServiceRecords.Where(sr => sr.OwnerId == users[0].Id && sr.VehicleId == userVehicle.Id).First().Id.ToString();

        var rawToken = jwtService.GenerateJwtToken(claims);
        string token = new JwtSecurityTokenHandler().WriteToken(rawToken);

        ServiceRecordFormRequestModel editedRecord = new ServiceRecordFormRequestModel()
        {
            Title = "",
            Cost = 10,
            Description = "EditedDescription",
            Mileage = 1506,
            PerformedOn = DateTime.Now,
            VehicleId = userVehicle.Id.ToString(),
        };

        var recordJson = JsonConvert.SerializeObject(editedRecord);

        var request = new HttpRequestMessage(HttpMethod.Patch, $"/ServiceRecords/Edit/{serviceRecordToEditId}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        request.Content = new StringContent(recordJson, Encoding.UTF8, "application/json");
        //Act
        var response = await client.SendAsync(request);

        //Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    /// <summary>
    /// Tests the PATCH endpoint for editing service record of the ServiceRecords Controller with missing user claims 
    /// </summary>
    [Test]
    public async Task PATCH_Edit_ReturnsStatusCode403_WhenUserClaims_AreMissing()
    {
        //Assert
        ICollection<string> userRoles = new HashSet<string>();
        ICollection<Claim> claims = new List<Claim>();
        Vehicle userVehicle = Vehicles.Where(v => v.OwnerId == users[0].Id).First();
        string serviceRecordToEditId = ServiceRecords.Where(sr => sr.OwnerId == users[0].Id && sr.VehicleId == userVehicle.Id).First().Id.ToString();

        var rawToken = jwtService.GenerateJwtToken(claims);
        string token = new JwtSecurityTokenHandler().WriteToken(rawToken);

        ServiceRecordFormRequestModel editedRecord = new ServiceRecordFormRequestModel()
        {
            Title = "Test",
            Cost = 10,
            Description = "EditedDescription",
            Mileage = 1506,
            PerformedOn = DateTime.Now,
            VehicleId = userVehicle.Id.ToString(),
        };

        var recordJson = JsonConvert.SerializeObject(editedRecord);

        var request = new HttpRequestMessage(HttpMethod.Patch, $"/ServiceRecords/Edit/{serviceRecordToEditId}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        request.Content = new StringContent(recordJson, Encoding.UTF8, "application/json");

        //Act
        var response = await client.SendAsync(request);

        //Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
    }

    //Test the PATCH end point for editing a service record with valid data but non existing vehicle
    [Test]
    public async Task PATCH_Edit_ReturnsStatusCode403_WhenVehicle_DoesntExist()
    {
        //Assert 
        string vehicleId = "879335f9-d671-46a0-bbe3-a8e2eb9089f9";

        ICollection<string> userRoles = new HashSet<string>();
        ICollection<Claim> claims = new List<Claim>();
        string serviceRecordToEditId = ServiceRecords.Where(sr => sr.OwnerId == users[0].Id).First().Id.ToString();

        var rawToken = jwtService.GenerateJwtToken(claims);
        string token = new JwtSecurityTokenHandler().WriteToken(rawToken);

        ServiceRecordFormRequestModel editedRecord = new ServiceRecordFormRequestModel()
        {
            Title = "Test",
            Cost = 10,
            Description = "EditedDescription",
            Mileage = 1506,
            PerformedOn = DateTime.Now,
            VehicleId = vehicleId,
        };

        var recordJson = JsonConvert.SerializeObject(editedRecord);

        var request = new HttpRequestMessage(HttpMethod.Patch, $"/ServiceRecords/Edit/{serviceRecordToEditId}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        request.Content = new StringContent(recordJson, Encoding.UTF8, "application/json");


        //Act
        var response = await client.SendAsync(request);


        //Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
    }

    //Test the PATCH end point for editing a service record with valid data but user is not record owner
    [Test]
    public async Task PATCH_Edit_ReturnsStatusCode403_WhenUser_IsNotRecordOwner()
    {
        //Assert 
        string vehicleId = "879335f9-d671-46a0-bbe3-a8e2eb9089f9";

        ICollection<string> userRoles = new HashSet<string>();
        ICollection<Claim> claims = new List<Claim>();
        string serviceRecordToEditId = ServiceRecords.Where(sr => sr.OwnerId != users[0].Id).First().Id.ToString();

        var rawToken = jwtService.GenerateJwtToken(claims);
        string token = new JwtSecurityTokenHandler().WriteToken(rawToken);

        ServiceRecordFormRequestModel editedRecord = new ServiceRecordFormRequestModel()
        {
            Title = "Test",
            Cost = 10,
            Description = "EditedDescription",
            Mileage = 1506,
            PerformedOn = DateTime.Now,
            VehicleId = vehicleId,
        };

        var recordJson = JsonConvert.SerializeObject(editedRecord);

        var request = new HttpRequestMessage(HttpMethod.Patch, $"/ServiceRecords/Edit/{serviceRecordToEditId}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        request.Content = new StringContent(recordJson, Encoding.UTF8, "application/json");


        //Act
        var response = await client.SendAsync(request);


        //Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
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
