namespace CarCare_Companion.Api.Controllers.Admin;

using CarCare_Companion.Common;
using CarCare_Companion.Core.Contracts;
using CarCare_Companion.Core.Models.Ads;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using static Common.GlobalConstants;
using static Common.StatusResponses;

[Route("[controller]")]

public class AdsController : BaseAdminController
{
    private readonly IAdService adService;
    private readonly IIdentityService identityService;
    private readonly ILogger<AdsController> logger;

    public AdsController(IAdService adService, IIdentityService identityService, ILogger<AdsController> logger)
    {
        this.adService = adService;
        this.identityService = identityService;
        this.logger = logger;
    }

    [HttpGet]
    [Route("CarouselAds")]
    [ProducesResponseType(200, Type = typeof(ICollection<CarouselAdResponseModel>))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(403, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            string userId = this.User.GetId()!;

            bool isUserAdministrator = await identityService.IsUserInRole(userId, AdministratorRoleName);


            if (!isUserAdministrator)
            {
                return StatusCode(403, new ProblemDetails()
                {
                    Detail = StatusResponses.BadRequest,

                });
            }

            ICollection<CarouselAdResponseModel> carouselAds = await adService.GetAllAsync();

            return StatusCode(200, carouselAds);
        }
        catch (SqlException ex)
        {
            logger.LogWarning(ex.Message);
            return StatusCode(400, new ProblemDetails()
            {
                Detail = StatusResponses.BadRequest,

            });
        }
        catch (Exception ex)
        {
            logger.LogInformation(ex.Message);
            return StatusCode(400, new ProblemDetails()
            {
                Detail = InvalidData,

            });
        }

    }

    [HttpGet]
    [Route("CarouselAds/Details/{carouselAdId}")]
    [ProducesResponseType(200, Type = typeof(CarouselAdResponseModel))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(403, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> CarouselAdDetails([FromRoute] string carouselAdId)
    {
        try
        {
            string userId = this.User.GetId()!;

            bool isUserAdministrator = await identityService.IsUserInRole(userId, AdministratorRoleName);


            if (!isUserAdministrator)
            {
                return StatusCode(403, new ProblemDetails()
                {
                    Detail = StatusResponses.BadRequest,

                });
            }

            bool doesAdExist = await adService.DoesAdExistAsync(carouselAdId);

            if (!doesAdExist)
            {
                return StatusCode(400, new ProblemDetails()
                {
                    Detail = StatusResponses.ResourceNotFound,

                });
            }

            CarouselAdResponseModel carouselAd = await adService.GetDetailsAsync(carouselAdId);

            return StatusCode(200, carouselAd);
        }
        catch (SqlException ex)
        {
            logger.LogWarning(ex.Message);
            return StatusCode(400, new ProblemDetails()
            {
                Detail = StatusResponses.BadRequest,

            });
        }
        catch (Exception ex)
        {
            logger.LogInformation(ex.Message);
            return StatusCode(400, new ProblemDetails()
            {
                Detail = InvalidData,

            });
        }

    }

    
}
