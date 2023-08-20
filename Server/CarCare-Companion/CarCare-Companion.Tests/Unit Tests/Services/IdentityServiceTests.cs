namespace CarCare_Companion.Tests.Services;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;

using Moq;

using CarCare_Companion.Core.Contracts;
using CarCare_Companion.Core.Models.Identity;
using CarCare_Companion.Core.Services;
using CarCare_Companion.Infrastructure.Data.Models.Identity;
using CarCare_Companion.Infrastructure.Data.Common;
using CarCare_Companion.Infrastructure.Data;

using static Common.GlobalConstants;



[TestFixture]
public class IdentityServiceTests
{
    private IIdentityService identityService;
    private IJWTService jwtService;
    private IRefreshTokenService refreshTokenService;
    private IConfiguration configuration;
    private IRepository repository;
    private UserManager<ApplicationUser> userManager;
    private RoleManager<ApplicationRole> roleManager;
    private CarCareCompanionDbContext applicationDbContext;

    RegisterRequestModel registerRequestModel;
    LoginRequestModel loginRequestModel;


    [SetUp]
    public void Setup()
    {
        registerRequestModel = new RegisterRequestModel
        {
            Email = "test@test.com",
            FirstName = "Test",
            LastName = "Test",
            Password = "123456a",
            ConfirmPassword = "123456a"
        };

        loginRequestModel = new LoginRequestModel
        {
            Email = "test@test.com",
            Password = "123456a"
        };

        configuration = GetTestConfiguration();

        var contextOptions = new DbContextOptionsBuilder<CarCareCompanionDbContext>()
           .UseInMemoryDatabase("ServiceRecordsDb")
           .Options;
        applicationDbContext = new CarCareCompanionDbContext(contextOptions);

        this.repository = new Repository(applicationDbContext);
        this.jwtService = new JWTService(userManager, repository, configuration);

        this.identityService = new IdentityService(userManager, roleManager, configuration, repository, jwtService,refreshTokenService);

        applicationDbContext.Database.EnsureDeleted();
        applicationDbContext.Database.EnsureCreated();
    }

    /// <summary>
    /// Tests the registration functionality with correct data
    /// </summary>
    [Test]
    public async Task UserRegistration_WithCorrectData()
    {
        //Arange
        var userManagerMock = GenerateUserManagerMock();

        IList<string> emptyList = new List<string>();

        userManagerMock
        .Setup(userManager => userManager.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
        .Returns(Task.FromResult(IdentityResult.Success));

        userManagerMock
        .Setup(userManager => userManager.GetRolesAsync(It.IsAny<ApplicationUser>()))
        .ReturnsAsync(emptyList);

        userManager = userManagerMock.Object;

        var roleManagerMock = GenerateRoleManagerMock();

        roleManager = roleManagerMock.Object;

        identityService = new IdentityService(userManager, roleManager, configuration, repository, jwtService, refreshTokenService);

        //Act
        bool result = await identityService.RegisterAsync(registerRequestModel);

        //Assert
        Assert.IsTrue(result);
    }

    /// <summary>
    /// Test the registration functionality with not successful registration process
    /// </summary>
    [Test]
    public void UserRegistration_ShouldThrowException_OnUnsuccessfulRegister()
    {
        //Arange
        var userManagerMock = GenerateUserManagerMock();

        IList<string> emptyList = new List<string>();

        userManagerMock
        .Setup(userManager => userManager.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
        .Returns(Task.FromResult(IdentityResult.Failed()));

        userManagerMock
        .Setup(userManager => userManager.GetRolesAsync(It.IsAny<ApplicationUser>()))
        .ReturnsAsync(emptyList);

        userManager = userManagerMock.Object;

        var roleManagerMock = GenerateRoleManagerMock();

        roleManager = roleManagerMock.Object;

        identityService = new IdentityService(userManager, roleManager, configuration, repository, jwtService, refreshTokenService);

        //Act && Assert
        Task Act() => identityService.RegisterAsync(registerRequestModel);
        Assert.That(Act, Throws.Exception);

    }

    /// <summary>
    /// Test the login functionality with correct input data
    /// </summary>
    [Test]
    public async Task UserLogin_ShouldReturnCorrectData()
    {
        //Arange
        var userManagerMock = GenerateUserManagerMock();

        IList<string> emptyList = new List<string>();

        var user = new ApplicationUser { Id = Guid.NewGuid(), UserName = "exampleUser", Email = loginRequestModel.Email };

        userManagerMock
        .Setup(userManager => userManager.FindByNameAsync(It.IsAny<string>()))
        .ReturnsAsync(user);

        userManagerMock
        .Setup(userManager => userManager.CheckPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
        .ReturnsAsync(true);

        userManagerMock
        .Setup(userManager => userManager.GetRolesAsync(It.IsAny<ApplicationUser>()))
        .ReturnsAsync(emptyList);

        userManager = userManagerMock.Object;

        var roleManagerMock = GenerateRoleManagerMock();

        roleManager = roleManagerMock.Object;

        identityService = new IdentityService(userManager, roleManager, configuration, repository, jwtService, refreshTokenService);

        //Act
        var result = await identityService.LoginAsync(loginRequestModel);

        //Assert
        Assert.That(result.Email, Is.EqualTo(loginRequestModel.Email));
        Assert.That(result.AccessToken, Is.Not.Null.Or.Empty);

    }

    /// <summary>
    /// Tests if an exception is thrown when the user doesn't exist
    /// </summary>
    [Test]
    public void UserLogin_ShouldThrowError_IfUserIsNotFound()
    {
        //Arange
        var userManagerMock = GenerateUserManagerMock();

        IList<string> emptyList = new List<string>();


#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        userManagerMock
        .Setup(userManager => userManager.FindByNameAsync(It.IsAny<string>()))
        .ReturnsAsync((ApplicationUser?)null);
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.


        userManagerMock
        .Setup(userManager => userManager.CheckPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
        .ReturnsAsync(true);

        userManagerMock
        .Setup(userManager => userManager.GetRolesAsync(It.IsAny<ApplicationUser>()))
        .ReturnsAsync(emptyList);

        userManager = userManagerMock.Object;

        var roleManagerMock = GenerateRoleManagerMock();

        roleManager = roleManagerMock.Object;

        identityService = new IdentityService(userManager, roleManager, configuration, repository, jwtService, refreshTokenService);

        //Act && Assert
        Task Act() => identityService.LoginAsync(loginRequestModel);
        Assert.That(Act, Throws.ArgumentNullException);

    }

    /// <summary>
    /// Tests if an exception is thrown when the user password is invalid
    /// </summary>
    [Test]
    public void UserLogin_ShouldThrowException_OnInvalidPassword()
    {
        //Arange
        var userManagerMock = GenerateUserManagerMock();

        IList<string> emptyList = new List<string>();

        var user = new ApplicationUser { Id = Guid.NewGuid(), UserName = "exampleUser", Email = loginRequestModel.Email };

        userManagerMock
        .Setup(userManager => userManager.FindByNameAsync(It.IsAny<string>()))
        .ReturnsAsync(user);

        userManagerMock
        .Setup(userManager => userManager.CheckPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
        .ReturnsAsync(false);

        userManagerMock
        .Setup(userManager => userManager.GetRolesAsync(It.IsAny<ApplicationUser>()))
        .ReturnsAsync(emptyList);

        userManager = userManagerMock.Object;

        var roleManagerMock = GenerateRoleManagerMock();

        roleManager = roleManagerMock.Object;

        identityService = new IdentityService(userManager, roleManager, configuration, repository, jwtService, refreshTokenService);

        //Act && Assert
        Task Act() => identityService.LoginAsync(loginRequestModel);
        Assert.That(Act, Throws.ArgumentException);

    }

    /// <summary>
    /// Test the login functionality with correct input data
    /// </summary>
    [Test]
    public async Task UserLogin_ShouldReturnCorrectData_WithAdministratorClaim()
    {
        //Arange
        var userManagerMock = GenerateUserManagerMock();

        IList<string> claims = new List<string>();
        claims.Add("Administrator");

        var user = new ApplicationUser { Id = Guid.NewGuid(), UserName = "exampleUser", Email = loginRequestModel.Email };

        userManagerMock
        .Setup(userManager => userManager.FindByNameAsync(It.IsAny<string>()))
        .ReturnsAsync(user);

        userManagerMock
        .Setup(userManager => userManager.CheckPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
        .ReturnsAsync(true);

        userManagerMock
        .Setup(userManager => userManager.GetRolesAsync(It.IsAny<ApplicationUser>()))
        .ReturnsAsync(claims);

        userManager = userManagerMock.Object;

        var roleManagerMock = GenerateRoleManagerMock();

        roleManager = roleManagerMock.Object;

        identityService = new IdentityService(userManager, roleManager, configuration, repository, jwtService, refreshTokenService);

        //Act
        var result = await identityService.LoginAsync(loginRequestModel);

        //Assert
        Assert.That(result.Email, Is.EqualTo(loginRequestModel.Email));
        Assert.That(result.AccessToken, Is.Not.Null.Or.Empty);

    }

    /// <summary>
    /// Test the searching by username functionality
    /// </summary>
    [Test]
    public async Task DoesUserExistByUsernameAsync_ShouldReturnTrue_IfUserExists()
    {
        //Arrange
        var userManagerMock = GenerateUserManagerMock();
        var user = new ApplicationUser { Id = Guid.NewGuid(), UserName = "exampleUser", Email = loginRequestModel.Email };

        userManagerMock
        .Setup(userManager => userManager.FindByNameAsync(It.IsAny<string>()))
        .ReturnsAsync(user);

        userManager = userManagerMock.Object;

        identityService = new IdentityService(userManager, roleManager, configuration, repository, jwtService, refreshTokenService);
        //Act
        bool doesUserExist = await identityService.DoesUserExistByUsernameAsync(user.UserName);

        //Assert
        Assert.IsTrue(doesUserExist);
    }

    /// <summary>
    /// Test the searching by username functionality
    /// </summary>
    [Test]
    public async Task DoesUserExistByUsernameAsync_ShouldReturnFalse_IfUserDoesntExists()
    {
        //Arrange
        string nonExistingUser = "nonExistingUser";
        var userManagerMock = GenerateUserManagerMock();

        userManagerMock
        .Setup(userManager => userManager.FindByNameAsync(It.IsAny<string>()))
        .ReturnsAsync((ApplicationUser?)null);

        userManager = userManagerMock.Object;

        identityService = new IdentityService(userManager, roleManager, configuration, repository, jwtService, refreshTokenService);
        //Act
        bool doesUserExist = await identityService.DoesUserExistByUsernameAsync(nonExistingUser);

        //Assert
        Assert.IsFalse(doesUserExist);
    }

    /// <summary>
    /// Test the searching by id functionality
    /// </summary>
    [Test]
    public async Task DoesUserExistByIdAsync_ShouldReturnTrue_IfUserExists()
    {
        //Arrange
        string userId = "908953a5-e4dc-4d82-9f0b-b2cbb7504fb0";
        var userManagerMock = GenerateUserManagerMock();
        var user = new ApplicationUser { Id = Guid.Parse(userId), UserName = "exampleUser", Email = loginRequestModel.Email };

        userManagerMock
        .Setup(userManager => userManager.FindByIdAsync(It.IsAny<string>()))
        .ReturnsAsync(user);

        userManager = userManagerMock.Object;

        identityService = new IdentityService(userManager, roleManager, configuration, repository, jwtService, refreshTokenService);
        //Act
        bool doesUserExist = await identityService.DoesUserExistByIdAsync(user.UserName);

        //Assert
        Assert.IsTrue(doesUserExist);
    }

    /// <summary>
    /// Test the searching by id functionality
    /// </summary>
    [Test]
    public async Task DoesUserExistByIdAsync_ShouldReturnFalse_IfUserDoesntExists()
    {
        //Arrange
        string nonExistingUser = "nonExistingUser";
        var userManagerMock = GenerateUserManagerMock();

        userManagerMock
        .Setup(userManager => userManager.FindByIdAsync(It.IsAny<string>()))
        .ReturnsAsync((ApplicationUser?)null);

        userManager = userManagerMock.Object;

        identityService = new IdentityService(userManager, roleManager, configuration, repository, jwtService, refreshTokenService);

        //Act
        bool doesUserExist = await identityService.DoesUserExistByIdAsync(nonExistingUser);

        //Assert
        Assert.IsFalse(doesUserExist);
    }

    /// <summary>
    /// Test the searching by id functionality
    /// </summary>
    [Test]
    public async Task IsUserInRoleAsync_ShouldReturnTrue_IfUserIsInRole()
    {
        //Arrange
        string userId = "908953a5-e4dc-4d82-9f0b-b2cbb7504fb0";
        string role = "Administrator";
        var userManagerMock = GenerateUserManagerMock();
        var user = new ApplicationUser { Id = Guid.Parse(userId), UserName = "exampleUser", Email = loginRequestModel.Email };

        userManagerMock
        .Setup(userManager => userManager.FindByIdAsync(It.IsAny<string>()))
        .ReturnsAsync(user);

        userManagerMock
       .Setup(userManager => userManager.IsInRoleAsync(It.IsAny<ApplicationUser>(), role))
       .ReturnsAsync(true);

        userManager = userManagerMock.Object;

        //Act
        bool isUserInRole = await identityService.IsUserInRoleAsync(user.UserName, role);

        //Assert
        Assert.IsTrue(isUserInRole);
    }

    /// <summary>
    /// Test the searching by id functionality
    /// </summary>
    [Test]
    public async Task IsUserInRoleAsync_ShouldReturnFalse_IfUserDoesntExists()
    {
        //Arrange
        string userId = "908953a5-e4dc-4d82-9f0b-b2cbb7504fb0";
        string role = "Administrator";
        string wrongRole = "User";
        var userManagerMock = GenerateUserManagerMock();

        var user = new ApplicationUser { Id = Guid.Parse(userId), UserName = "exampleUser", Email = loginRequestModel.Email };

        userManagerMock
       .Setup(userManager => userManager.FindByIdAsync(It.IsAny<string>()))
        .ReturnsAsync(user);

        userManagerMock
        .Setup(userManager => userManager.IsInRoleAsync(It.IsAny<ApplicationUser>(), role))
       .ReturnsAsync(true);

        userManagerMock
        .Setup(userManager => userManager.FindByIdAsync(It.IsAny<string>()))
        .ReturnsAsync(user);

        userManager = userManagerMock.Object;

        identityService = new IdentityService(userManager, roleManager, configuration, repository, jwtService, refreshTokenService);
        //Act
        bool isUserInRole = await identityService.IsUserInRoleAsync(user.UserName, wrongRole);

        //Assert
        Assert.IsFalse(isUserInRole);
    }



    /// <summary>
    /// Tests JWT token refreshing
    /// </summary>
    [Test]
    public async Task RefreshJWTTokenAsync_ShouldRefreshToken_WhenUserExists()
    {
        //Arrange
        var user = new ApplicationUser { Id = Guid.NewGuid(), UserName = "exampleUser", Email = loginRequestModel.Email };
        var userManagerMock = GenerateUserManagerMock();

        userManagerMock
       .Setup(userManager => userManager.FindByNameAsync(It.IsAny<string>()))
       .ReturnsAsync(user);

        userManagerMock
       .Setup(userManager => userManager.GetRolesAsync(It.IsAny<ApplicationUser>()))
       .ReturnsAsync(new List<string>());

        userManager = userManagerMock.Object;

        identityService = new IdentityService(userManager, roleManager, configuration, repository, jwtService, refreshTokenService);

        //Act
        var result = await jwtService.RefreshJWTTokenAsync(user.UserName);

        //Assert
        Assert.IsNotNull(result.AccessToken);
        Assert.IsNotNull(result.Email);
        Assert.IsNotNull(result.Role);
    }


    [Test]
    public async Task AddAdminAsync_ShouldAddUser_ToAdminRole()
    {
        //Arrange
        string userId = "908953a5-e4dc-4d82-9f0b-b2cbb7504fb0";
        var user = new ApplicationUser { Id = Guid.Parse(userId), UserName = "exampleUser", Email = loginRequestModel.Email };
        var userManagerMock = GenerateUserManagerMock();

        userManagerMock
       .Setup(userManager => userManager.FindByIdAsync(It.IsAny<string>()))
       .ReturnsAsync(user);

        userManagerMock
       .Setup(userManager => userManager.AddToRoleAsync(It.IsAny<ApplicationUser>(), AdministratorRoleName))
       .ReturnsAsync(IdentityResult.Success);

        userManager = userManagerMock.Object;

        identityService = new IdentityService(userManager, roleManager, configuration, repository, jwtService, refreshTokenService);

        //Act
        bool result = await identityService.AddAdminAsync(userId);

        Assert.IsTrue(result);
    }

    [Test]
    public async Task AddAdminAsync_ShouldThrowException_WhenOperationFails()
    {
        //Arrange
        string userId = "908953a5-e4dc-4d82-9f0b-b2cbb7504fb0";
        var user = new ApplicationUser { Id = Guid.Parse(userId), UserName = "exampleUser", Email = loginRequestModel.Email };
        var userManagerMock = GenerateUserManagerMock();

        userManagerMock
       .Setup(userManager => userManager.FindByIdAsync(It.IsAny<string>()))
       .ReturnsAsync(user);

        userManagerMock
       .Setup(userManager => userManager.AddToRoleAsync(It.IsAny<ApplicationUser>(), AdministratorRoleName))
       .ReturnsAsync(IdentityResult.Failed(new IdentityError[] { new IdentityError { Code = "123", Description = "Test" } }));

        userManager = userManagerMock.Object;

        identityService = new IdentityService(userManager, roleManager, configuration, repository, jwtService, refreshTokenService);

        //Act 
        //Assert
        Assert.ThrowsAsync<Exception>(() => identityService.AddAdminAsync(userId));
    }

    [Test]
    public async Task RemoveAdminAsync_ShouldRemoveUser_FromAdminRole()
    {
        //Arrange
        string userId = "908953a5-e4dc-4d82-9f0b-b2cbb7504fb0";
        var user = new ApplicationUser { Id = Guid.Parse(userId), UserName = "exampleUser", Email = loginRequestModel.Email };
        var userManagerMock = GenerateUserManagerMock();

        userManagerMock
       .Setup(userManager => userManager.FindByIdAsync(It.IsAny<string>()))
       .ReturnsAsync(user);

        userManagerMock
       .Setup(userManager => userManager.RemoveFromRoleAsync(It.IsAny<ApplicationUser>(), AdministratorRoleName))
       .ReturnsAsync(IdentityResult.Success);

        userManager = userManagerMock.Object;

        identityService = new IdentityService(userManager, roleManager, configuration, repository, jwtService, refreshTokenService);

        //Act
        bool result = await identityService.RemoveAdminAsync(userId);

        Assert.IsTrue(result);
    }

    [Test]
    public async Task RemoveAdminAsync_ShouldThrowException_WhenOperationFails()
    {
        //Arrange
        string userId = "908953a5-e4dc-4d82-9f0b-b2cbb7504fb0";
        var user = new ApplicationUser { Id = Guid.Parse(userId), UserName = "exampleUser", Email = loginRequestModel.Email };
        var userManagerMock = GenerateUserManagerMock();

        userManagerMock
       .Setup(userManager => userManager.FindByIdAsync(It.IsAny<string>()))
       .ReturnsAsync(user);

        userManagerMock
       .Setup(userManager => userManager.RemoveFromRoleAsync(It.IsAny<ApplicationUser>(), AdministratorRoleName))
       .ReturnsAsync(IdentityResult.Failed(new IdentityError[] { new IdentityError { Code = "123", Description = "Test" } }));

        userManager = userManagerMock.Object;

        identityService = new IdentityService(userManager, roleManager, configuration, repository, jwtService, refreshTokenService);

        //Act 
        //Assert
        Assert.ThrowsAsync<Exception>(() => identityService.RemoveAdminAsync(userId));
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

    private Mock<RoleManager<ApplicationRole>> GenerateRoleManagerMock()
    {
        return new Mock<RoleManager<ApplicationRole>>(
              new Mock<IRoleStore<ApplicationRole>>().Object,
              new IRoleValidator<ApplicationRole>[0],
              new Mock<ILookupNormalizer>().Object,
              new Mock<IdentityErrorDescriber>().Object,
              new Mock<ILogger<RoleManager<ApplicationRole>>>().Object);
    }

    private IConfiguration GetTestConfiguration()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.Test.json")
            .Build();

        return configuration;
    }

    [TearDown]
    public void TearDown()
    {
        applicationDbContext.Dispose();
    }
}

