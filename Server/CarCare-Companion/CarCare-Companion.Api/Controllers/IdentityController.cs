namespace CarCare_Companion.Api.Controllers;

using CarCare_Companion.Core.Contracts;
using CarCare_Companion.Core.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


[Route("[controller]")]
public class IdentityController : BaseController
{
    private readonly IIdentityService identityService;

    public IdentityController(IIdentityService identityService)
    {
        this.identityService = identityService;
    }

    [AllowAnonymous]
    [HttpPost("/Register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Missing or invalid fields");
            }

            bool userExist = await identityService.DoesUserExistAsync(model.Email);

            if (userExist)
            {
                return BadRequest("User with the same email already exists");

            }

            bool createdSuccessful = await identityService.RegisterAsync(model);

            if (!createdSuccessful)
            {
                return BadRequest("Something went bad");
            }

            return Ok("Successfully created account"); ;
        }
        catch (Exception)
        {
            return StatusCode(403,"Exception");

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


            LoginRequestStatus loginStatus = await identityService.LoginAsync(model);

            if (loginStatus.LoginSuccessful)
            {
                return Ok(new JwtTokenTransferModel
                {
                    Token = loginStatus.Token,
                    Expiration = loginStatus.Expiration
                });
            }

            else
            {
                return BadRequest(loginStatus.StatusMassage);
            }

            
        }
        catch (Exception ex)
        {
            return StatusCode(403, "Exception");

        }
    }


}
