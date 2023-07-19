﻿namespace CarCare_Companion.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

using CarCare_Companion.Core.Models.Status;
using CarCare_Companion.Common;
using CarCare_Companion.Core.Contracts;
using CarCare_Companion.Core.Models.ServiceRecords;

using static Common.StatusResponses;




/// <summary>
/// The service records controller handles service records related operations
/// </summary>
[Route("[controller]")]
[Produces("application/json")]
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
    [Produces("application/json")]
    public async Task<IActionResult> All()
    {
        try
        {
            string userId = this.User.GetId();

            if (string.IsNullOrEmpty(userId))
            {
                return StatusCode(403, InvalidUser);
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
    [Produces("application/json")]
    public async Task<IActionResult> ServiceRecordDetails([FromRoute] string recordId)
    {
        try
        {
            string userId = this.User.GetId();

            if (string.IsNullOrEmpty(userId))
            {
                return StatusCode(403, InvalidUser);
            }

            bool doesServiceRecordExist = await serviceRecordsService.DoesRecordExistByIdAsync(recordId);

            if(!doesServiceRecordExist)
            {
                return StatusCode(400, new StatusInformationMessage(StatusResponses.BadRequest));
            }

            bool isUserCreator = await serviceRecordsService.IsUserCreatorOfRecordAsync(userId, recordId);

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
    [Produces("application/json")]
    public async Task<IActionResult> Create([FromBody] ServiceRecordFormRequestModel model)
    {
        try
        {
            string userId = this.User.GetId();

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

}
 