namespace CarCare_Companion.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

using CarCare_Companion.Core.Contracts;
using CarCare_Companion.Core.Models.Status;
using CarCare_Companion.Core.Models.Vehicle;

using static CarCare_Companion.Common.StatusResponses;



[Route("[/Vehicle]")]
public class VehicleController : BaseController
{
    private readonly IVehicleService vehicleService;
    private readonly IImageService imageService;
    private readonly ILogger<VehicleController> logger;

    public VehicleController(IVehicleService vehicleService, IImageService imageService, ILogger<VehicleController> logger)
    {
        this.vehicleService = vehicleService;
        this.imageService = imageService;
        this.logger = logger;
    }

    [HttpGet]
    [Route("/FuelTypes")]
    [Produces("application/json")]
    public async Task<IActionResult> GetFuelTypes()
    {
        try
        {
            ICollection<FuelTypeResponseModel> fuelTypes = await vehicleService.GetAllFuelTypesAsync();
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
            ICollection<VehicleTypeResponseModel> vehicleTypes = await vehicleService.GetAllVehicleTypesAsync();

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

    [HttpPost]
    [Route("/VehicleCreate")]
    [Produces("application/json")]
    public async Task<IActionResult> CreateVehicle(VehicleCreateRequestModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, new
                {
                    title = MissingOrInvalidFields,
                });
            }
            string vehicleId = await vehicleService.CreateVehicleAsync(model);
          
            return StatusCode(200, vehicleId);

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

    [HttpPost]
    [Route("/VehicleImageUpload")]
    [Produces("application/json")]
    public async Task<IActionResult> UploadVehicleImage([FromHeader] string vehicleId,[FromForm] IFormFile file)
    {
        if(file.Length == 0 || file == null)
        {
            return StatusCode(400, new StatusErrorInformation(GenericError));
        }

        string imageId = await imageService.UploadVehicleImage(file);

        await  vehicleService.AddImageToVehicle(vehicleId, imageId);
        return StatusCode(200);

    }

}
