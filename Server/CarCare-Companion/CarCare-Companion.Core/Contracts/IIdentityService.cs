﻿namespace CarCare_Companion.Core.Contracts;

using CarCare_Companion.Core.Models.Identity;
using CarCare_Companion.Infrastructure.Data.Models.Identity;
using System.Security.Claims;

public interface IIdentityService
{
    public Task<bool> DoesUserExistByUsernameAsync(string username);

    public Task RegisterAsync(RegisterRequestModel model);

    public Task<AuthDataInternalTransferModel> LoginAsync(LoginRequestModel model);

    public Task<string> UpdateRefreshToken(ApplicationUser user);

    public Task<AuthDataModel> RefreshJWTToken(string username);

    public Task<bool> IsUserRefreshTokenOwner(string username,  string refreshToken);

    public Task<bool> IsUserRefreshTokenExpired(string refreshToken);

    public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);


}
