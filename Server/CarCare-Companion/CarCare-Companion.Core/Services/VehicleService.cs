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

    public async Task<ICollection<FuelTypeRequestModel>> GetAllFuelTypesAsync()
    {
        return await repository.AllReadonly<FuelType>()
               .Select(ft => new FuelTypeRequestModel
               {
                   Id = ft.Id,
                   Name = ft.Name,
               })
               .ToListAsync();
    }

    public async Task<ICollection<VehicleTypeRequestModel>> GetAllVehicleTypesAsync()
    {
        return await repository.AllReadonly<VehicleType>()
             .Select(ft => new VehicleTypeRequestModel
             {
                 Id = ft.Id,
                 Name = ft.Name,
             })
             .ToListAsync();
    }
}
