namespace CarCare_Companion.Tests.Services;

using CarCare_Companion.Core.Contracts;
using CarCare_Companion.Core.Models.Identity;
using CarCare_Companion.Core.Services;
using CarCare_Companion.Infrastructure.Data;
using CarCare_Companion.Infrastructure.Data.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

public class Tests
{
    [TestFixture]
    public class IdentityServiceTests
    {
        private IIdentityService identityService;
        private IConfiguration configuration;
        private UserManager <ApplicationUser> userManager;
        private RoleManager<ApplicationRole> roleManager;
        RegisterRequestModel registerRequestModel;


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
        }

        /// <summary>
        /// The test should register an user successfully with correct input data
        /// </summary>
        [Test]
        public async Task UserRegistrationWithCorrectData()
        {
            //Arange
            var userManagerMock = new Mock<UserManager<ApplicationUser>>(
                new Mock<IUserStore<ApplicationUser>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<ApplicationUser>>().Object,
                new IUserValidator<ApplicationUser>[0],
                new IPasswordValidator<ApplicationUser>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<ApplicationUser>>>().Object);

            IList<string> emptyList = new List<string>();

            userManagerMock
            .Setup(userManager => userManager.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
            .Returns(Task.FromResult(IdentityResult.Success));

            userManagerMock
            .Setup(userManager => userManager.GetRolesAsync(It.IsAny<ApplicationUser>()))
            .ReturnsAsync(emptyList);

            userManager = userManagerMock.Object;   

            var roleManagerMock = new Mock<RoleManager<ApplicationRole>>(
                 new Mock<IRoleStore<ApplicationRole>>().Object,
                 new IRoleValidator<ApplicationRole>[0],
                 new Mock<ILookupNormalizer>().Object,
                 new Mock<IdentityErrorDescriber>().Object,
                 new Mock<ILogger<RoleManager<ApplicationRole>>>().Object);

            roleManager = roleManagerMock.Object;

            var configuration = GetTestConfiguration();

            identityService = new IdentityService(userManager, roleManager, configuration);

            //Act
            var result = await identityService.RegisterAsync(registerRequestModel);

            //Assert
            Assert.That(result.Email, Is.EqualTo(registerRequestModel.Email));
            Assert.That(result.Id, Is.Not.Null);
            Assert.That(result.Role, Is.Not.Null.Or.Empty);
            Assert.That(result.Role, Is.EqualTo("User"));
            Assert.That(result.Token, Is.Not.Null.Or.Empty);
        }

        /// <summary>
        /// The test should throw an exception on unsuccessful registration 
        /// </summary>
        [Test]
        public async Task UserRegistrationShouldThrowExceptionOnNotSuccessfullRegister()
        {
            //Arange
            var userManagerMock = new Mock<UserManager<ApplicationUser>>(
                new Mock<IUserStore<ApplicationUser>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<ApplicationUser>>().Object,
                new IUserValidator<ApplicationUser>[0],
                new IPasswordValidator<ApplicationUser>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<ApplicationUser>>>().Object);

            IList<string> emptyList = new List<string>();

            userManagerMock
            .Setup(userManager => userManager.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
            .Returns(Task.FromResult(IdentityResult.Failed()));

            userManagerMock
            .Setup(userManager => userManager.GetRolesAsync(It.IsAny<ApplicationUser>()))
            .ReturnsAsync(emptyList);

            userManager = userManagerMock.Object;

            var roleManagerMock = new Mock<RoleManager<ApplicationRole>>(
                 new Mock<IRoleStore<ApplicationRole>>().Object,
                 new IRoleValidator<ApplicationRole>[0],
                 new Mock<ILookupNormalizer>().Object,
                 new Mock<IdentityErrorDescriber>().Object,
                 new Mock<ILogger<RoleManager<ApplicationRole>>>().Object);

            roleManager = roleManagerMock.Object;

            var configuration = GetTestConfiguration();

            identityService = new IdentityService(userManager, roleManager, configuration);

            //Act && Assert
            Task Act() => identityService.RegisterAsync(registerRequestModel);

            Assert.That(Act, Throws.Exception);

        }




        private IConfiguration GetTestConfiguration()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.Test.json")
                .Build();

            return configuration;
        }
    }
    
}