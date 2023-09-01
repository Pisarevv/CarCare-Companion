﻿namespace CarCare_Companion.Tests.Integration_Tests;

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

using System.Net;
using CarCare_Companion.Infrastructure.Data.Models.Records;

[TestFixture]
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
        ICollection<string> userRoles = new HashSet<string>();
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

    [Test]
    public async Task ServiceRecordDetails_ShouldReturnRecord_WhenUserIsCreator_AndRecordExists()
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