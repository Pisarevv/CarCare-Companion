namespace CarCare_Companion.Core.Services;

using CarCare_Companion.Common;
using CarCare_Companion.Core.Contracts;
using CarCare_Companion.Infrastructure.Data.Common;
using CarCare_Companion.Infrastructure.Data.Models.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Threading.Tasks;

public class RefreshTokenService : IRefreshTokenService
{

    private readonly IRepository repository;

    public RefreshTokenService(IRepository repository)
    {
        this.repository = repository;
    }

    /// <summary>
    /// Updates the user refresh token. If the user doesn't have a token - a refresh token is added.
    /// If the user has a refresh token - the token is updated.
    /// </summary>
    /// <param name="user">The application user</param>
    /// <returns></returns>
    public async Task<string> UpdateRefreshTokenAsync(ApplicationUser user)
    {
        UserRefreshToken newToken = GenerateRefreshToken(user.Id.ToString());

        UserRefreshToken? userRefreshToken = await repository.All<UserRefreshToken>()
                                 .Where(urt => urt.UserId == user.Id)
        .FirstOrDefaultAsync();

        if (userRefreshToken == null)
        {
            user.RefreshToken = newToken;
            await repository.AddAsync<UserRefreshToken>(newToken);
            await repository.SaveChangesAsync();

            return newToken.RefreshToken.ToString();

        }

        userRefreshToken.RefreshToken = newToken.RefreshToken;
        userRefreshToken.RefreshTokenExpiration = newToken.RefreshTokenExpiration;
        await repository.SaveChangesAsync();

        return userRefreshToken.RefreshToken.ToString();
    }



    public async Task<string?> GetRefreshTokenOwnerAsync(string refreshToken)
    {
        return await repository.AllReadonly<UserRefreshToken>()
               .Where(urf => urf.RefreshToken == refreshToken)
               .Select(urf => urf.User.UserName)
               .FirstOrDefaultAsync();
    }


    /// <summary>
    /// Checks if the user is the refresh token owner
    /// </summary>
    /// <param name="username">The user identifier</param>
    /// <param name="refreshToken">The refresh token</param>
    /// <returns>Boolean based on the search result</returns>
    public async Task<bool> IsUserRefreshTokenOwnerAsync(string username, string refreshToken)
    {
        return await repository.AllReadonly<UserRefreshToken>()
               .Where(urt => urt.User.UserName == username && urt.RefreshToken == refreshToken)
               .AnyAsync();
    }

    /// <summary>
    /// Checks if the user refresh token is expired
    /// </summary>
    /// <param name="refreshToken">The refresh token</param>
    /// <returns>Boolean based on the search result</returns>
    public async Task<bool> IsUserRefreshTokenExpiredAsync(string refreshToken)
    {
        DateTime? tokenExpirationDate = await repository.AllReadonly<UserRefreshToken>()
            .Where(urt => urt.RefreshToken == refreshToken)
            .Select(urt => urt.RefreshTokenExpiration)
            .FirstAsync();

        return tokenExpirationDate < DateTime.UtcNow;
    }

    /// <summary>
    /// Generates a refresh token.
    /// </summary>
    /// <param name="userId">The user identifier</param>
    /// <returns>A refresh token containing an Id, UserId, RefreshToken and RefreshTokenExpiration</returns>
    private UserRefreshToken GenerateRefreshToken(string userId)
    {
        var randomNumber = new byte[256];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        var token = Convert.ToBase64String(randomNumber);

        UserRefreshToken refreshToken = new UserRefreshToken()
        {
            Id = Guid.NewGuid(),
            UserId = Guid.Parse(userId),
            RefreshToken = token,
            RefreshTokenExpiration = DateTime.UtcNow.AddDays(GlobalConstants.RefreshTokenExpirationTime),
        };


        return refreshToken;
    }

    public async Task<bool> TerminateUserRefreshTokenAsync(string userId)
    {
        UserRefreshToken? refreshToken = await repository.All<UserRefreshToken>()
                         .Where(urt => urt.UserId == Guid.Parse(userId))
                         .FirstOrDefaultAsync();

        if (refreshToken == null)
        {
            return false;
        }

        refreshToken.RefreshToken = null;
        refreshToken.RefreshTokenExpiration = DateTime.UtcNow;

        await repository.SaveChangesAsync();

        return true;

    }


}
