namespace CarCare_Companion.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

using CarCare_Companion.Core.Contracts;
using CarCare_Companion.Core.Models.Status;


using static CarCare_Companion.Common.StatusResponses;
using CarCare_Companion.Core.Models.Vehicle;

[Route("[/Vehicle]")]
public class VehicleController : BaseController
{
    private readonly IVehicleService vehicleService;
    private readonly ILogger<VehicleController> logger;

    public VehicleController(IVehicleService vehicleService, ILogger<VehicleController> logger)
    {
        this.vehicleService = vehicleService;
        this.logger = logger;
    }

    [HttpGet]
    [Route("/FuelTypes")]
    [Produces("application/json")]
    public async Task<IActionResult> GetFuelTypes()
    {
        try
        {
            ICollection<FuelTypeRequestModel> fuelTypes = await vehicleService.GetAllFuelTypesAsync();

            return StatusCode(200, fuelTypes);
        }
        catch (SqlException ex)
        {
            logger.LogWarning(ex.Message);
            return StatusCode(400, new StatusErrorInformation(GenericError));
        }
        catch (Exception ex)
        {
            logger.LogInformation(ex.Message);
            return StatusCode(403, new StatusErrorInformation(InvalidData));
        }
    }

    [HttpGet]
    [Route("/VehicleTypes")]
    [Produces("application/json")]
    public async Task<IActionResult> GetVehicleTypes()
    {
        try
        {
            ICollection<VehicleTypeRequestModel> vehicleTypes = await vehicleService.GetAllVehicleTypesAsync();

            return StatusCode(200, vehicleTypes);
        }
        catch (SqlException ex)
        {
            logger.LogWarning(ex.Message);
            return StatusCode(400, new StatusErrorInformation(GenericError));
        }
        catch (Exception ex)
        {
            logger.LogInformation(ex.Message);
            return StatusCode(403, new StatusErrorInformation(InvalidData));
        }
    }
}
