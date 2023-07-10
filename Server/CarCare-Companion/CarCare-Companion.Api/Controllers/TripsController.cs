namespace CarCare_Companion.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

using CarCare_Companion.Core.Contracts;
using CarCare_Companion.Core.Models.Status;
using CarCare_Companion.Core.Models.Trip;

using static Common.StatusResponses;

[Route("[controller]")]
public class TripsController : BaseController
{
    private readonly ITripService tripService;
    private readonly IVehicleService vehicleService;
    private readonly IIdentityService identityService;
    private readonly ILogger<TripsController> logger;

    public TripsController
    (
        ITripService tripService, IVehicleService vehicleService,
        IIdentityService identityService, ILogger<TripsController> logger
    )

    {
        this.tripService = tripService;
        this.vehicleService = vehicleService;
        this.identityService = identityService;
        this.logger = logger;
    }

    /// <summary>
    /// Retrieves  all the user trips 
    /// </summary>
    /// <param name="userId">The user identifier</param>
    /// <returns>Collection of the user trips </returns>
    [HttpGet]
    [Produces("application/json")]
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

    /// <summary>
    /// Creates a trip on a vehicle selected by the user
    /// </summary>
    /// <param name="userId">The user identifier</param>
    /// <param name="model">The model containing the trip details</param>
    /// <returns>The Id of the created trip</returns>
    [HttpPost]
    [Produces("application/json")]
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

    

   
}
