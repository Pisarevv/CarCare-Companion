namespace CarCare_Companion.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

using CarCare_Companion.Common;
using CarCare_Companion.Core.Contracts;
using CarCare_Companion.Core.Models.TaxRecords;
using CarCare_Companion.Core.Services;


/// <summary>
/// The service records controller handles tax records related operations
/// </summary>
[Route("[controller]")]
public class TaxRecordsController : BaseController
{
    private readonly ITaxRecordsService taxRecordsService;
    private readonly IVehicleService vehicleService;
    private readonly ILogger<TaxRecordsService> logger;

    public TaxRecordsController(ITaxRecordsService taxRecordsService, IVehicleService vehicleService, ILogger<TaxRecordsService> logger)
    {
        this.taxRecordsService = taxRecordsService;
        this.vehicleService = vehicleService;
        this.logger = logger;
    }

    /// <summary>
    /// Retrieves all tax records
    /// </summary>
    /// <returns>Collection of tax records</returns>
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(ICollection<TaxRecordDetailsResponseModel>))]
    [ProducesResponseType(403, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> All()
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

            ICollection<TaxRecordDetailsResponseModel> taxRecords = await taxRecordsService.GetAllByUserIdAsync(userId);

            return StatusCode(200, taxRecords);
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
    /// Creates a new tax record
    /// </summary>
    /// <param name="model">The model containing the tax record details</param>
    /// <returns>The Id of the tax record trip</returns>
    [HttpPost]
    [ProducesResponseType(201, Type = typeof(TaxRecordResponseModel))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(403, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Create([FromBody] TaxRecordFormRequestModel model)
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

            bool isUserVehicleOwner = await vehicleService.IsUserOwnerOfVehicleAsync(userId, model.VehicleId);

            if (!isUserVehicleOwner)
            {
                return StatusCode(403, new ProblemDetails
                {
                    Title = StatusResponses.InvalidUser
                });
            }

            TaxRecordResponseModel createdTaxRecord = await taxRecordsService.CreateAsync(userId, model);

            return StatusCode(201, createdTaxRecord);
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
    /// Retrieves a tax record details of an user
    /// </summary>
    /// <param name="recordId">The record identifier</param>
    /// <returns>A model containing the record details </returns>
    [HttpGet]
    [Route("Details/{recordId}")]
    [ProducesResponseType(200, Type = typeof(TaxRecordEditDetailsResponseModel))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(403, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> TaxRecordsDetails([FromRoute] string recordId)
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

            bool doesTaxRecordExist = await taxRecordsService.DoesRecordExistByIdAsync(recordId);

            if (!doesTaxRecordExist)
            {
                return StatusCode(403, new ProblemDetails
                {
                    Title = StatusResponses.InvalidUser
                });
            }

            bool isUserCreator = await taxRecordsService.IsUserRecordCreatorAsync(userId, recordId);

            if (!isUserCreator)
            {
                return StatusCode(403, new ProblemDetails
                {
                    Title = StatusResponses.InvalidUser
                });
            }

            TaxRecordEditDetailsResponseModel taxRecord = await taxRecordsService.GetEditDetailsByIdAsync(recordId);

            return StatusCode(200, taxRecord);


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
    /// Edits a  tax record
    /// </summary>
    /// <param name="model">The model containing the tax record details</param>
    /// <param name="recordId">The record identifier</param>
    /// <returns>A status message based on the result</returns>
    [HttpPatch]
    [Route("Edit/{recordId}")]
    [ProducesResponseType(200, Type = typeof(TaxRecordResponseModel))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(401, Type = typeof(ProblemDetails))]
    [ProducesResponseType(403, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Edit([FromRoute] string recordId,[FromBody] TaxRecordFormRequestModel model)
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

            bool doesTaxRecordExist = await taxRecordsService.DoesRecordExistByIdAsync(recordId);

            if (!doesTaxRecordExist)
            {
                return StatusCode(403, new ProblemDetails
                {
                    Title = StatusResponses.InvalidUser
                });
            }

            bool isUserCreator = await taxRecordsService.IsUserRecordCreatorAsync(userId, recordId);

            if (!isUserCreator)
            {
                return StatusCode(403, new ProblemDetails
                {
                    Title = StatusResponses.InvalidUser
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

            bool isUserVehicleOwner = await vehicleService.IsUserOwnerOfVehicleAsync(userId, model.VehicleId);

            if (!isUserVehicleOwner)
            {
                return StatusCode(403, new ProblemDetails
                {
                    Title = StatusResponses.InvalidUser
                });
            }

            TaxRecordResponseModel editedTaxRecord = await taxRecordsService.EditAsync(recordId, userId, model);

            return StatusCode(200, editedTaxRecord);
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
    /// Retrieves a specified count of user upcoming user taxes
    /// </summary>
    /// <param name="count">The count of records to be retrieved</param>
    /// <returns>Collection of the user upcoming Taxes</returns>
    [HttpGet]
    [Route("Upcoming/{count?}")]
    [ProducesResponseType(200, Type = typeof(ICollection<UpcomingTaxRecordResponseModel>))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(403, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> UpcomingNCountTaxesByUserId([FromQuery] int count = 3)
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

            ICollection<UpcomingTaxRecordResponseModel> userTrips = await taxRecordsService.GetUpcomingTaxesAsync(userId, count);
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
    /// Deletes a tax records 
    /// </summary>
    /// <param name="recordId">The tax identifier</param>
    /// <returns>A status message based on the result</returns>
    [HttpDelete]
    [Route("Delete/{recordId}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(403, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Delete([FromRoute] string recordId)
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

            bool doesTaxRecordExist = await taxRecordsService.DoesRecordExistByIdAsync(recordId);

            if (!doesTaxRecordExist)
            {
                return StatusCode(403, new ProblemDetails
                {
                    Title = StatusResponses.InvalidUser
                });
            }


            bool isUserCreator = await taxRecordsService.IsUserRecordCreatorAsync(userId, recordId);

            if (!isUserCreator)
            {
                return StatusCode(403, new ProblemDetails
                {
                    Title = StatusResponses.InvalidUser
                });
            }

            await taxRecordsService.DeleteAsync(recordId, userId);

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
    /// Retrieves the user tax records count
    /// </summary>
    /// <returns>A integer containing the total user tax records count</returns>
    [HttpGet]
    [Route("Count")]
    [ProducesResponseType(200, Type = typeof(int))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(403, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> TaxRecordsCount()
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

            int taxRecordsCount = await taxRecordsService.GetAllUserTaxRecordsCountAsync(userId);

            return StatusCode(200, taxRecordsCount);


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
    /// Retrieves the user tax records cost
    /// </summary>
    /// <returns>A decimal containing the total cost of the user tax records</returns>
    [HttpGet]
    [Route("Cost")]
    [ProducesResponseType(200, Type = typeof(decimal))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(403, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> TaxRecordsCost()
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

            decimal taxRecordsCost = await taxRecordsService.GetAllUserTaxRecordsCostAsync(userId);

            return StatusCode(200, taxRecordsCost);


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
