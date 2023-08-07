namespace CarCare_Companion.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

using CarCare_Companion.Core.Contracts;
using CarCare_Companion.Core.Models.Status;
using CarCare_Companion.Core.Models.Vehicle;

using CarCare_Companion.Common;


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
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(403, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> GetUserVehicles()
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

            ICollection<VehicleBasicInfoResponseModel> vehicles = await vehicleService.AllUserVehiclesByIdAsync(userId);
           

            return StatusCode(200, vehicles);
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
    /// Creates a new vehicle and adds it to the user vehicle collection
    /// </summary>
    /// <param name="model">The input data containing the vehicle information</param>
    /// <returns>The Id of the created vehicle</returns>
    [HttpPost]
    [ProducesResponseType(200, Type = typeof(VehicleFormRequestModel))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(403, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Create([FromBody] VehicleFormRequestModel model)
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

            string vehicleId = await vehicleService.CreateAsync(userId,model);
          
            return StatusCode(200, vehicleId);

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
    /// Edits a record of a user vehicle
    /// </summary>
    /// <param name="vehicleId">The vehicle identifier</param>
    /// <param name="model">The input data containing the vehicle information</param>
    /// <returns>Status response based on the edit result</returns>
    [HttpPatch]
    [Route("Edit/{vehicleId}")]
    [ProducesResponseType(200, Type = typeof(VehicleFormRequestModel))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(403, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Edit([FromRoute] string vehicleId,[FromBody] VehicleFormRequestModel model)
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

            bool vehicleExist = await vehicleService.DoesVehicleExistByIdAsync(vehicleId);

            if (!vehicleExist)
            {
                return StatusCode(403, new ProblemDetails
                {
                    Title = StatusResponses.InvalidUser
                });
            }

            bool isUserOwnerOfVehicle = await vehicleService.IsUserOwnerOfVehicleAsync(userId, vehicleId);

            if (!isUserOwnerOfVehicle)
            {
                return StatusCode(403, new ProblemDetails
                {
                    Title = StatusResponses.InvalidUser
                });
            }

            await vehicleService.EditAsync(vehicleId, userId, model);

            return StatusCode(200, model);

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
    /// Deletes a vehicle and all of its records
    /// </summary>
    /// <param name="vehicleId">The vehicle identifier</param>
    /// <returns>A status code with message based on the process of deleting </returns>
    [HttpDelete]
    [Route("Delete/{vehicleId}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(403, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Delete([FromRoute] string vehicleId)
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

            bool isUserOwner = await vehicleService.IsUserOwnerOfVehicleAsync(userId,vehicleId);

            if (!isUserOwner)
            {
                return StatusCode(403, new ProblemDetails
                {
                    Title = StatusResponses.InvalidUser
                });
            }
           
            await vehicleService.DeleteAsync(vehicleId, userId);

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
    /// Uploads the vehicle image to Amazon S3 and adds a relation to the vehicle
    /// </summary>
    /// <param name="vehicleId">The vehicle identifier</param>
    /// <param name="file">The image file for the vehicle</param>
    /// <returns></returns>
    [HttpPost]
    [Route("ImageUpload")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(403, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> UploadVehicleImage([FromHeader] string vehicleId,[FromForm] IFormFile file)
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

            if (file.Length == 0 || file == null)
            {
                return StatusCode(403, new ProblemDetails
                {
                    Title = StatusResponses.InvalidData
                });
            }

            if(file.ContentType != "image/jpeg")
            {
                return StatusCode(415, new ProblemDetails
                {
                    Title = StatusResponses.InvalidData
                });
            }

            if(file.Length/1024 > 2048)
            {
                return StatusCode(413, new ProblemDetails
                {
                    Title = StatusResponses.FileSizeTooBig
                });
            }

            string imageId = await imageService.UploadVehicleImage(file);

            await vehicleService.AddImageToVehicle(vehicleId, userId ,imageId);

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
    /// Retrieves detailed vehicle  information
    /// </summary>
    /// <param name="vehicleId">The vehicle identifier</param>
    /// <returns>Model containing all the vehicle details</returns>
    [HttpGet]
    [Route("Details/{vehicleId}")]
    [ProducesResponseType(200, Type = typeof(VehicleDetailsResponseModel))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(403, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> VehicleDetails([FromRoute] string vehicleId)
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

            bool vehicleExist = await vehicleService.DoesVehicleExistByIdAsync(vehicleId);

            if (!vehicleExist)
            {
                return StatusCode(403, new ProblemDetails
                {
                    Title = StatusResponses.InvalidUser
                });
            }

            bool isUserOwnerOfVehicle = await vehicleService.IsUserOwnerOfVehicleAsync(userId,vehicleId);

            if (!isUserOwnerOfVehicle)
            {
                return StatusCode(403, new ProblemDetails
                {
                    Title = StatusResponses.InvalidUser
                });
            }

            VehicleDetailsResponseModel vehicle = await vehicleService.GetVehicleDetailsByIdAsync(vehicleId);

            return StatusCode(200, vehicle);

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
    /// Retrieves detailed vehicle  information
    /// </summary>
    /// <param name="vehicleId">The vehicle identifier</param>
    /// <returns>Model containing all the vehicle details needed</returns>
    [HttpGet]
    [Route("Edit/{vehicleId}")]
    [ProducesResponseType(200, Type = typeof(VehicleDetailsEditResponseModel))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(403, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> VehicleEditDetails([FromRoute] string vehicleId)
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

            bool vehicleExist = await vehicleService.DoesVehicleExistByIdAsync(vehicleId);

            if (!vehicleExist)
            {
                return StatusCode(403, new ProblemDetails
                {
                    Title = StatusResponses.InvalidUser
                });
            }

            bool isUserOwnerOfVehicle = await vehicleService.IsUserOwnerOfVehicleAsync(userId, vehicleId);

            if (!isUserOwnerOfVehicle)
            {
                return StatusCode(403, new ProblemDetails
                {
                    Title = StatusResponses.InvalidUser
                });
            }

            VehicleDetailsEditResponseModel vehicle = await vehicleService.GetVehicleEditDetails(vehicleId);

            return StatusCode(200, vehicle);

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
    /// Returns the available fuel types
    /// </summary>
    /// <returns>Collection of fuel types</returns>
    [HttpGet]
    [Route("FuelTypes")]
    [ProducesResponseType(200, Type = typeof(ICollection<VehicleTypeResponseModel>))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(403, Type = typeof(ProblemDetails))]
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
    /// Returns the available vehicle types
    /// </summary>
    /// <returns>Collection of vehicle types</returns>
    [HttpGet]
    [Route("Types")]
    [ProducesResponseType(200, Type = typeof(ICollection<VehicleTypeResponseModel>))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(403, Type = typeof(ProblemDetails))]
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
