namespace CarCare_Companion.Api.Controllers;

using Microsoft.AspNetCore.Mvc;

using CarCare_Companion.Common;
using CarCare_Companion.Core.Contracts;
using CarCare_Companion.Core.Models.Status;
using CarCare_Companion.Core.Models.TaxRecords;
using CarCare_Companion.Core.Services;

using static Common.StatusResponses;


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
    /// Creates a new tax record
    /// </summary>
    /// <param name="model">The model containing the tax record details</param>
    /// <returns>The Id of the tax record trip</returns>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TaxRecordFormRequestModel model)
    {
        if (!ModelState.IsValid)
        {
            return StatusCode(400, new StatusInformationMessage(InvalidData));
        }
        string? userId = this.User.GetId();

        if(string.IsNullOrEmpty(userId) )
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
}
