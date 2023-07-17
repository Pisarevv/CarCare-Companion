﻿namespace CarCare_Companion.Core.Contracts;

using CarCare_Companion.Core.Models.Vehicle;

public interface IVehicleService
{
    public Task<ICollection<FuelTypeResponseModel>> AllFuelTypesAsync();

    public Task<ICollection<VehicleTypeResponseModel>> AllVehicleTypesAsync();

    public Task<bool> DoesFuelTypeExistAsync(int id);

    public Task<bool> DoesVehicleTypeExistAsync(int id);

    public Task<bool> DoesVehicleExistByIdAsync(string id);

    public Task<string> CreateAsync(string userId, VehicleFormRequestModel model);

    public Task EditAsync(string vehicleId, VehicleFormRequestModel model);

    public Task DeleteAsync(string vehicleId);

    public Task<bool> AddImageToVehicle(string vehicleId, string imageId);

    public Task<ICollection<VehicleBasicInfoResponseModel>> AllUserVehiclesByIdAsync(string userId);

    public Task<VehicleDetailsResponseModel> GetVehicleDetails(string vehicleId);

    public Task<VehicleDetailsEditResponseModel> GetVehicleEditDetails(string vehicleId);

    public Task<bool> IsUserOwnerOfVehicleAsync(string  userId, string vehicleId);
}
