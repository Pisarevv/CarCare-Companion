namespace CarCare_Companion.Core.Services;

using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using CarCare_Companion.Core.Contracts;
using CarCare_Companion.Core.Models.Vehicle;
using CarCare_Companion.Infrastructure.Data.Common;
using CarCare_Companion.Infrastructure.Data.Models.Vehicle;


public class VehicleService : IVehicleService
{
    private readonly IRepository repository;

    public VehicleService(IRepository repository)
    {
        this.repository = repository;
    }

    public async Task<bool> AddImageToVehicle(string vehicleId, string imageId)
    {
        Vehicle vehicle = await repository.GetByIdAsync<Vehicle>(Guid.Parse(vehicleId));
        if (vehicle != null)
        {
           vehicle.VehicleImageKey = Guid.Parse(imageId);
           await  repository.SaveChangesAsync();  
           return true;
        }

        return false;
    }

    public async Task<string> CreateVehicleAsync(VehicleCreateRequestModel model)
    {
        Vehicle newVehicle = new Vehicle()
        {
            Make = model.Make,
            Model = model.Model,
            Mileage = model.Mileage,
            FuelTypeId = model.FuelTypeId,
            VehicleTypeId = model.VehicleTypeId,
            Year = model.Year,
            OwnerId = model.OwnerId,
            CreatedOn = DateTime.UtcNow
        };

        await repository.AddAsync(newVehicle);
        await repository.SaveChangesAsync();

        return newVehicle.Id.ToString();
    }



    public async Task<bool> DoesFuelTypeExistAsync(int id)
    {
        var result = await repository.GetByIdAsync<FuelType>(id);

        if(result !=  null)
        {
            return true;
        }

        return false;
               
    }

    public async Task<bool> DoesVehicleTypeExistAsync(int id)
    {
        var result = await repository.GetByIdAsync<VehicleType>(id);

        if (result != null)
        {
            return true;
        }

        return false;
    }

    public async Task<ICollection<FuelTypeResponseModel>> GetAllFuelTypesAsync()
    {
        return await repository.AllReadonly<FuelType>()
               .Select(ft => new FuelTypeResponseModel
               {
                   Id = ft.Id,
                   Name = ft.Name,
               })
               .ToListAsync();
    }

    public async Task<ICollection<VehicleTypeResponseModel>> GetAllVehicleTypesAsync()
    {
        return await repository.AllReadonly<VehicleType>()
             .Select(ft => new VehicleTypeResponseModel
             {
                 Id = ft.Id,
                 Name = ft.Name,
             })
             .ToListAsync();
    }
}
