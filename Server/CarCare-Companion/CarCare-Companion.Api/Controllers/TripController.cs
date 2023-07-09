namespace CarCare_Companion.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

using CarCare_Companion.Core.Contracts;
using CarCare_Companion.Core.Models.Status;
using CarCare_Companion.Core.Models.Trip;

using static Common.StatusResponses;

[Route("api/[controller]")]
public class TripController : BaseController
{
    private readonly ITripService tripService;
    private readonly IVehicleService vehicleService;
    private readonly IIdentityService identityService;
    private readonly ILogger<TripController> logger;

    public TripController
    (
        ITripService tripService, IVehicleService vehicleService,
        IIdentityService identityService, ILogger<TripController> logger
    )

    {
        this.tripService = tripService;
        this.vehicleService = vehicleService;
        this.identityService = identityService;
        this.logger = logger;
    }

    /// <summary>
    /// Creates a trip on a vehicle selected by the user
    /// </summary>
    /// <param name="userId">The user identifier</param>
    /// <param name="model">The model containing the trip details</param>
    /// <returns>The Id of the created trip</returns>
    [HttpPost]
    [Route("/CreateTrip")]
    public async Task<IActionResult> CreateTrip([FromHeader] string userId,[FromBody] TripCreateRequestModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, InvalidData);
            }

            bool doesUserExist = await identityService.DoesUserExistByIdAsync(userId);

            if (!doesUserExist)
            {
                return StatusCode(403, InvalidUser);
            }

            bool doesVehicleExist = await vehicleService.DoesVehicleExistByIdAsync(model.VehicleId);

            if (!doesVehicleExist)
            {
                return StatusCode(404, ResourceNotFound);
            }

            string createdTripId = await tripService.CreateTripAsync(userId,model);

            return StatusCode(200, new StatusInformationMessage(createdTripId));
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
    /// Creates a trip on a vehicle selected by the user
    /// </summary>
    /// <param name="userId">The user identifier</param>
    /// <param name="model">The model containing the trip details</param>
    /// <returns>The Id of the created trip</returns>
    [HttpGet]
    [Route("/AllUserTrips")]
    public async Task<IActionResult> AllTripsByUsedIdAsync([FromHeader] string userId)
    {
        try
        {
            bool doesUserExist = await identityService.DoesUserExistByIdAsync(userId);

            if (!doesUserExist)
            {
                return StatusCode(403, InvalidUser);
            }

            ICollection<TripDetailsByUserResponseModel> userTrips = await tripService.GetAllTripsByUsedIdAsync(userId);           
            return StatusCode(200, userTrips);

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
