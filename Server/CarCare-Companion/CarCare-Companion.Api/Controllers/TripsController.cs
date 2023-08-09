namespace CarCare_Companion.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

using CarCare_Companion.Common;
using CarCare_Companion.Core.Contracts;
using CarCare_Companion.Core.Models.Trip;
using CarCare_Companion.Core.Models.TripRecords;


/// <summary>
/// The trips controller handles trip related operations
/// </summary>
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
    [ProducesResponseType(200, Type = typeof(TripResponseModel))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(403, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Create([FromBody] TripFormRequestModel model)
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
                return StatusCode(400, new ProblemDetails
                {
                    Title = StatusResponses.InvalidData
                });
            }

            bool doesVehicleExist = await vehicleService.DoesVehicleExistByIdAsync(model.VehicleId);

            if (!doesVehicleExist)
            {
                return StatusCode(403, new ProblemDetails
                {
                    Title = StatusResponses.InvalidUser
                });
            }

            TripResponseModel createdTrip =await tripService.CreateAsync(userId, model);

            return StatusCode(200, createdTrip);
        }
        catch (SqlException ex)
        {
            logger.LogWarning(ex.Message);
            return StatusCode(400, new ProblemDetails
            {
                Title = StatusResponses.GenericError
            });
        }
        catch (Exception ex)
        {
            logger.LogInformation(ex.Message);
            return StatusCode(400, new ProblemDetails
            {
                Title = StatusResponses.BadRequest
            });
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
    [ProducesResponseType(200, Type = typeof(TripResponseModel))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(403, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Edit([FromRoute] string tripId, [FromBody] TripFormRequestModel model)
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
                return StatusCode(400, new ProblemDetails
                {
                    Title = StatusResponses.InvalidData
                });
            }

            bool doesTripRecordExist = await tripService.DoesTripExistByIdAsync(tripId);

            if (!doesTripRecordExist)
            {
                return StatusCode(403, new ProblemDetails
                {
                    Title = StatusResponses.InvalidUser
                });
            }

            bool isUserTripCreator = await tripService.IsUserCreatorOfTripAsync(userId, tripId);

            if (!isUserTripCreator)
            {
                return StatusCode(403, new ProblemDetails
                {
                    Title = StatusResponses.InvalidUser
                });
            }

            TripResponseModel editedTrip = await tripService.EditAsync(tripId, userId, model);

            return StatusCode(200, editedTrip);

        }
        catch (SqlException ex)
        {
            logger.LogWarning(ex.Message);
            return StatusCode(400, new ProblemDetails
            {
                Title = StatusResponses.GenericError
            });
        }
        catch (Exception ex)
        {
            logger.LogInformation(ex.Message);
            return StatusCode(400, new ProblemDetails
            {
                Title = StatusResponses.BadRequest
            });
        }
    }

    /// <summary>
    /// Deletes a trip record
    /// </summary>
    /// <param name="tripId">The vehicle identifier</param>
    /// <returns>A status code with message based on the process of deleting</returns>
    [HttpDelete]
    [Route("Delete/{tripId}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(403, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Delete([FromRoute] string tripId)
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

            bool isUserCreator = await tripService.IsUserCreatorOfTripAsync(userId, tripId);

            if (!isUserCreator)
            {
                return StatusCode(403, new ProblemDetails
                {
                    Title = StatusResponses.InvalidUser
                });
            }

            await tripService.DeleteAsync(tripId, userId);

            return StatusCode(200);

        }
        catch (SqlException ex)
        {
            logger.LogWarning(ex.Message);
            return StatusCode(400, new ProblemDetails
            {
                Title = StatusResponses.GenericError
            });
        }
        catch (Exception ex)
        {
            logger.LogInformation(ex.Message);
            return StatusCode(400, new ProblemDetails
            {
                Title = StatusResponses.BadRequest
            });
        }
    }

    /// <summary>
    /// Retrieves  all the user trips 
    /// </summary>
    /// <returns>Collection of the user trips </returns>
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(ICollection<TripDetailsByUserResponseModel>))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(403, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> AllTripsByUsedId()
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

            ICollection<TripDetailsByUserResponseModel> userTrips = await tripService.GetAllTripsByUsedIdAsync(userId);

            return StatusCode(200, userTrips);

        }
        catch (SqlException ex)
        {
            logger.LogWarning(ex.Message);
            return StatusCode(400, new ProblemDetails
            {
                Title = StatusResponses.GenericError
            });
        }
        catch (Exception ex)
        {
            logger.LogInformation(ex.Message);
            return StatusCode(400, new ProblemDetails
            {
                Title = StatusResponses.BadRequest
            });
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
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(403, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> TripDetails([FromRoute] string tripId)
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

            bool doesTripRecordExist = await tripService.DoesTripExistByIdAsync(tripId);

            if(!doesTripRecordExist) 
            {
                return StatusCode(403, new ProblemDetails
                {
                    Title = StatusResponses.InvalidUser
                });
            }

            bool isUserTripCreator = await tripService.IsUserCreatorOfTripAsync(userId, tripId);

            if(!isUserTripCreator)
            {
                return StatusCode(403, new ProblemDetails
                {
                    Title = StatusResponses.InvalidUser
                });
            }

            TripEditDetailsResponseModel trip = await tripService.GetTripDetailsByIdAsync(tripId);
            return StatusCode(200, trip);

        }
        catch (SqlException ex)
        {
            logger.LogWarning(ex.Message);
            return StatusCode(400, new ProblemDetails
            {
                Title = StatusResponses.GenericError
            });
        }
        catch (Exception ex)
        {
            logger.LogInformation(ex.Message);
            return StatusCode(400, new ProblemDetails
            {
                Title = StatusResponses.BadRequest
            });
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
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(403, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> LastNCountTripsByUserId([FromQuery] int count = 3)
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

            ICollection<TripBasicInformationByUserResponseModel> userTrips = await tripService.GetLastNCountAsync(userId,count);
            return StatusCode(200, userTrips);

        }
        catch (SqlException ex)
        {
            logger.LogWarning(ex.Message);
            return StatusCode(400, new ProblemDetails
            {
                Title = StatusResponses.GenericError
            });
        }
        catch (Exception ex)
        {
            logger.LogInformation(ex.Message);
            return StatusCode(400, new ProblemDetails
            {
                Title = StatusResponses.BadRequest
            });
        }
    }

    /// <summary>
    /// Retrieves  all the user trips count
    /// </summary>
    /// <returns>The count of the user trips</returns>
    [HttpGet]
    [Route("Count")]
    [ProducesResponseType(200, Type = typeof(string))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(403, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> UserTripsCount()
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

            int userTripsCount = await tripService.GetAllUserTripsCountAsync(userId);

            return StatusCode(200, userTripsCount);

        }
        catch (SqlException ex)
        {
            logger.LogWarning(ex.Message);
            return StatusCode(400, new ProblemDetails
            {
                Title = StatusResponses.GenericError
            });
        }
        catch (Exception ex)
        {
            logger.LogInformation(ex.Message);
            return StatusCode(400, new ProblemDetails
            {
                Title = StatusResponses.BadRequest
            });
        }
    }

    /// <summary>
    /// Retrieves  all the user trips cost
    /// </summary>
    /// <returns>The cost of the user trips</returns>
    [HttpGet]
    [Route("Cost")]
    [ProducesResponseType(200, Type = typeof(decimal?))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(403, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> UserTripsCost()
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

            decimal? userTripsCost = await tripService.GetAllUserTripsCostAsync(userId);

            return StatusCode(200, userTripsCost);

        }
        catch (SqlException ex)
        {
            logger.LogWarning(ex.Message);
            return StatusCode(400, new ProblemDetails
            {
                Title = StatusResponses.GenericError
            });
        }
        catch (Exception ex)
        {
            logger.LogInformation(ex.Message);
            return StatusCode(400, new ProblemDetails
            {
                Title = StatusResponses.BadRequest
            });
        }
    }
}
