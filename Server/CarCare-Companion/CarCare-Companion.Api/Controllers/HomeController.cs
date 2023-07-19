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

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(CarouselAdResponseModel))]
    [ProducesResponseType(400, Type = typeof(StatusInformationMessage))]
    [ProducesResponseType(403, Type = typeof(StatusInformationMessage))]
    public async Task<IActionResult> Index()
    {
        try
        {
            ICollection<CarouselAdResponseModel> ads = await adService.GetFiveAsync();
            return StatusCode(200, ads);
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

}
