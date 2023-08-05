namespace CarCare_Companion.Api.Controllers;

using Microsoft.AspNetCore.Mvc;

using Microsoft.Data.SqlClient;
using CarCare_Companion.Common;
using CarCare_Companion.Core.Contracts;
using CarCare_Companion.Core.Models.Status;
using CarCare_Companion.Core.Models.Trip;

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
    [ProducesResponseType(200, Type = typeof(string))]
    [ProducesResponseType(400, Type = typeof(StatusInformationMessage))]
    [ProducesResponseType(403, Type = typeof(StatusInformationMessage))]
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

            return StatusCode(200, createdTripId);
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
    /// Edits a user trip record
    /// </summary>
    /// <param name="tripId">The trip identifier</param>
    /// <param name="model">The model containing the trip details</param>
    /// <returns>A status message based on the result</returns>
    [HttpPatch]
    [Route("Edit/{tripId}")]
    [ProducesResponseType(200, Type = typeof(StatusInformationMessage))]
    [ProducesResponseType(400, Type = typeof(StatusInformationMessage))]
    [ProducesResponseType(403, Type = typeof(StatusInformationMessage))]
    public async Task<IActionResult> Edit([FromRoute] string tripId,[FromBody] TripFormRequestModel model)
    {
        try
        {
            var userId = this.User.GetId();

            if (string.IsNullOrEmpty(userId))
            {
                return StatusCode(403, InvalidUser);
            }

            bool doesTripRecordExist = await tripService.DoesTripExistByIdAsync(tripId);

            if (!doesTripRecordExist)
            {
                return StatusCode(400, new StatusInformationMessage(StatusResponses.BadRequest));
            }

            bool isUserTripCreator = await tripService.IsUserCreatorOfTripAsync(userId, tripId);

            if (!isUserTripCreator)
            {
                return StatusCode(403, InvalidUser);
            }

            await tripService.EditAsync(tripId, userId, model);

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
    /// Deletes a trip record
    /// </summary>
    /// <param name="tripId">The vehicle identifier</param>
    /// <returns>A status code with message based on the process of deleting</returns>
    [HttpDelete]
    [Route("Delete/{tripId}")]
    [ProducesResponseType(200, Type = typeof(StatusInformationMessage))]
    [ProducesResponseType(400, Type = typeof(StatusInformationMessage))]
    [ProducesResponseType(403, Type = typeof(StatusInformationMessage))]
    public async Task<IActionResult> Delete([FromRoute] string tripId)
    {
        try
        {
            var userId = this.User.GetId();

            if (string.IsNullOrEmpty(userId))
            {
                return StatusCode(403, InvalidUser);
            }

            bool isUserCreator = await tripService.IsUserCreatorOfTripAsync(userId, tripId);

            if (!isUserCreator)
            {
                return StatusCode(403, InvalidUser);
            }

            await tripService.DeleteAsync(tripId, userId);

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
    [ProducesResponseType(200, Type = typeof(ICollection<TripDetailsByUserResponseModel>))]
    [ProducesResponseType(400, Type = typeof(StatusInformationMessage))]
    [ProducesResponseType(403, Type = typeof(StatusInformationMessage))]
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
    /// <param name="tripId">The trip identifier</param>
    /// <returns>Model of the user trip with details </returns>
    [HttpGet]
    [Route("Details/{tripId}")]
    [ProducesResponseType(200, Type = typeof(TripEditDetailsResponseModel))]
    [ProducesResponseType(400, Type = typeof(StatusInformationMessage))]
    [ProducesResponseType(403, Type = typeof(StatusInformationMessage))]
    public async Task<IActionResult> TripDetails([FromRoute] string tripId)
    {
        try
        {
            var userId = this.User.GetId();

            if (string.IsNullOrEmpty(userId))
            {
                return StatusCode(403, InvalidUser);
            }

            bool doesTripRecordExist = await tripService.DoesTripExistByIdAsync(tripId);

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
    /// Retrieves a specified count of user records
    /// </summary>
    /// <param name="count">The count of records to be retrieved</param>
    /// <returns>Collection of the user trips</returns>
    [HttpGet]
    [Route("Last/{count?}")]
    [ProducesResponseType(200, Type = typeof(ICollection<TripBasicInformationByUserResponseModel>))]
    [ProducesResponseType(400, Type = typeof(StatusInformationMessage))]
    [ProducesResponseType(403, Type = typeof(StatusInformationMessage))]
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
    [ProducesResponseType(200, Type = typeof(string))]
    [ProducesResponseType(400, Type = typeof(StatusInformationMessage))]
    [ProducesResponseType(403, Type = typeof(StatusInformationMessage))]
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
    [ProducesResponseType(200, Type = typeof(decimal?))]
    [ProducesResponseType(400, Type = typeof(StatusInformationMessage))]
    [ProducesResponseType(403, Type = typeof(StatusInformationMessage))]
    public async Task<IActionResult> UserTripsCost()
    {
        try
        {
            var userId = this.User.GetId(); 
            
            if (string.IsNullOrEmpty(userId))
            {
                return StatusCode(403, InvalidUser);
            }

            decimal? userTripsCost = await tripService.GetAllUserTripsCostAsync(userId);
            return StatusCode(200, userTripsCost);

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
