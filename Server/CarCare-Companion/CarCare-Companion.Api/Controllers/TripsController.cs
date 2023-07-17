namespace CarCare_Companion.Api.Controllers;

using CarCare_Companion.Common;
using CarCare_Companion.Core.Contracts;
using CarCare_Companion.Core.Models.Status;
using CarCare_Companion.Core.Models.Trip;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

using static Common.StatusResponses;

[Route("[controller]")]
public class TripsController : BaseController
{
    private readonly ITripRecordsService tripService;
    private readonly IVehicleService vehicleService;
    private readonly ILogger<TripsController> logger;

    public TripsController
    (
        ITripRecordsService tripService, IVehicleService vehicleService, ILogger<TripsController> logger
    )

    {
        this.tripService = tripService;
        this.vehicleService = vehicleService;
        this.logger = logger;
    }

    /// <summary>
    /// Creates a trip on a vehicle selected by the user
    /// </summary>
    /// <param name="model">The model containing the trip details</param>
    /// <returns>The Id of the created trip</returns>
    [HttpPost]
    [Produces("application/json")]
    public async Task<IActionResult> CreateTrip([FromBody] TripFormRequestModel model)
    {
        try
        {

            if (!ModelState.IsValid)
            {
                return StatusCode(400, InvalidData);
            }

            var userId = this.User.GetId();

            if (string.IsNullOrEmpty(userId))
            {
                return StatusCode(403, InvalidUser);
            }

            bool doesVehicleExist = await vehicleService.DoesVehicleExistByIdAsync(model.VehicleId);

            if (!doesVehicleExist)
            {
                return StatusCode(404, ResourceNotFound);
            }

            string createdTripId = await tripService.CreateAsync(userId, model);

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
    /// Edits a user trip
    /// </summary>
    /// <returns>Model of the user trip with details </returns>
    [HttpPost]
    [Route("Edit/{tripId}")]
    [Produces("application/json")]
    public async Task<IActionResult> Edit([FromRoute] string tripId, TripFormRequestModel model)
    {
        try
        {
            var userId = this.User.GetId();

            if (string.IsNullOrEmpty(userId))
            {
                return StatusCode(403, InvalidUser);
            }

            bool doesTripRecordExist = await tripService.DoesTripExistById(tripId);

            if (!doesTripRecordExist)
            {
                return StatusCode(400, new StatusInformationMessage(StatusResponses.BadRequest));
            }

            bool isUserTripCreator = await tripService.IsUserCreatorOfTripAsync(userId, tripId);

            if (!isUserTripCreator)
            {
                return StatusCode(403, InvalidUser);
            }

            await tripService.EditAsync(tripId, model);

            return StatusCode(200, new StatusInformationMessage(Success));

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
    /// Retrieves  all the user trips 
    /// </summary>
    /// <returns>Collection of the user trips </returns>
    [HttpGet]
    [Produces("application/json")]
    public async Task<IActionResult> AllTripsByUsedId()
    {
        try
        {
            var userId = this.User.GetId();

            if (string.IsNullOrEmpty(userId))
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
    /// Retrieves a user trip
    /// </summary>
    /// <returns>Model of the user trip with details </returns>
    [HttpGet]
    [Route("Details/{tripId}")]
    [Produces("application/json")]
    public async Task<IActionResult> TripDetails([FromRoute] string tripId)
    {
        try
        {
            var userId = this.User.GetId();

            if (string.IsNullOrEmpty(userId))
            {
                return StatusCode(403, InvalidUser);
            }

            bool doesTripRecordExist = await tripService.DoesTripExistById(tripId);

            if(!doesTripRecordExist) 
            {
                return StatusCode(400, new StatusInformationMessage(StatusResponses.BadRequest));
            }

            bool isUserTripCreator = await tripService.IsUserCreatorOfTripAsync(userId, tripId);

            if(!isUserTripCreator)
            {
                return StatusCode(403, InvalidUser);
            }

            TripEditDetailsResponseModel trip = await tripService.GetTripDetailsByIdAsync(tripId);
            return StatusCode(200, trip);

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
    /// Retrieves  all the user trips 
    /// </summary>
    /// <returns>Collection of the user trips </returns>
    [HttpGet]
    [Route("Last/{count?}")]
    [Produces("application/json")]
    public async Task<IActionResult> LastNCountTripsByUserId([FromQuery] int count = 3)
    {
        try
        {
            var userId = this.User.GetId();

            if (string.IsNullOrEmpty(userId))
            {
                return StatusCode(403, InvalidUser);
            }

            ICollection<TripBasicInformationByUserResponseModel> userTrips = await tripService.GetLastNCountAsync(userId,count);
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
    /// Retrieves  all the user trips count
    /// </summary>
    /// <returns>The count of the user trips</returns>
    [HttpGet]
    [Route("Count")]
    [Produces("application/json")]
    public async Task<IActionResult> UserTripsCount()
    {
        try
        {
            var userId = this.User.GetId();

            if (string.IsNullOrEmpty(userId))
            {
                return StatusCode(403, InvalidUser);
            }

            int userTripsCount = await tripService.GetAllUserTripsCountAsync(userId);
            return StatusCode(200, userTripsCount);

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
    /// Retrieves  all the user trips cost
    /// </summary>
    /// <returns>The cost of the user trips</returns>
    [HttpGet]
    [Route("Cost")]
    [Produces("application/json")]
    public async Task<IActionResult> UserTripsCost()
    {
        try
        {
            var userId = this.User.GetId(); 
            
            if (string.IsNullOrEmpty(userId))
            {
                return StatusCode(403, InvalidUser);
            }

            decimal? userTripsCount = await tripService.GetAllUserTripsCostAsync(userId);
            return StatusCode(200, userTripsCount);

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
