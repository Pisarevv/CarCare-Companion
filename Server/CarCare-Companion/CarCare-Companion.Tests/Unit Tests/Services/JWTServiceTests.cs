namespace CarCare_Companion.Tests.Unit_Tests.Services;

using System.Security.Claims;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Moq;

using CarCare_Companion.Core.Contracts;
using CarCare_Companion.Core.Services;
using CarCare_Companion.Infrastructure.Data;
using CarCare_Companion.Infrastructure.Data.Common;
using CarCare_Companion.Infrastructure.Data.Models.Identity;

using static Common.GlobalConstants;

[TestFixture]
[Category("Unit")]
public class JWTServiceTests
{

    private IJWTService jwtService;
    private IRepository repository;
    private IConfiguration configuration;
    private Mock<UserManager<ApplicationUser>> mockUserManager;
    private CarCareCompanionDbContext applicationDbContext;

    private ApplicationUser user;

    [SetUp]
    public void Setup()
    {
        configuration = GetTestConfiguration();

        user = new ApplicationUser { Id = Guid.NewGuid(), UserName = "exampleUser", Email = "example@mail.com" };

        var contextOptions = new DbContextOptionsBuilder<CarCareCompanionDbContext>()
           .UseInMemoryDatabase("ServiceRecordsDb")
           .Options;
        applicationDbContext = new CarCareCompanionDbContext(contextOptions);

        this.mockUserManager = GenerateUserManagerMock();
        this.repository = new Repository(applicationDbContext);
        this.jwtService = new JWTService(mockUserManager.Object, repository, configuration);


        applicationDbContext.Database.EnsureDeleted();
        applicationDbContext.Database.EnsureCreated();
    }

    /// <summary>
    /// Tests JWT token refreshing with valid user
    /// </summary>
    [Test]
    public async Task RefreshJWTTokenAsync_ShouldRefreshToken_WhenUserExists()
    {
        //Arrange
        mockUserManager
        .Setup(x => x.FindByNameAsync(It.IsAny<string>()))
        .ReturnsAsync(user);

        List<string> userRoles = new List<string>();

        mockUserManager
        .Setup(x => x.GetRolesAsync(It.IsAny<ApplicationUser>()))
        .ReturnsAsync(userRoles);

 
        //Act
        var result = await jwtService.RefreshJWTTokenAsync(user.UserName);

        //Assert
        Assert.IsNotNull(result.AccessToken);
        Assert.IsNotNull(result.Email);
        Assert.IsNotNull(result.Role);
    }

    /// <summary>
    /// Tests JWT token refreshing with invalid user
    /// </summary>
    [Test]
    public async Task RefreshJWTTokenAsync_ShouldThrowArgumentNullException_WhenUserDoesntExists()
    {
        //Arrange
        mockUserManager
        .Setup(x => x.FindByNameAsync(It.IsAny<string>()))
        .ReturnsAsync((ApplicationUser)null);

        //Act && Assert
        Assert.ThrowsAsync<ArgumentNullException>(async () => await jwtService.RefreshJWTTokenAsync("NonExistingUser"));
    }

    /// <summary>
    /// Tests the auth claims generating
    /// </summary>
    [Test]
    public void GenerateUserAuthClaims_ShouldReturn_UserAuthClaims()
    {
        //Arrange
        ICollection<string> userRoles = new List<string>()
        {
            AdministratorRoleName
        };

        //Act
        var result = jwtService.GenerateUserAuthClaims(user, userRoles);

        //Assert
        Assert.IsNotNull(result);

    }

    /// <summary>
    /// Tests JWT token generation
    /// </summary>
    [Test]
    public void GenerateJwtToken_ShouldGenerate_JWTToken()
    {
        //Arrange
        var userClaims = new List<Claim>()
        {
             new Claim(ClaimTypes.Name, user.UserName),
             new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
             new Claim(ClaimTypes.Role, AdministratorRoleName)
        };

        //Act
        var result = jwtService.GenerateJwtToken(userClaims);

        //Assert
        Assert.IsNotNull(result.SigningCredentials);
        Assert.IsNotNull(result.Issuer);
        Assert.IsNotNull(result.Audiences);
        Assert.IsNotNull(result.Claims);
        Assert.IsNotNull(result.ValidTo);
    }

    private IConfiguration GetTestConfiguration()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.Test.json")
            .Build();

        return configuration;
    }

    private Mock<UserManager<ApplicationUser>> GenerateUserManagerMock()
    {
        return new Mock<UserManager<ApplicationUser>>(
            new Mock<IUserStore<ApplicationUser>>().Object,
            new Mock<IOptions<IdentityOptions>>().Object,
            new Mock<IPasswordHasher<ApplicationUser>>().Object,
            new IUserValidator<ApplicationUser>[0],
            new IPasswordValidator<ApplicationUser>[0],
            new Mock<ILookupNormalizer>().Object,
            new Mock<IdentityErrorDescriber>().Object,
            new Mock<IServiceProvider>().Object,
            new Mock<ILogger<UserManager<ApplicationUser>>>().Object);
    }

    [TearDown]
    public void TearDown()
    {
        applicationDbContext.Dispose();
    }

}
