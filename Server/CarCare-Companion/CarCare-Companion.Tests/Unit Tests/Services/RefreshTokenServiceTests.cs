namespace CarCare_Companion.Tests.Unit_Tests.Services;

using CarCare_Companion.Core.Contracts;
using CarCare_Companion.Core.Models.Identity;
using CarCare_Companion.Core.Services;
using CarCare_Companion.Infrastructure.Data;
using CarCare_Companion.Infrastructure.Data.Common;
using CarCare_Companion.Infrastructure.Data.Models.Identity;
using Microsoft.EntityFrameworkCore;


[TestFixture]
public class RefreshTokenServiceTests
{

    private IRepository repository;
    private IRefreshTokenService refreshTokenService;
    private CarCareCompanionDbContext applicationDbContext;

    private LoginRequestModel loginRequestModel;

    [SetUp]
    public void Setup()
    {
        var contextOptions = new DbContextOptionsBuilder<CarCareCompanionDbContext>()
            .UseInMemoryDatabase("ServiceRecordsDb")
            .Options;
        applicationDbContext = new CarCareCompanionDbContext(contextOptions);

        this.repository = new Repository(applicationDbContext);
        this.refreshTokenService = new RefreshTokenService(repository);

        loginRequestModel = new LoginRequestModel
        {
            Email = "test@test.com",
            Password = "123456a"
        };
    }


    /// <summary>
    /// Tests refresh token updating when user doesn't have a previous token
    /// </summary>
    [Test]
    public async Task UpdateRefreshTokenAsync_ShouldGenerateAToken_WhenUserDoesntHave()
    {
        //Arange
        string userId = "908953a5-e4dc-4d82-9f0b-b2cbb7504fb0";
        var user = new ApplicationUser { Id = Guid.Parse(userId), UserName = "exampleUser", Email = loginRequestModel.Email };


        //Act 
        string result = await refreshTokenService.UpdateRefreshTokenAsync(user);

        //Assert
        Assert.IsNotNull(result);
    }

    /// <summary>
    /// Tests refresh token updating when user has a previous token
    /// </summary>
    [Test]
    public async Task UpdateRefreshTokenAsync_ShouldRefreshTheToken_WhenUserHasOne()
    {
        //Arange
        string userId = "908953a5-e4dc-4d82-9f0b-b2cbb7504fb0";
        string oldRefreshToken = "TestTestTestTest";
        var user = new ApplicationUser { Id = Guid.Parse(userId), UserName = "exampleUser", Email = loginRequestModel.Email };


        UserRefreshToken newToken = new UserRefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = Guid.Parse(userId),
            RefreshToken = oldRefreshToken,
            RefreshTokenExpiration = DateTime.UtcNow.AddDays(1)
        };

        await repository.AddAsync<UserRefreshToken>(newToken);
        await repository.SaveChangesAsync();


        //Act 
        string result = await refreshTokenService.UpdateRefreshTokenAsync(user);

        //Assert
        Assert.AreNotEqual(oldRefreshToken, result);
    }

    /// <summary>
    /// Tests if user is refresh token owner
    /// </summary>
    [Test]
    public async Task IsUserRefreshTokenOwnerAsync_ShouldReturnTrue_WhenUserIsTokenOwner()
    {
        //Arrange
        string userId = "938953a5-e4dc-4d82-9f0b-b2cbb7504fb0";
        var user = new ApplicationUser { Id = Guid.Parse(userId), UserName = "exampleUser", Email = loginRequestModel.Email };

        await repository.AddAsync(user);

        UserRefreshToken token = new UserRefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = Guid.Parse(userId),
            RefreshToken = "TestTestTestTest",
            RefreshTokenExpiration = DateTime.UtcNow.AddDays(1)
        };

        await repository.AddAsync<UserRefreshToken>(token);
        await repository.SaveChangesAsync();

        //Act
        bool result = await refreshTokenService.IsUserRefreshTokenOwnerAsync(user.UserName, token.RefreshToken);

        //Assert
        Assert.IsTrue(result);

    }


    /// <summary>
    /// Tests if user is refresh token owner
    /// </summary>
    [Test]
    public async Task IsUserRefreshTokenOwnerAsync_ShouldReturnFalse_WhenUserIsNotTokenOwner()
    {
        //Arrange
        string userId = "908153a5-e4dc-4d82-9f0b-b2cbb7504fb0";
        string otherUserName = "OtherUser";
        var user = new ApplicationUser { Id = Guid.Parse(userId), UserName = "exampleUser", Email = loginRequestModel.Email };

        await repository.AddAsync(user);

        UserRefreshToken token = new UserRefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = Guid.Parse(userId),
            RefreshToken = "TestTestTestTest",
            RefreshTokenExpiration = DateTime.UtcNow.AddDays(1)
        };

        await repository.AddAsync<UserRefreshToken>(token);
        await repository.SaveChangesAsync();

        //Act
        bool result = await refreshTokenService.IsUserRefreshTokenOwnerAsync(otherUserName, token.RefreshToken);

        //Assert
        Assert.IsFalse(result);

    }

    /// <summary>
    /// Tests if refresh token is expired
    /// </summary>
    [Test]
    public async Task IsUserRefreshTokenExpiredAsync_ShouldReturnTrue_WhenTokenIsExpired()
    {
        //Arrange
        UserRefreshToken token = new UserRefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            RefreshToken = "TestTestTestTest",
            RefreshTokenExpiration = DateTime.UtcNow.AddDays(-1)
        };

        await repository.AddAsync<UserRefreshToken>(token);
        await repository.SaveChangesAsync();

        //Act 
        bool result = await refreshTokenService.IsUserRefreshTokenExpiredAsync(token.RefreshToken);

        //Assert
        Assert.IsTrue(result);
    }

    /// <summary>
    /// Tests if refresh token is expired
    /// </summary>
    [Test]
    public async Task IsUserRefreshTokenExpiredAsync_ShouldReturnFalse_WhenTokenIsNotExpired()
    {
        //Arrange
        UserRefreshToken token = new UserRefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            RefreshToken = "TestTestTestTest",
            RefreshTokenExpiration = DateTime.UtcNow.AddDays(+1)
        };

        await repository.AddAsync<UserRefreshToken>(token);
        await repository.SaveChangesAsync();

        //Act 
        bool result = await refreshTokenService.IsUserRefreshTokenExpiredAsync(token.RefreshToken);

        //Assert
        Assert.IsFalse(result);
    }

    /// <summary>
    /// Tests the refresh token termination
    /// </summary>
    [Test]
    public async Task TerminateUserRefreshTokenAsync_ShouldReturnTrue_WhenUserHasOne()
    {
        //Arrange
        string userId = "908953aa-e4dc-4d82-9f0b-b2cbb7504fb0";
        var user = new ApplicationUser { Id = Guid.Parse(userId), UserName = "exampleUser", Email = loginRequestModel.Email };

        await repository.AddAsync(user);

        UserRefreshToken token = new UserRefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = Guid.Parse(userId),
            RefreshToken = "TestTestTestTest",
            RefreshTokenExpiration = DateTime.UtcNow.AddDays(1)
        };

        await repository.AddAsync<UserRefreshToken>(token);
        await repository.SaveChangesAsync();

        //Act
        bool result = await refreshTokenService.TerminateUserRefreshTokenAsync(userId);

        Assert.IsTrue(result);
    }

    /// <summary>
    /// Tests the refresh token termination
    /// </summary>
    [Test]
    public async Task TerminateUserRefreshTokenAsync_ShouldReturnFalse_WhenUserDoesntHaveOne()
    {
        //Arrange
        string userId = "908953a5-e4dc-4d82-9f0b-b2cbb7504fb0";

        //Act
        bool result = await refreshTokenService.TerminateUserRefreshTokenAsync(userId);

        Assert.IsFalse(result);
    }

    /// <summary>
    /// Tests the username finding based on a refresh token
    /// </summary>
    [Test]
    public async Task GetRefreshTokenOwnerAsync_ShouldReturnUsername_WhenUserExists()
    {
        //Arrange
        string userId = "908953a5-e4dc-4d82-9f0b-b2cbb7504fb0";
        var user = new ApplicationUser { Id = Guid.Parse(userId), UserName = "exampleUser", Email = loginRequestModel.Email };

        await repository.AddAsync(user);

        UserRefreshToken token = new UserRefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = Guid.Parse(userId),
            RefreshToken = "TestTestTestTest",
            RefreshTokenExpiration = DateTime.UtcNow.AddDays(1)
        };

        await repository.AddAsync<UserRefreshToken>(token);
        await repository.SaveChangesAsync();

        //Act
        string? result = await refreshTokenService.GetRefreshTokenOwnerAsync(token.RefreshToken);

        //Assert
        Assert.AreEqual(user.UserName, result);
    }

    /// <summary>
    /// Tests the username finding based on a refresh token
    /// </summary>
    [Test]
    public async Task GetRefreshTokenOwnerAsync_ShouldReturnNull_WhenUserDoesntExists()
    {
        //Arrange
        string userId = "908953a5-e4dc-4d82-9f0b-b2cbb7504fb0";
        string testUserName = "Test";


        UserRefreshToken token = new UserRefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = Guid.Parse(userId),
            RefreshToken = "TestTestTestTest",
            RefreshTokenExpiration = DateTime.UtcNow.AddDays(1)
        };

        await repository.AddAsync<UserRefreshToken>(token);
        await repository.SaveChangesAsync();

        //Act
        string? result = await refreshTokenService.GetRefreshTokenOwnerAsync(token.RefreshToken);

        //Assert
        Assert.IsNull(result);
    }

    [TearDown]
    public void TearDown()
    {
        applicationDbContext.Dispose();
    }
}
