namespace CarCare_Companion.Core.Contracts;

using CarCare_Companion.Core.Models.Vehicle;

public interface IVehicleService
{
    public Task<ICollection<FuelTypeRequestModel>> GetAllFuelTypesAsync();

    public Task<ICollection<VehicleTypeRequestModel>> GetAllVehicleTypesAsync();
}
