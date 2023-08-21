namespace CarCare_Companion.Api.Controllers;


using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

using CarCare_Companion.Core.Contracts;
using CarCare_Companion.Core.Models.Identity;
using CarCare_Companion.Common;



/// <summary>
/// The IdentityController handles user-related operations - registration,login. 
/// </summary>
[Route("[controller]")]
public class IdentityController : BaseController
{
    private readonly IIdentityService identityService;
    private readonly IJWTService jwtService;
    private readonly IRefreshTokenService refreshTokenService;
    private readonly ILogger<IdentityController> logger;

    public IdentityController(IIdentityService identityService, IRefreshTokenService refreshTokenService, IJWTService jwtService, ILogger<IdentityController> logger)
    {
        this.identityService = identityService;
        this.refreshTokenService = refreshTokenService;
        this.jwtService = jwtService;
        this.logger = logger;
        this.refreshTokenService = refreshTokenService;
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
    [ProducesResponseType(200)]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(403, Type = typeof(ProblemDetails))]
    [ProducesResponseType(409, Type = typeof(ProblemDetails))]
   
    public async Task<IActionResult> Register([FromBody] RegisterRequestModel registerData)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, new ProblemDetails
                {
                    Title = StatusResponses.InvalidData
                });
            }

            bool userExist = await identityService.DoesUserExistByUsernameAsync(registerData.Email);

            if (userExist)
            {
                return StatusCode(409, new ProblemDetails
                {
                    Title = StatusResponses.UserEmailAlreadyExists
                });
            }

            await identityService.RegisterAsync(registerData);

            return StatusCode(200);
        }
        catch (SqlException ex)
        {
            logger.LogWarning(ex.Message);
            return StatusCode(400, new ProblemDetails
            {
                Title = StatusResponses.GenericError
            });
        }
        catch (Exception ex)
        {
            logger.LogInformation(ex.Message);
            return StatusCode(400, new ProblemDetails
            {
                Title = StatusResponses.BadRequest
            });
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
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(401, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Login([FromBody] LoginRequestModel loginData)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, new ProblemDetails
                {
                    Title = StatusResponses.MissingOrInvalidFields
                });
            }

            AuthDataInternalTransferModel userData = await identityService.LoginAsync(loginData);

            Response.Cookies.Append("refreshToken", userData.RefreshToken, GenerateCookieOptions());

            return StatusCode(200, new AuthDataModel
            {
                AccessToken = userData.AccessToken,
                Email = userData.Email,
                Role = userData.Role
            });


        }
        catch (ArgumentNullException ex)
        {
            logger.LogInformation(ex.Message);
            return StatusCode(403, new ProblemDetails
            {
                Title = StatusResponses.InvalidCredentials
            });
        }
        catch (ArgumentException ex)
        {
            logger.LogInformation(ex.Message);
            return StatusCode(403, new ProblemDetails
            {
                Title = ex.Message
            });
        }
        catch (SqlException ex)
        {
            logger.LogWarning(ex.Message);
            return StatusCode(400, new ProblemDetails
            {
                Title = StatusResponses.GenericError
            });
        }
        catch (Exception ex)
        {
            logger.LogInformation(ex.Message);
            return StatusCode(400, new ProblemDetails
            {
                Title = StatusResponses.BadRequest
            });
        }
    }

    /// <summary>
    /// Logs out the user by terminating the refresh token
    /// </summary>
    /// <returns>A status message based on the result</returns>
    [HttpPost]
    [Route("/Logout")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(401, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Logout()
    {
        try
        {
            string? userId = this.User.GetId();

            if (string.IsNullOrEmpty(userId))
            {
                return StatusCode(403, new ProblemDetails
                {
                    Title = StatusResponses.InvalidUser
                });
            }

            await refreshTokenService.TerminateUserRefreshTokenAsync(userId);

            return StatusCode(200);

        }
        catch (SqlException ex)
        {
            logger.LogWarning(ex.Message);
            return StatusCode(400, new ProblemDetails
            {
                Title = StatusResponses.GenericError
            });
        }
        catch (Exception ex)
        {
            logger.LogInformation(ex.Message);
            return StatusCode(400, new ProblemDetails
            {
                Title = StatusResponses.BadRequest
            });
        }
    }


    /// <summary>
    /// Refreshes the user JWT token
    /// </summary>
    /// <returns>AuthData model containing the new refresh JWT token</returns>
    [HttpGet]
    [Route("/Refresh")]
    [AllowAnonymous]
    [ProducesResponseType(200, Type = typeof(AuthDataModel))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(401, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> RefreshUserToken()
    {
        try
        {
            var refreshToken = Request.Cookies["refreshToken"];

            if (refreshToken == null)
            {
                return StatusCode(401, new ProblemDetails
                {
                    Title = StatusResponses.InvalidData
                });
            }

            string? refreshTokenOwnerUsername = await refreshTokenService.GetRefreshTokenOwnerAsync(refreshToken);

            if (refreshTokenOwnerUsername == null)
            {
                return StatusCode(401, new ProblemDetails
                {
                    Title = StatusResponses.TokenExpired
                });

            }

            bool isTokenExpired = await refreshTokenService.IsUserRefreshTokenExpiredAsync(refreshToken);

            if (isTokenExpired)
            {
                return StatusCode(401, new ProblemDetails
                {
                    Title = StatusResponses.TokenExpired
                });
            }

            AuthDataModel authData = await jwtService.RefreshJWTTokenAsync(refreshTokenOwnerUsername);

            return StatusCode(200, authData);

        }
        catch (SqlException ex)
        {
            logger.LogWarning(ex.Message);
            return StatusCode(400, new ProblemDetails
            {
                Title = StatusResponses.GenericError
            });
        }
        catch (Exception ex)
        {
            logger.LogInformation(ex.Message);
            return StatusCode(400, new ProblemDetails
            {
                Title = StatusResponses.BadRequest
            });
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
        //Change to SameSite.Strict in production
        cookieOptions.SameSite = (Microsoft.AspNetCore.Http.SameSiteMode)SameSiteMode.None;

        return cookieOptions;
    }
}
