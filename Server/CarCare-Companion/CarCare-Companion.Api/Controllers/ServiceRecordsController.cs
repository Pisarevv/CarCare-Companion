﻿namespace CarCare_Companion.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

using CarCare_Companion.Core.Models.Status;
using CarCare_Companion.Common;
using CarCare_Companion.Core.Contracts;
using CarCare_Companion.Core.Models.ServiceRecords;

using static Common.StatusResponses;
using CarCare_Companion.Core.Models.Trip;



/// <summary>
/// The service records controller handles service records related operations
/// </summary>
[Route("[controller]")]
public class ServiceRecordsController : BaseController
{
    private readonly IServiceRecordsService serviceRecordsService;
    private readonly IVehicleService vehicleService;
    private readonly ILogger<ServiceRecordsController> logger;

    public ServiceRecordsController(IServiceRecordsService serviceRecordsService, IVehicleService vehicleService, ILogger<ServiceRecordsController> logger)
    {
        this.serviceRecordsService = serviceRecordsService;
        this.vehicleService = vehicleService;
        this.logger = logger;
    }

    /// <summary>
    /// Retrieves all service records of an user
    /// </summary>
    /// <returns>A collection of user service records</returns>
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(ServiceRecordResponseModel))]
    [ProducesResponseType(400, Type = typeof(StatusInformationMessage))]
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

            ICollection<ServiceRecordResponseModel> serviceRecords = await serviceRecordsService.GetAllByUserIdAsync(userId);

            return StatusCode(200, serviceRecords);


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
    /// Retrieves a service record details of an user
    /// </summary>
    /// <returns>A collection of user service records</returns>
    [HttpGet]
    [Route("Details/{recordId}")]
    [ProducesResponseType(200, Type = typeof(ServiceRecordEditDetailsResponseModel))]
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

            bool doesServiceRecordExist = await serviceRecordsService.DoesRecordExistByIdAsync(recordId);

            if(!doesServiceRecordExist)
            {
                return StatusCode(400, new StatusInformationMessage(StatusResponses.BadRequest));
            }

            bool isUserCreator = await serviceRecordsService.IsUserRecordCreatorAsync(userId, recordId);

            if (!isUserCreator)
            {
                return StatusCode(403, InvalidUser);
            }

            ServiceRecordEditDetailsResponseModel serviceRecord = await serviceRecordsService.GetEditDetailsByIdAsync(recordId);

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
    /// Creates a new service records 
    /// </summary>
    /// <param name="model">The input data containing the service record information</param>
    /// <returns>The Id of the created service record</returns>
    [HttpPost]
    [ProducesResponseType(201, Type = typeof(string))]
    [ProducesResponseType(400, Type = typeof(StatusInformationMessage))]
    [ProducesResponseType(403, Type = typeof(StatusInformationMessage))]
    public async Task<IActionResult> Create([FromBody] ServiceRecordFormRequestModel model)
    {
        try
        {
            string? userId = this.User.GetId();

            if (string.IsNullOrEmpty(userId))
            {
                return StatusCode(403, InvalidUser);
            }

            if (!ModelState.IsValid)
            {
                return StatusCode(400, new StatusInformationMessage(InvalidData));
            }

            bool vehicleExists = await vehicleService.DoesVehicleExistByIdAsync(model.VehicleId);

            if (!vehicleExists)
            {
                return StatusCode(400, new StatusInformationMessage(InvalidData));
            }

            string recordId = await serviceRecordsService.CreateAsync(userId, model);

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
    /// Edits a service records 
    /// </summary>
    /// <param name="model">The input data containing the service record information</param>
    /// <param name="recordId">The record identifier</param>
    /// <returns>A status message based on the result</returns>
    [HttpPatch]
    [Route("Edit/{recordId}")]
    [ProducesResponseType(200, Type = typeof(StatusInformationMessage))]
    [ProducesResponseType(400, Type = typeof(StatusInformationMessage))]
    [ProducesResponseType(403, Type = typeof(StatusInformationMessage))]
    public async Task<IActionResult> Edit([FromRoute] string recordId, [FromBody] ServiceRecordFormRequestModel model)
    {
        try
        {
            string? userId = this.User.GetId();

            if (string.IsNullOrEmpty(userId))
            {
                return StatusCode(403, InvalidUser);
            }

            if (!ModelState.IsValid)
            {
                return StatusCode(400, new StatusInformationMessage(InvalidData));
            }

            bool vehicleExists = await vehicleService.DoesVehicleExistByIdAsync(model.VehicleId);

            if (!vehicleExists)
            {
                return StatusCode(400, new StatusInformationMessage(InvalidData));
            }

            bool isUserCreator = await serviceRecordsService.IsUserRecordCreatorAsync(userId, recordId);

            if (!isUserCreator)
            {
                return StatusCode(403, InvalidUser);
            }

            await serviceRecordsService.EditAsync(recordId, userId, model);

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
    /// Deletes a service records 
    /// </summary>
    /// <param name="recordId">The record identifier</param>
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

            bool isUserCreator = await serviceRecordsService.IsUserRecordCreatorAsync(userId, recordId);

            if (!isUserCreator)
            {
                return StatusCode(403, InvalidUser);
            }

            await serviceRecordsService.DeleteAsync(recordId, userId);

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
    /// Retrieves a specified count of user service records
    /// </summary>
    /// <param name="count">The count of service records to be retrieved</param>
    /// <returns>Collection of the user service records</returns>
    [HttpGet]
    [Route("Last/{count?}")]
    [ProducesResponseType(200, Type = typeof(ICollection<ServiceRecordBasicInformationResponseModel>))]
    [ProducesResponseType(400, Type = typeof(StatusInformationMessage))]
    [ProducesResponseType(403, Type = typeof(StatusInformationMessage))]
    public async Task<IActionResult> LastNCountServiceRecordsByUserId([FromQuery] int count = 3)
    {
        try
        {
            var userId = this.User.GetId();

            if (string.IsNullOrEmpty(userId))
            {
                return StatusCode(403, InvalidUser);
            }

            ICollection<ServiceRecordBasicInformationResponseModel> userServiceRecords = await serviceRecordsService.GetLastNCountAsync(userId, count);
            return StatusCode(200, userServiceRecords);

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
    /// Retrieves the user service records count
    /// </summary>
    /// <returns>A integer containing the total user service records count</returns>
    [HttpGet]
    [Route("Count")]
    [ProducesResponseType(200, Type = typeof(int))]
    [ProducesResponseType(400, Type = typeof(StatusInformationMessage))]
    [ProducesResponseType(403, Type = typeof(StatusInformationMessage))]
    public async Task<IActionResult> ServiceRecordsCount()
    {
        try
        {
            string? userId = this.User.GetId();

            if (string.IsNullOrEmpty(userId))
            {
                return StatusCode(403, InvalidUser);
            }

            int serviceRecordsCount = await serviceRecordsService.GetAllUserServiceRecordsCountAsync(userId);

            return StatusCode(200, serviceRecordsCount);


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
    /// Retrieves the user service records cost
    /// </summary>
    /// <returns>A decimal containing the total cost of the user service records</returns>
    [HttpGet]
    [Route("Cost")]
    [ProducesResponseType(200, Type = typeof(decimal))]
    [ProducesResponseType(400, Type = typeof(StatusInformationMessage))]
    [ProducesResponseType(403, Type = typeof(StatusInformationMessage))]
    public async Task<IActionResult> ServiceRecordsCost()
    {
        try
        {
            string? userId = this.User.GetId();

            if (string.IsNullOrEmpty(userId))
            {
                return StatusCode(403, InvalidUser);
            }

            decimal serviceRecordsCost = await serviceRecordsService.GetAllUserServiceRecordsCostAsync(userId);

            return StatusCode(200, serviceRecordsCost);


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
 