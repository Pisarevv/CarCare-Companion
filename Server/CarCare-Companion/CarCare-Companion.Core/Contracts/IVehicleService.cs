namespace CarCare_Companion.Core.Contracts;

using CarCare_Companion.Core.Models.Vehicle;

public interface IVehicleService
{
    /// <summary>
    /// Retrieves all fuel types asynchronously.
    /// </summary>
    /// <returns>A collection of FuelTypeResponseModels representing the fuel types.</returns>
    public Task<ICollection<FuelTypeResponseModel>> AllFuelTypesAsync();

    /// <summary>
    /// Retrieves all vehicle types asynchronously.
    /// </summary>
    /// <returns>A collection of VehicleTypeResponseModels representing the vehicle types.</returns>
    public Task<ICollection<VehicleTypeResponseModel>> AllVehicleTypesAsync();

    /// <summary>
    /// Checks if a specific fuel type exists based on its ID asynchronously.
    /// </summary>
    /// <param name="id">The ID of the fuel type to check.</param>
    /// <returns>True if the fuel type exists, otherwise false.</returns>
    public Task<bool> DoesFuelTypeExistAsync(int id);

    /// <summary>
    /// Checks if a specific vehicle type exists based on its ID asynchronously.
    /// </summary>
    /// <param name="id">The ID of the vehicle type to check.</param>
    /// <returns>True if the vehicle type exists, otherwise false.</returns>
    public Task<bool> DoesVehicleTypeExistAsync(int id);

    /// <summary>
    /// Checks if a specific vehicle exists based on its ID asynchronously.
    /// </summary>
    /// <param name="id">The ID of the vehicle to check.</param>
    /// <returns>True if the vehicle exists, otherwise false.</returns>
    public Task<bool> DoesVehicleExistByIdAsync(string id);

    /// <summary>
    /// Creates a new vehicle record for a user asynchronously.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="model">The model containing the vehicle details.</param>
    /// <returns>The created VehicleResponseModel.</returns>
    public Task<VehicleResponseModel> CreateAsync(string userId, VehicleFormRequestModel model);

    /// <summary>
    /// Edits an existing vehicle record for a user asynchronously.
    /// </summary>
    /// <param name="vehicleId">The ID of the vehicle to edit.</param>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="model">The model containing the updated vehicle details.</param>
    /// <returns>The edited VehicleResponseModel.</returns>
    public Task<VehicleResponseModel> EditAsync(string vehicleId, string userId, VehicleFormRequestModel model);

    /// <summary>
    /// Deletes a vehicle record based on its ID and the associated user's ID asynchronously.
    /// </summary>
    /// <param name="vehicleId">The ID of the vehicle to delete.</param>
    /// <param name="userId">The ID of the associated user.</param>
    public Task DeleteAsync(string vehicleId, string userId);

    /// <summary>
    /// Adds an image to a specific vehicle record asynchronously.
    /// </summary>
    /// <param name="vehicleId">The ID of the vehicle.</param>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="imageId">The ID of the image to add.</param>
    /// <returns>True if the operation is successful, otherwise false.</returns>
    public Task<bool> AddImageToVehicle(string vehicleId, string userId, string imageId);

    /// <summary>
    /// Retrieves all vehicles associated with a specific user ID asynchronously.
    /// </summary>
    /// <param name="userId">The user's ID.</param>
    /// <returns>A collection of VehicleBasicInfoResponseModels.</returns>
    public Task<ICollection<VehicleBasicInfoResponseModel>> AllUserVehiclesByIdAsync(string userId);

    /// <summary>
    /// Retrieves detailed information of a specific vehicle based on its ID asynchronously.
    /// </summary>
    /// <param name="vehicleId">The ID of the vehicle to retrieve details for.</param>
    /// <returns>The VehicleDetailsResponseModel containing the vehicle's details.</returns>
    public Task<VehicleDetailsResponseModel> GetVehicleDetailsByIdAsync(string vehicleId);

    /// <summary>
    /// Retrieves the edit details of a specific vehicle based on its ID asynchronously.
    /// </summary>
    /// <param name="vehicleId">The ID of the vehicle to retrieve edit details for.</param>
    /// <returns>The VehicleDetailsEditResponseModel containing the vehicle's edit details.</returns>
    public Task<VehicleDetailsEditResponseModel> GetVehicleEditDetailsAsync(string vehicleId);

    /// <summary>
    /// Verifies if a user is the owner of a specific vehicle record asynchronously.
    /// </summary>
    /// <param name="userId">The user's ID.</param>
    /// <param name="vehicleId">The ID of the vehicle to check.</param>
    /// <returns>True if the user is the owner of the vehicle, otherwise false.</returns>
    public Task<bool> IsUserOwnerOfVehicleAsync(string userId, string vehicleId);
}

