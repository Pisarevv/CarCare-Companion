namespace CarCare_Companion.Api.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

using CarCare_Companion.Core.Contracts;
using CarCare_Companion.Core.Models.Identity;
using CarCare_Companion.Core.Models.Status;

using static CarCare_Companion.Common.StatusResponses;
using Microsoft.Net.Http.Headers;
using CarCare_Companion.Common;
using Azure.Core;


/// <summary>
/// The IdentityController handles user-related operations - registration,login. 
/// </summary>
[Route("[controller]")]
public class IdentityController : BaseController
{
    private readonly IIdentityService identityService;
    private readonly ILogger<IdentityController> logger;

    public IdentityController(IIdentityService identityService, ILogger<IdentityController> logger)
    {
        this.identityService = identityService;
        this.logger = logger;
    }

    /// <summary>
    /// Creates a new user.
    /// </summary>
    /// <param name="registerData">The input data containing the user first name, 
    /// last name, email, password and confirm password</param>
    /// <returns>A status response code</returns>
    [AllowAnonymous]
    [HttpPost]
    [Route("/Register")]
    [ProducesResponseType(200, Type = typeof(StatusInformationMessage))]
    [ProducesResponseType(400, Type = typeof(StatusInformationMessage))]
    [ProducesResponseType(403, Type = typeof(StatusInformationMessage))]
    [ProducesResponseType(409, Type = typeof(StatusInformationMessage))]
   
    public async Task<IActionResult> Register([FromBody] RegisterRequestModel registerData)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, new
                {
                    title = MissingOrInvalidFields,
                });
            }

            bool userExist = await identityService.DoesUserExistByUsernameAsync(registerData.Email);

            if (userExist)
            {
                return StatusCode(409, new StatusInformationMessage(UserEmailAlreadyExists));

            }

            await identityService.RegisterAsync(registerData);

            return StatusCode(200, new StatusInformationMessage(Success));
        }
        catch (SqlException ex)
        {
            logger.LogWarning(ex.Message);
            return StatusCode(400, new StatusInformationMessage(GenericError));
        }
        catch (Exception ex)
        {
            logger.LogInformation(ex.Message);
            return StatusCode(403, new StatusInformationMessage(InvalidData));
        }
    }

    /// <summary>
    /// Logs in the user if the credentials are valid.
    /// </summary>
    /// <param name="loginData">The input data containing the user email and password</param>
    /// <returns>The logged in user with his email,id and JWT token</returns>
    [AllowAnonymous]
    [HttpPost]
    [Route("/Login")]
    [ProducesResponseType(200, Type = typeof(AuthDataModel))]
    [ProducesResponseType(400, Type = typeof(StatusInformationMessage))]
    [ProducesResponseType(401, Type = typeof(StatusInformationMessage))]
    public async Task<IActionResult> Login([FromBody] LoginRequestModel loginData)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, new StatusInformationMessage(MissingOrInvalidFields));
            }

            AuthDataInternalTransferModel userData = await identityService.LoginAsync(loginData);

            Response.Cookies.Append("refreshToken", userData.RefreshToken, GenerateCookieOptions());

            return StatusCode(200, new AuthDataModel
            {
                AccessToken = userData.AccessToken,
                Email = userData.Email,
            });
           
            
        }
        catch(ArgumentNullException ex)
        {
            logger.LogInformation(ex.Message);
            return StatusCode(401, new StatusInformationMessage(InvalidCredentials));
        }
        catch (ArgumentException ex)
        {
            logger.LogInformation(ex.Message);
            return StatusCode(401, new StatusInformationMessage(InvalidCredentials));
        }
        catch (SqlException ex)
        {
            logger.LogWarning(ex.Message);
            return StatusCode(400, new StatusInformationMessage(GenericError));
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex.Message);
            return StatusCode(400, new StatusInformationMessage(GenericError));
        }
        }


    /// <summary>
    /// Logs in the user if the credentials are valid.
    /// </summary>
    /// <param name="loginData">The input data containing the user email and password</param>
    /// <returns>The logged in user with his email,id and JWT token</returns>
    [HttpPost]
    [Route("/Logout")]
    [ProducesResponseType(200, Type = typeof(StatusInformationMessage))]
    [ProducesResponseType(400, Type = typeof(StatusInformationMessage))]
    [ProducesResponseType(401, Type = typeof(StatusInformationMessage))]
    public async Task<IActionResult> Logout()
    {
        try
        {
            string userId = this.User.GetId();

            if(userId == null)
            {
                return StatusCode(403, new StatusInformationMessage(InvalidUser));
            }

            await identityService.TerminateUserRefreshToken(userId);


            return StatusCode(200, new StatusInformationMessage(StatusResponses.Success));

        }
        catch (ArgumentNullException ex)
        {
            logger.LogInformation(ex.Message);
            return StatusCode(401, new StatusInformationMessage(InvalidCredentials));
        }
        catch (ArgumentException ex)
        {
            logger.LogInformation(ex.Message);
            return StatusCode(401, new StatusInformationMessage(InvalidCredentials));
        }
        catch (SqlException ex)
        {
            logger.LogWarning(ex.Message);
            return StatusCode(400, new StatusInformationMessage(GenericError));
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex.Message);
            return StatusCode(400, new StatusInformationMessage(GenericError));
        }
    }

    ///// <summary>
    ///// Refreshes the user JWT token
    ///// </summary>
    ///// <param name="jwtToken">The user JWT token</param>
    ///// <returns>AuthData model containing the new refresh JWT token</returns>
    //[HttpPost]
    //[Route("/Refresh")]
    //[AllowAnonymous]
    //[ProducesResponseType(200, Type = typeof(AuthDataModel))]
    //[ProducesResponseType(400, Type = typeof(StatusInformationMessage))]
    //[ProducesResponseType(401, Type = typeof(StatusInformationMessage))]
    //public async Task<IActionResult> RefreshUserToken([FromBody] string jwtToken)
    //{
    //    try
    //    {
    //        if (string.IsNullOrEmpty(jwtToken))
    //        {
    //            return StatusCode(400, new StatusInformationMessage(StatusResponses.BadRequest));
    //        }

    //        var refreshToken = Request.Cookies["refreshToken"];

    //        if(refreshToken == null)
    //        {
    //            return StatusCode(400, new StatusInformationMessage(InvalidData));
    //        }

    //        var principal = identityService.GetPrincipalFromExpiredToken(jwtToken);

    //        if (principal == null)
    //        {
    //            return StatusCode(400, new StatusInformationMessage(StatusResponses.BadRequest));
    //        }

    //        string username = principal.Identity.Name;

    //        bool isUserRefreshTokenOwner = await identityService.IsUserRefreshTokenOwner(username, refreshToken);

    //        if (!isUserRefreshTokenOwner)
    //        {
    //            return StatusCode(401, new StatusInformationMessage(NoPermission));
    //        }

    //        bool isTokenExpired = await identityService.IsUserRefreshTokenExpired(refreshToken);

    //        if (isTokenExpired)
    //        {
    //            return StatusCode(401, new StatusInformationMessage(TokenExpired));
    //        }

    //        AuthDataModel authData = await identityService.RefreshJWTToken(username);

    //        return StatusCode(200, authData);

    //    }
    //    catch (ArgumentNullException ex)
    //    {
    //        logger.LogInformation(ex.Message);
    //        return StatusCode(401, new StatusInformationMessage(InvalidCredentials));
    //    }
    //    catch (ArgumentException ex)
    //    {
    //        logger.LogInformation(ex.Message);
    //        return StatusCode(401, new StatusInformationMessage(InvalidCredentials));
    //    }
    //    catch (SqlException ex)
    //    {
    //        logger.LogWarning(ex.Message);
    //        return StatusCode(400, new StatusInformationMessage(GenericError));
    //    }
    //    catch (Exception ex)
    //    {
    //        logger.LogWarning(ex.Message);
    //        return StatusCode(400, new StatusInformationMessage(GenericError));
    //    }
    //}

    /// <summary>
    /// Refreshes the user JWT token
    /// </summary>
    /// <returns>AuthData model containing the new refresh JWT token</returns>
    [HttpGet]
    [Route("/Refresh")]
    [AllowAnonymous]
    [ProducesResponseType(200, Type = typeof(AuthDataModel))]
    [ProducesResponseType(400, Type = typeof(StatusInformationMessage))]
    [ProducesResponseType(401, Type = typeof(StatusInformationMessage))]
    public async Task<IActionResult> RefreshUserToken()
    {
        try
        {
            var refreshToken = Request.Cookies["refreshToken"];

            if (refreshToken == null)
            {
                return StatusCode(401, new StatusInformationMessage(InvalidData));
            }

            string? refreshTokenOwnerUsername = await identityService.GetRefreshTokenOwner(refreshToken);

            if (refreshTokenOwnerUsername == null)
            {
                return StatusCode(204, new StatusInformationMessage(TokenExpired));
            }

            bool isTokenExpired = await identityService.IsUserRefreshTokenExpired(refreshToken);

            if (isTokenExpired)
            {
                return StatusCode(401, new StatusInformationMessage(TokenExpired));
            }

            AuthDataModel authData = await identityService.RefreshJWTToken(refreshTokenOwnerUsername);

            return StatusCode(200, authData);

        }
        catch (SqlException ex)
        {
            logger.LogWarning(ex.Message);
            return StatusCode(400, new StatusInformationMessage(GenericError));
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex.Message);
            return StatusCode(400, new StatusInformationMessage(GenericError));
        }
    }



    /// <summary>
    /// Generates a HttpOnly secure cookie options
    /// </summary>
    /// <returns></returns>
    private CookieOptions GenerateCookieOptions()
    {
        var cookieOptions = new CookieOptions();
        cookieOptions.Expires = DateTime.UtcNow.AddDays(GlobalConstants.RefreshTokenExpirationTime);
        cookieOptions.Secure = true;
        cookieOptions.HttpOnly = true;
        cookieOptions.Path = "/";
        cookieOptions.SameSite = (Microsoft.AspNetCore.Http.SameSiteMode)SameSiteMode.None;

        return cookieOptions;
    }
}
