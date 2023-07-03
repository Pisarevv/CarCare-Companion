namespace CarCare_Companion.Core.Contracts;

using CarCare_Companion.Core.Models.Vehicle;

public interface IVehicleService
{
    public Task<ICollection<FuelTypeResponseModel>> GetAllFuelTypesAsync();

    public Task<ICollection<VehicleTypeResponseModel>> GetAllVehicleTypesAsync();

    public Task<bool> DoesFuelTypeExistAsync(int id);

    public Task<bool> DoesVehicleTypeExistAsync(int id);

    public Task<string> CreateVehicleAsync(VehicleCreateRequestModel model);

    public Task<bool> AddImageToVehicle(string vehicleId, string imageId);
}
