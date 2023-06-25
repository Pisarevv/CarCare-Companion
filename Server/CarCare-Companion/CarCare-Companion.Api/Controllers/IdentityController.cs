namespace CarCare_Companion.Api.Controllers;

using CarCare_Companion.Core.Contracts;
using CarCare_Companion.Core.Models.Identity;
using CarCare_Companion.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


[Route("[controller]")]
public class IdentityController : BaseController
{
    private readonly IIdentityService identityService;
    private readonly ILogger<IdentityService> logger;

    public IdentityController(IIdentityService identityService, ILogger<IdentityService> logger)
    {
        this.identityService = identityService;
        this.logger = logger;
    }

    [AllowAnonymous]
    [HttpPost("/Register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(422,"Missing or invalid fields");
            }

            bool userExist = await identityService.DoesUserExistAsync(model.Email);

            if (userExist)
            {
                return StatusCode(409,"User with the same email address already exists");

            }

            AuthDataModel userData = await identityService.RegisterAsync(model);

            return StatusCode(201,userData);
        }
        catch (Exception ex)
        {
            logger.LogInformation(ex.Message);
            return StatusCode(403,"Invalid data");

        }
    }

    [AllowAnonymous]
    [HttpPost("/Login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Missing or invalid fields");
            }

            AuthDataModel userData = await identityService.LoginAsync(model);

            return StatusCode(200, userData);
           
            
        }
        catch (Exception ex)
        {
            logger.LogInformation(ex.Message);
            return StatusCode(401, "Invalid credentials");
        }
    }


}
