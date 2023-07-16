namespace CarCare_Companion.Core.Services;

using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using CarCare_Companion.Core.Contracts;
using CarCare_Companion.Core.Models.Vehicle;
using CarCare_Companion.Infrastructure.Data.Common;
using CarCare_Companion.Infrastructure.Data.Models.Vehicle;
using CarCare_Companion.Infrastructure.Data.Models.Contracts;

/// <summary>
/// The VehicleService is responsible for operations regarding the vehicle-related actions
/// </summary>
public class VehicleService : IVehicleService
{
    private readonly IRepository repository;

    public VehicleService(IRepository repository)
    {
        this.repository = repository;
    }

    /// <summary>
    /// Creates a new vehicle
    /// </summary>
    /// <param name="model">The input model containing the vehicle information</param>
    /// <returns>String containing the newly created vehicle Id</returns>
    public async Task<string> CreateVehicleAsync(string userId, VehicleFormRequestModel model)
    {
        Vehicle newVehicle = new Vehicle()
        {
            Make = model.Make,
            Model = model.Model,
            Mileage = model.Mileage,
            FuelTypeId = model.FuelTypeId,
            VehicleTypeId = model.VehicleTypeId,
            Year = model.Year,
            OwnerId = Guid.Parse(userId),
            CreatedOn = DateTime.UtcNow
        };

        await repository.AddAsync(newVehicle);
        await repository.SaveChangesAsync();

        return newVehicle.Id.ToString();
    }

    /// <summary>
    /// Edit a existing vehicle
    /// </summary>
    /// <param name="vehicleId">The vehicle identifier</param>
    /// <param name="model">The input model containing the vehicle information</param>
    public async Task EditVehicleAsync(string vehicleId, VehicleFormRequestModel model)
    {
        Vehicle vehicleToEdit = await repository.GetByIdAsync<Vehicle>(Guid.Parse(vehicleId));

        vehicleToEdit.Make = model.Make;
        vehicleToEdit.Model = model.Model;
        vehicleToEdit.Mileage = model.Mileage;
        vehicleToEdit.Year = model.Year;
        vehicleToEdit.FuelTypeId = model.FuelTypeId;
        vehicleToEdit.VehicleTypeId = model.VehicleTypeId;
        vehicleToEdit.ModifiedOn = DateTime.UtcNow;

        await repository.SaveChangesAsync();
    }


    /// <summary>
    /// Adds an image url to the vehicle
    /// </summary>
    /// <param name="vehicleId">The vehicle Id</param>
    /// <param name="imageId">The image Id</param>
    /// <returns>A boolean regarding the adding process</returns>
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


    /// <summary>
    /// Checks if the fuel type exists
    /// </summary>
    /// <param name="id">The fuel identifier</param>
    /// <returns>Boolean based on the search result</returns>
    public async Task<bool> DoesFuelTypeExistAsync(int id)
    {
        var result = await repository.GetByIdAsync<FuelType>(id);

        if(result !=  null)
        {
            return true;
        }

        return false;
               
    }

    /// <summary>
    /// Checks if the vehicle type exists
    /// </summary>
    /// <param name="id">The vehicle identifier</param>
    /// <returns>Boolean based on the search result</returns>
    public async Task<bool> DoesVehicleTypeExistAsync(int id)
    {
        var result = await repository.GetByIdAsync<VehicleType>(id);

        if (result != null)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Takes all available fuel types
    /// </summary>
    /// <returns>Collection of fuel types</returns>
    public async Task<ICollection<FuelTypeResponseModel>> AllFuelTypesAsync()
    {
        return await repository.AllReadonly<FuelType>()
               .Select(ft => new FuelTypeResponseModel
               {
                   Id = ft.Id,
                   Name = ft.Name,
               })
               .ToListAsync();
    }

    /// <summary>
    /// Takes all available vehicle types
    /// </summary>
    /// <returns>Collection of vehicle types</returns>
    public async Task<ICollection<VehicleTypeResponseModel>> AllVehicleTypesAsync()
    {
        return await repository.AllReadonly<VehicleType>()
             .Select(ft => new VehicleTypeResponseModel
             {
                 Id = ft.Id,
                 Name = ft.Name,
             })
             .ToListAsync();
    }

    /// <summary>
    /// Takes all user vehicles ordered by date of their creation
    /// </summary>
    /// <param name="userId">The user identifier</param>
    /// <returns>A collection of vehicles with basic information about them</returns>
    public async Task<ICollection<VehicleBasicInfoResponseModel>> AllUserVehiclesByIdAsync(string userId)
    {
         return await repository.AllReadonly<Vehicle>()
               .Where(v => v.IsDeleted == false)
               .Where(v => v.OwnerId == Guid.Parse(userId))
               .OrderBy(v => v.CreatedOn)
               .Select(v => new VehicleBasicInfoResponseModel
               {
                   Id = v.Id.ToString(),
                   Make = v.Make,
                   Model = v.Model,
                   ImageUrl = v.VehicleImageKey.ToString()
               })
               .ToListAsync();
    }

    /// <summary>
    /// Retrieves the vehicle details to the user
    /// </summary>
    /// <param name="vehicleId">The vehicle identifier</param>
    /// <returns>Detailed model containing all the vehicle information</returns>
    public async Task<VehicleDetailsResponseModel> GetVehicleDetails(string vehicleId)
    {
        return await repository.AllReadonly<Vehicle>()
               .Where(v => v.Id == Guid.Parse(vehicleId))
               .Select(v => new VehicleDetailsResponseModel
               {
                   Id = v.Id.ToString(),
                   Make = v.Make,
                   Model = v.Model,
                   Mileage = v.Mileage,
                   Year = v.Year,
                   ImageUrl = v.VehicleImageKey.ToString(),
                   FuelType = v.FuelType.Name,
                   VehicleType = v.VehicleType.Name
               })
               .FirstAsync();
                                         
    }

    /// <summary>
    /// Retrieves the vehicle edit details to the user
    /// </summary>
    /// <param name="vehicleId">The vehicle identifier</param>
    /// <returns>A less detailed model containing the vehicle information for editing</returns>
    public async Task<VehicleDetailsEditResponseModel> GetVehicleEditDetails(string vehicleId)
    {
        return await repository.AllReadonly<Vehicle>()
               .Where(v => v.Id == Guid.Parse(vehicleId))
               .Select(v => new VehicleDetailsEditResponseModel
               {
                   Id = v.Id.ToString(),
                   Make = v.Make,
                   Model = v.Model,
                   Mileage = v.Mileage,
                   Year = v.Year,
                   FuelTypeId = v.FuelTypeId,
                   VehicleTypeId = v.VehicleTypeId
               })
               .FirstAsync();

    }

    /// <summary>
    /// Checks if the vehicle with the passed id exists
    /// </summary>
    /// <param name="id">The vehicle identifier</param>
    /// <returns>Boolean based on the search result</returns>
    public async Task<bool> DoesVehicleExistByIdAsync(string id)
    {
        return await repository.AllReadonly<Vehicle>().
                     Where(v => v.Id == Guid.Parse(id))
                     .AnyAsync();
    }

    /// <summary>
    /// Checks if the user is the owner of the vehicle
    /// </summary>
    /// <param name="userId">The user identifier</param>
    /// <param name="vehicleId">The vehicle identifier</param>
    /// <returns>Boolean based on the search result</returns>
    public async Task<bool> IsUserOwnerOfVehicleAsync(string userId, string vehicleId)
    {
        return await repository.AllReadonly<Vehicle>().
                    Where(v => v.Id == Guid.Parse(vehicleId) && v.OwnerId == Guid.Parse(userId))
                    .AnyAsync();    
    }

    /// <summary>
    /// Deletes a vehicle and all of its records
    /// </summary>
    /// <param name="vehicleId">The vehicle identifier</param>
    public async Task DeleteVehicleAsync(string vehicleId)
    {
        Vehicle vehicleToDelete = await repository.All<Vehicle>().
                                  Where(v => v.Id == Guid.Parse(vehicleId))
                                  .Include(v => v.TaxRecords)
                                  .AsSplitQuery()
                                  .Include(v => v.TripRecords)
                                  .AsSplitQuery()
                                  .Include(v => v.ServiceRecords)
                                  .AsSplitQuery()
                                  .FirstAsync();


        repository.SoftDelete(vehicleToDelete);

        foreach(var serviceRecord in vehicleToDelete.ServiceRecords)
        {
            repository.SoftDelete(serviceRecord);
        }

        foreach(var tripRecord in vehicleToDelete.TripRecords)
        {
            repository.SoftDelete(tripRecord);
        }

        foreach (var taxRecord in vehicleToDelete.TaxRecords)
        {
            repository.SoftDelete(taxRecord);
        }

        await repository.SaveChangesAsync();
    }

}
