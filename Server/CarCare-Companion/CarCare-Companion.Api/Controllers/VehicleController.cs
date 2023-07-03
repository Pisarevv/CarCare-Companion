namespace CarCare_Companion.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

using CarCare_Companion.Core.Contracts;
using CarCare_Companion.Core.Models.Status;
using CarCare_Companion.Core.Models.Vehicle;

using static CarCare_Companion.Common.StatusResponses;


/// <summary>
/// The vehicle controller handles vehicle related operations
/// </summary>
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

    /// <summary>
    /// Returns the available fuel types
    /// </summary>
    /// <returns>Collection of fuel types</returns>
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
            return StatusCode(400, new StatusInformationMessage(GenericError));
        }
        catch (Exception ex)
        {
            logger.LogInformation(ex.Message);
            return StatusCode(403, new StatusInformationMessage(InvalidData));
        }
    }

    /// <summary>
    /// Returns the available vehicle types
    /// </summary>
    /// <returns>Collection of vehicle types</returns>
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
            return StatusCode(400, new StatusInformationMessage(GenericError));
        }
        catch (Exception ex)
        {
            logger.LogInformation(ex.Message);
            return StatusCode(403, new StatusInformationMessage(InvalidData));
        }
    }

    /// <summary>
    /// Creates a new vehicle and adds it to the user vehicle collection
    /// </summary>
    /// <param name="model">The input data containing the vehicle information</param>
    /// <returns>The Id of the created vehicle</returns>
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
            return StatusCode(400, new StatusInformationMessage(GenericError));
        }
        catch (Exception ex)
        {
            logger.LogInformation(ex.Message);
            return StatusCode(403, new StatusInformationMessage(InvalidData));
        }
    }

    /// <summary>
    /// Uploads the vehicle image to Amazon S3 and adds a relation to the vehicle
    /// </summary>
    /// <param name="vehicleId">The vehicle Id</param>
    /// <param name="file">The image file for the vehicle</param>
    /// <returns></returns>
    [HttpPost]
    [Route("/VehicleImageUpload")]
    [Produces("application/json")]
    public async Task<IActionResult> UploadVehicleImage([FromHeader] string vehicleId,[FromForm] IFormFile file)
    {
        try
        {
            if (file.Length == 0 || file == null)
            {
                return StatusCode(400, new StatusInformationMessage(InvalidData));
            }

            if(file.ContentType != "image/jpeg")
            {
                return StatusCode(415, new StatusInformationMessage(InvalidData));
            }

            if(file.Length/1024 > 2048)
            {
                return StatusCode(413, new StatusInformationMessage(FileSizeTooBig));
            }

            string imageId = await imageService.UploadVehicleImage(file);

            await vehicleService.AddImageToVehicle(vehicleId, imageId);
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
            return StatusCode(403, new StatusInformationMessage(GenericError));
        }

    }

}
