namespace CarCare_Companion.Core.Services;

using CarCare_Companion.Core.Contracts;
using CarCare_Companion.Core.Models.Admin.Users;
using CarCare_Companion.Infrastructure.Data.Common;
using CarCare_Companion.Infrastructure.Data.Models.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

public class UserService : IUserService
{
    private readonly IRepository repository;

    public UserService(IRepository repository)
    {
        this.repository = repository;
    }

    public async Task<ICollection<UserInformationResponseModel>> GetAllUsersAsync()
    {
        return await repository.AllReadonly<ApplicationUser>()
            .Select(au => new UserInformationResponseModel()
            {
                UserId = au.Id.ToString(),
                FirstName = au.FirstName,
                LastName = au.LastName,
                Username = au.UserName,
                VehiclesCount = au.Vehicles.Count(),
                ServiceRecordsCount = au.ServiceRecords.Count(),
                TaxRecordsCount = au.TaxRecords.Count(),
                TripsCount = au.TripRecords.Count()
            })
            .ToListAsync();
    }
}
