namespace CarCare_Companion.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

using CarCare_Companion.Core.Contracts;
using CarCare_Companion.Core.Models.Status;
using CarCare_Companion.Core.Models.Vehicle;

using CarCare_Companion.Common;
using static CarCare_Companion.Common.StatusResponses;

/// <summary>
/// The vehicle controller handles vehicle related operations
/// </summary>
[Route("[controller]")]
public class VehiclesController : BaseController
{
    private readonly IVehicleService vehicleService;
    private readonly IImageService imageService;
    private readonly ILogger<VehiclesController> logger;

    public VehiclesController(IVehicleService vehicleService, IImageService imageService, ILogger<VehiclesController> logger)
    {
        this.vehicleService = vehicleService;
        this.imageService = imageService;
        this.logger = logger;
    }

    /// <summary>
    /// Retrieves all the user vehicles
    /// </summary>
    /// <returns>Collection of the user vehicles</returns>
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(ICollection<VehicleBasicInfoResponseModel>))]
    [ProducesResponseType(400, Type = typeof(StatusInformationMessage))]
    [ProducesResponseType(403, Type = typeof(StatusInformationMessage))]
    public async Task<IActionResult> GetUserVehicles()
    {
        try
        {
            var userId = this.User.GetId();

            if (string.IsNullOrEmpty(userId))
            {
                return StatusCode(403, InvalidUser);
            }

            ICollection<VehicleBasicInfoResponseModel> vehicles = await vehicleService.AllUserVehiclesByIdAsync(userId);
            foreach (var vehicle in vehicles)
            {
                if(vehicle.ImageUrl != null)
                {
                    vehicle.ImageUrl = await imageService.GetImageUrlAsync(vehicle.ImageUrl);
                }
            }
            return StatusCode(200, vehicles);
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
    [ProducesResponseType(200, Type = typeof(string))]
    [ProducesResponseType(400, Type = typeof(StatusInformationMessage))]
    [ProducesResponseType(403, Type = typeof(StatusInformationMessage))]
    public async Task<IActionResult> CreateVehicle([FromBody] VehicleFormRequestModel model)
    {
        try
        {
            var userId = this.User.GetId();

            if (string.IsNullOrEmpty(userId))
            {
                return StatusCode(403, InvalidUser);
            }

            if (!ModelState.IsValid)
            {
                return StatusCode(400, new StatusInformationMessage(InvalidData));
            }
            string vehicleId = await vehicleService.CreateAsync(userId,model);
          
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
    /// Edits a record of a user vehicle
    /// </summary>
    /// <param name="vehicleId">The vehicle identifier</param>
    /// <param name="model">The input data containing the vehicle information</param>
    /// <returns>Status response based on the edit result</returns>
    [HttpPatch]
    [Route("Edit/{vehicleId}")]
    [ProducesResponseType(200, Type = typeof(StatusInformationMessage))]
    [ProducesResponseType(400, Type = typeof(StatusInformationMessage))]
    [ProducesResponseType(403, Type = typeof(StatusInformationMessage))]
    public async Task<IActionResult> EditVehicle([FromRoute] string vehicleId,[FromBody] VehicleFormRequestModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, new StatusInformationMessage(InvalidData));
            }

            bool vehicleExist = await vehicleService.DoesVehicleExistByIdAsync(vehicleId);

            if (!vehicleExist)
            {
                return StatusCode(404, new StatusInformationMessage(ResourceNotFound));
            }

            var userId = this.User.GetId();

            if (string.IsNullOrEmpty(userId))
            {
                return StatusCode(403, InvalidUser);
            }

            bool isUserOwnerOfVehicle = await vehicleService.IsUserOwnerOfVehicleAsync(userId, vehicleId);


            if (!isUserOwnerOfVehicle)
            {
                return StatusCode(403, new StatusInformationMessage(NoPermission));
            }

            await vehicleService.EditAsync(vehicleId, model);

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
    /// Deletes a vehicle and all of its records
    /// </summary>
    /// <param name="vehicleId">The vehicle identifier</param>
    /// <returns>A status code with message based on the process of deleting </returns>
    [HttpDelete]
    [Route("Delete/{vehicleId}")]
    [ProducesResponseType(200, Type = typeof(StatusInformationMessage))]
    [ProducesResponseType(400, Type = typeof(StatusInformationMessage))]
    [ProducesResponseType(403, Type = typeof(StatusInformationMessage))]
    public async Task<IActionResult> Delete([FromRoute] string vehicleId)
    {
        try
        {
            var userId = this.User.GetId();

            if (string.IsNullOrEmpty(userId))
            {
                return StatusCode(403, InvalidUser);
            }

            bool isUserOwner = await vehicleService.IsUserOwnerOfVehicleAsync(userId,vehicleId);

            if (!isUserOwner)
            {
                return StatusCode(403, InvalidUser);
            }
           
            await vehicleService.DeleteAsync(vehicleId);

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
    /// Uploads the vehicle image to Amazon S3 and adds a relation to the vehicle
    /// </summary>
    /// <param name="vehicleId">The vehicle identifier</param>
    /// <param name="file">The image file for the vehicle</param>
    /// <returns></returns>
    [HttpPost]
    [Route("ImageUpload")]
    [ProducesResponseType(200, Type = typeof(StatusInformationMessage))]
    [ProducesResponseType(400, Type = typeof(StatusInformationMessage))]
    [ProducesResponseType(403, Type = typeof(StatusInformationMessage))]
    public async Task<IActionResult> UploadVehicleImage([FromHeader] string vehicleId,[FromForm] IFormFile file)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, new StatusInformationMessage(InvalidData));
            }

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

            bool isAdded = await vehicleService.AddImageToVehicle(vehicleId, imageId);

            if (isAdded) 
            {
                return StatusCode(200, new StatusInformationMessage(Success));
            }

            logger.LogInformation("Attempt to adding an image to non existent car has occurred");
            return StatusCode(400, new StatusInformationMessage(InvalidData));


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


    /// <summary>
    /// Retrieves detailed vehicle  information
    /// </summary>
    /// <param name="vehicleId">The vehicle identifier</param>
    /// <returns>Model containing all the vehicle details</returns>
    [HttpGet]
    [Route("Details/{vehicleId}")]
    [ProducesResponseType(200, Type = typeof(VehicleDetailsResponseModel))]
    [ProducesResponseType(400, Type = typeof(StatusInformationMessage))]
    [ProducesResponseType(403, Type = typeof(StatusInformationMessage))]
    public async Task<IActionResult> VehicleDetails([FromRoute] string vehicleId)
    {
        try
        {
            bool vehicleExist = await vehicleService.DoesVehicleExistByIdAsync(vehicleId);

            if (!vehicleExist)
            {
                return StatusCode(404, new StatusInformationMessage(ResourceNotFound));
            }

            var userId = this.User.GetId();

            if (string.IsNullOrEmpty(userId))
            {
                return StatusCode(403, InvalidUser);
            }

            bool isUserOwnerOfVehicle = await vehicleService.IsUserOwnerOfVehicleAsync(userId,vehicleId);


            if (!isUserOwnerOfVehicle)
            {
                return StatusCode(403, new StatusInformationMessage(NoPermission));
            }

            VehicleDetailsResponseModel vehicle = await vehicleService.GetVehicleDetailsByIdAsync(vehicleId);

            if(vehicle.ImageUrl != null)
            {
                vehicle.ImageUrl = await imageService.GetImageUrlAsync(vehicle.ImageUrl);
            }

            return StatusCode(200, vehicle);

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
    /// Retrieves detailed vehicle  information
    /// </summary>
    /// <param name="vehicleId">The vehicle identifier</param>
    /// <returns>Model containing all the vehicle details needed</returns>
    [HttpGet]
    [Route("Edit/{vehicleId}")]
    [ProducesResponseType(200, Type = typeof(VehicleDetailsEditResponseModel))]
    [ProducesResponseType(400, Type = typeof(StatusInformationMessage))]
    [ProducesResponseType(403, Type = typeof(StatusInformationMessage))]
    public async Task<IActionResult> VehicleEditDetails([FromRoute] string vehicleId)
    {
        try
        {
            bool vehicleExist = await vehicleService.DoesVehicleExistByIdAsync(vehicleId);

            if (!vehicleExist)
            {
                return StatusCode(404, new StatusInformationMessage(ResourceNotFound));
            }

            var userId = this.User.GetId();

            if (string.IsNullOrEmpty(userId))
            {
                return StatusCode(403, InvalidUser);
            }

            bool isUserOwnerOfVehicle = await vehicleService.IsUserOwnerOfVehicleAsync(userId, vehicleId);


            if (!isUserOwnerOfVehicle)
            {
                return StatusCode(403, new StatusInformationMessage(NoPermission));
            }

            VehicleDetailsEditResponseModel vehicle = await vehicleService.GetVehicleEditDetails(vehicleId);

            return StatusCode(200, vehicle);

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
    /// Returns the available fuel types
    /// </summary>
    /// <returns>Collection of fuel types</returns>
    [HttpGet]
    [Route("FuelTypes")]
    [ProducesResponseType(200, Type = typeof(ICollection<VehicleTypeResponseModel>))]
    [ProducesResponseType(400, Type = typeof(StatusInformationMessage))]
    [ProducesResponseType(403, Type = typeof(StatusInformationMessage))]
    public async Task<IActionResult> GetFuelTypes()
    {
        try
        {
            ICollection<FuelTypeResponseModel> fuelTypes = await vehicleService.AllFuelTypesAsync();
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
    [Route("Types")]
    [ProducesResponseType(200, Type = typeof(ICollection<VehicleTypeResponseModel>))]
    [ProducesResponseType(400, Type = typeof(StatusInformationMessage))]
    [ProducesResponseType(403, Type = typeof(StatusInformationMessage))]
    public async Task<IActionResult> GetVehicleTypes()
    {
        try
        {
            ICollection<VehicleTypeResponseModel> vehicleTypes = await vehicleService.AllVehicleTypesAsync();

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


}
