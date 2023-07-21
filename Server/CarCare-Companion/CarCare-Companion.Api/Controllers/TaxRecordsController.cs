namespace CarCare_Companion.Api.Controllers;

using Microsoft.AspNetCore.Mvc;

using CarCare_Companion.Common;
using CarCare_Companion.Core.Contracts;
using CarCare_Companion.Core.Models.Status;
using CarCare_Companion.Core.Models.TaxRecords;
using CarCare_Companion.Core.Services;

using static Common.StatusResponses;
using CarCare_Companion.Core.Models.ServiceRecords;
using Microsoft.Data.SqlClient;


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


    [HttpGet]
    [ProducesResponseType(200, Type = typeof(ICollection<TaxRecordResponseModel>))]
    [ProducesResponseType(403, Type = typeof(StatusInformationMessage))]
    public async Task<IActionResult> All()
    {
        try
        {
            string? userId = this.User.GetId();

            if (string.IsNullOrEmpty(userId))
            {
                return StatusCode(403, new StatusInformationMessage(InvalidUser));
            }

            ICollection<TaxRecordResponseModel> taxRecords = await taxRecordsService.GetAllByUserIdAsync(userId);

            return StatusCode(200, taxRecords);
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
    /// Creates a new tax record
    /// </summary>
    /// <param name="model">The model containing the tax record details</param>
    /// <returns>The Id of the tax record trip</returns>
    [HttpPost]
    [ProducesResponseType(201, Type = typeof(string))]
    [ProducesResponseType(400, Type = typeof(StatusInformationMessage))]
    [ProducesResponseType(403, Type = typeof(StatusInformationMessage))]
    public async Task<IActionResult> Create([FromBody] TaxRecordFormRequestModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, new StatusInformationMessage(InvalidData));
            }
            string? userId = this.User.GetId();

            if (string.IsNullOrEmpty(userId))
            {
                return StatusCode(403, new StatusInformationMessage(InvalidUser));
            }

            bool doesVehicleExist = await vehicleService.DoesVehicleExistByIdAsync(model.VehicleId);

            if (!doesVehicleExist)
            {
                return StatusCode(404, ResourceNotFound);
            }

            bool isUserVehicleOwner = await vehicleService.IsUserOwnerOfVehicleAsync(userId, model.VehicleId);

            if (!isUserVehicleOwner)
            {
                return StatusCode(400, new StatusInformationMessage(InvalidData));
            }

            string recordId = await taxRecordsService.CreateAsync(userId, model);

            return StatusCode(201, recordId);
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
    /// Retrieves a tax record details of an user
    /// </summary>
    /// <param name="recordId">The record identifier</param>
    /// <returns>A model containing the record details </returns>
    [HttpGet]
    [Route("Details/{recordId}")]
    [ProducesResponseType(200, Type = typeof(TaxRecordEditDetailsResponseModel))]
    [ProducesResponseType(400, Type = typeof(StatusInformationMessage))]
    [ProducesResponseType(403, Type = typeof(StatusInformationMessage))]
    public async Task<IActionResult> ServiceRecordDetails([FromRoute] string recordId)
    {
        try
        {
            string? userId = this.User.GetId();

            if (string.IsNullOrEmpty(userId))
            {
                return StatusCode(403, InvalidUser);
            }

            bool doesTaxRecordExist = await taxRecordsService.DoesRecordExistByIdAsync(recordId);

            if (!doesTaxRecordExist)
            {
                return StatusCode(400, new StatusInformationMessage(StatusResponses.BadRequest));
            }

            bool isUserCreator = await taxRecordsService.IsUserRecordCreatorAsync(userId, recordId);

            if (!isUserCreator)
            {
                return StatusCode(403, InvalidUser);
            }

            TaxRecordEditDetailsResponseModel serviceRecord = await taxRecordsService.GetEditDetailsByIdAsync(recordId);

            return StatusCode(200, serviceRecord);


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
    /// Edits a  tax record
    /// </summary>
    /// <param name="model">The model containing the tax record details</param>
    /// <param name="recordId">The record identifier</param>
    /// <returns>A status message based on the result</returns>
    [HttpPatch]
    [Route("Edit/{recordId}")]
    [ProducesResponseType(200, Type = typeof(StatusInformationMessage))]
    [ProducesResponseType(400, Type = typeof(StatusInformationMessage))]
    [ProducesResponseType(401, Type = typeof(StatusInformationMessage))]
    [ProducesResponseType(403, Type = typeof(StatusInformationMessage))]
    public async Task<IActionResult> Edit([FromRoute] string recordId,[FromBody] TaxRecordFormRequestModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, new StatusInformationMessage(InvalidData));
            }
            string? userId = this.User.GetId();

            if (string.IsNullOrEmpty(userId))
            {
                return StatusCode(403, new StatusInformationMessage(InvalidUser));
            }

            bool doesTaxRecordExist = await taxRecordsService.DoesRecordExistByIdAsync(recordId);

            if (!doesTaxRecordExist)
            {
                return StatusCode(400, new StatusInformationMessage(StatusResponses.BadRequest));
            }

            bool isUserCreator = await taxRecordsService.IsUserRecordCreatorAsync(userId, recordId);

            if (!isUserCreator)
            {
                return StatusCode(401, new StatusInformationMessage(NoPermission));
            }

            bool doesVehicleExist = await vehicleService.DoesVehicleExistByIdAsync(model.VehicleId);

            if (!doesVehicleExist)
            {
                return StatusCode(400, new StatusInformationMessage(StatusResponses.BadRequest));
            }

            bool isUserVehicleOwner = await vehicleService.IsUserOwnerOfVehicleAsync(userId, model.VehicleId);

            if (!isUserVehicleOwner)
            {
                return StatusCode(401, new StatusInformationMessage(NoPermission));
            }

            await taxRecordsService.EditAsync(recordId, model);

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
    /// Deletes a tax records 
    /// </summary>
    /// <param name="recordId">The tax identifier</param>
    /// <returns>A status message based on the result</returns>
    [HttpDelete]
    [Route("Delete/{recordId}")]
    [ProducesResponseType(200, Type = typeof(StatusInformationMessage))]
    [ProducesResponseType(400, Type = typeof(StatusInformationMessage))]
    [ProducesResponseType(403, Type = typeof(StatusInformationMessage))]
    public async Task<IActionResult> Delete([FromRoute] string recordId)
    {
        try
        {
            string? userId = this.User.GetId();

            if (string.IsNullOrEmpty(userId))
            {
                return StatusCode(403, InvalidUser);
            }

            bool doesTaxRecordExist = await taxRecordsService.DoesRecordExistByIdAsync(recordId);

            if (!doesTaxRecordExist)
            {
                return StatusCode(400, new StatusInformationMessage(StatusResponses.BadRequest));
            }


            bool isUserCreator = await taxRecordsService.IsUserRecordCreatorAsync(userId, recordId);

            if (!isUserCreator)
            {
                return StatusCode(403, InvalidUser);
            }

            await taxRecordsService.DeleteAsync(recordId);

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
}
