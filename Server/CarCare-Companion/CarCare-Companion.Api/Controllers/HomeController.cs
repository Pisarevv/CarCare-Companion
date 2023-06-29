namespace CarCare_Companion.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

using CarCare_Companion.Core.Contracts;
using CarCare_Companion.Core.Models.Ads;
using CarCare_Companion.Core.Models.Status;

using Microsoft.AspNetCore.Authorization;

using static CarCare_Companion.Common.StatusResponses;


[Route("[controller]")]
[AllowAnonymous]
public class HomeController : BaseController
{
    private readonly IAdService adService;
    private readonly ILogger<HomeController> logger;

    public HomeController(IAdService adService, ILogger<HomeController> logger)
    {
        this.adService = adService;
        this.logger = logger;
    }

    [HttpGet(Name = "/Home")]
    [Produces("application/json")]
    public async Task<IActionResult> Index()
    {
        try
        {
            ICollection<CarouselAdRequestModel> ads = await adService.GetFiveAsync();
            return StatusCode(200, ads);
        }
        catch (SqlException ex)
        {
            logger.LogWarning(ex.Message);
            return StatusCode(400, new StatusErrorInformation(GenericError));
        }
        catch (Exception ex)
        {
            logger.LogInformation(ex.Message);
            return StatusCode(403, new StatusErrorInformation(InvalidData));
        }


    }

}
