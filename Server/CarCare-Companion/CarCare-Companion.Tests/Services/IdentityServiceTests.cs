namespace CarCare_Companion.Tests.Services;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Moq;

using CarCare_Companion.Core.Contracts;
using CarCare_Companion.Core.Models.Identity;
using CarCare_Companion.Core.Services;
using CarCare_Companion.Infrastructure.Data.Models.Identity;


[TestFixture]
public class IdentityServiceTests
{
    private IIdentityService identityService = null!;
    private IConfiguration configuration;
    private UserManager<ApplicationUser> userManager = null!;
    private RoleManager<ApplicationRole> roleManager = null!;

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
    }

    /// <summary>
    /// Tests the registration functionality with correct data
    /// </summary>
    [Test]
    public async Task UserRegistrationWithCorrectData()
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

        identityService = new IdentityService(userManager, roleManager, configuration);

        //Act
        var result = await identityService.RegisterAsync(registerRequestModel);

        //Assert
        Assert.That(result.Email, Is.EqualTo(registerRequestModel.Email));
        Assert.That(result.Id, Is.Not.Null);
        Assert.That(result.Role, Is.Not.Null.Or.Empty);
        Assert.That(result.Role, Is.EqualTo("User"));
        Assert.That(result.AccessToken, Is.Not.Null.Or.Empty);
    }

    /// <summary>
    /// Test the registration functionality with not successful registration process
    /// </summary>
    [Test]
    public void UserRegistrationShouldThrowExceptionOnUnsuccessfulRegister()
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

        identityService = new IdentityService(userManager, roleManager, configuration);

        //Act && Assert
        Task Act() =>  identityService.RegisterAsync(registerRequestModel);
        Assert.That(Act, Throws.Exception);

    }

    /// <summary>
    /// Test the login functionality with correct input data
    /// </summary>
    [Test]
    public async Task UserLoginShouldReturnCorrectData()
    {
        //Arange
        var userManagerMock = GenerateUserManagerMock();

        IList<string> emptyList = new List<string>();

        var user = new ApplicationUser { Id = Guid.NewGuid(), UserName = "exampleUser", Email = loginRequestModel.Email };

        userManagerMock
        .Setup(userManager => userManager.FindByNameAsync(It.IsAny<string>()))
        .ReturnsAsync(user);

        userManagerMock
        .Setup(userManager => userManager.CheckPasswordAsync(It.IsAny<ApplicationUser>(),It.IsAny<string>()))
        .ReturnsAsync(true);

        userManagerMock
        .Setup(userManager => userManager.GetRolesAsync(It.IsAny<ApplicationUser>()))
        .ReturnsAsync(emptyList);

        userManager = userManagerMock.Object;

        var roleManagerMock = GenerateRoleManagerMock();

        roleManager = roleManagerMock.Object;

        identityService = new IdentityService(userManager, roleManager, configuration);

        //Act
        var result = await identityService.LoginAsync(loginRequestModel);

        //Assert
        Assert.That(result.Email, Is.EqualTo(loginRequestModel.Email));
        Assert.That(result.Id, Is.Not.Null);
        Assert.That(result.Role, Is.Not.Null.Or.Empty);
        Assert.That(result.Role, Is.EqualTo("User"));
        Assert.That(result.AccessToken, Is.Not.Null.Or.Empty);

    }

    /// <summary>
    /// Tests if an exception is thrown when the user doesn't exist
    /// </summary>
    [Test]
    public void UserLoginShouldThrowErrorIfUserIsNotFound()
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

        identityService = new IdentityService(userManager, roleManager, configuration);

        //Act && Assert
        Task Act() => identityService.LoginAsync(loginRequestModel);
        Assert.That(Act, Throws.ArgumentNullException);

    }

    /// <summary>
    /// Tests if an exception is thrown when the user password is invalid
    /// </summary>
    [Test]
    public void UserLoginShouldThrowExceptionOnInvalidPassword()
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

        identityService = new IdentityService(userManager, roleManager, configuration);

        //Act && Assert
        Task Act() => identityService.LoginAsync(loginRequestModel);
        Assert.That(Act, Throws.ArgumentException);

    }

    /// <summary>
    /// Test the login functionality with correct input data
    /// </summary>
    [Test]
    public async Task UserLoginShouldReturnCorrectDataWithAdministratorClaim()
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

        identityService = new IdentityService(userManager, roleManager, configuration);

        //Act
        var result = await identityService.LoginAsync(loginRequestModel);

        //Assert
        Assert.That(result.Email, Is.EqualTo(loginRequestModel.Email));
        Assert.That(result.Id, Is.Not.Null);
        Assert.That(result.Role, Is.Not.Null.Or.Empty);
        Assert.That(result.Role, Is.EqualTo("Administrator"));
        Assert.That(result.AccessToken, Is.Not.Null.Or.Empty);

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
}
    
