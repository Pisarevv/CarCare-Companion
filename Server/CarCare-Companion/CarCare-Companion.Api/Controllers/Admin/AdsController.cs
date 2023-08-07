namespace CarCare_Companion.Api.Controllers.Admin;

using CarCare_Companion.Common;
using CarCare_Companion.Core.Contracts;
using CarCare_Companion.Core.Models.Ads;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

using static Common.GlobalConstants;

/// <summary>
/// The ads controller handles all ads related operations
/// </summary>
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
            string? userId = this.User.GetId();

            if (string.IsNullOrEmpty(userId))
            {
                return StatusCode(403, new ProblemDetails
                {
                    Title = StatusResponses.InvalidUser
                });
            }

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
                Detail = StatusResponses.InvalidData,

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
            string? userId = this.User.GetId();

            if (string.IsNullOrEmpty(userId))
            {
                return StatusCode(403, new ProblemDetails
                {
                    Title = StatusResponses.InvalidUser
                });
            }

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
                Detail = StatusResponses.InvalidData,

            });
        }

    }

    /// <summary>
    /// Edits a carousel ad
    /// </summary>
    /// <param name="carouselAdId">The ad identifier</param>
    /// <param name="model">The model containing the ad details</param>
    /// <returns>A status message based on the result</returns>
    [HttpPatch]
    [Route("CarouselAds/Edit/{carouselAdId}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(403, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> EditCarouselAd([FromRoute] string carouselAdId, [FromBody] CarouselAdFromRequestModel model)
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

            if (!ModelState.IsValid)
            {
                return StatusCode(400, new ProblemDetails()
                {
                    Detail = StatusResponses.InvalidData,

                });
            }

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

            await adService.EditAsync(carouselAdId,model);

            return StatusCode(200);
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
                Detail = StatusResponses.InvalidData,

            });
        }

    }
}
