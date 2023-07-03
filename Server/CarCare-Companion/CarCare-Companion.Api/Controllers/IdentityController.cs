namespace CarCare_Companion.Api.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

using CarCare_Companion.Core.Contracts;
using CarCare_Companion.Core.Models.Identity;
using CarCare_Companion.Core.Services;

using static CarCare_Companion.Common.StatusResponses;
using CarCare_Companion.Core.Models.Status;



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
    /// <returns>Logs in the user and returns him with his email,id and JWT token</returns>
    [AllowAnonymous]
    [HttpPost("/Register")]
    [Produces("application/json")]
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

            bool userExist = await identityService.DoesUserExistAsync(registerData.Email);

            if (userExist)
            {
                return StatusCode(409, new StatusInformationMessage(UserEmailAlreadyExists));

            }

            AuthDataModel userData = await identityService.RegisterAsync(registerData);

            return StatusCode(201,userData);
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
    [HttpPost("/Login")]
    [Produces("application/json")]
    public async Task<IActionResult> Login([FromBody] LoginRequestModel loginData)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, new StatusInformationMessage(MissingOrInvalidFields));
            }

            AuthDataModel userData = await identityService.LoginAsync(loginData);

            return StatusCode(200, userData);
           
            
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


}
