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
}
