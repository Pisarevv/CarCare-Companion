namespace CarCare_Companion.Core.Services;

using CarCare_Companion.Core.Contracts;
using CarCare_Companion.Core.Models.Admin.Users;
using CarCare_Companion.Infrastructure.Data.Common;
using CarCare_Companion.Infrastructure.Data.Models.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Common.GlobalConstants;

public class UserService : IUserService
{
    private readonly IRepository repository;
    private readonly IIdentityService identityService;

    public UserService(IRepository repository, IIdentityService identityService)
    {
        this.repository = repository;
        this.identityService = identityService;
    }



    /// <summary>
    /// Retrieve all the users of the application
    /// </summary>
    /// <returns>Collection of the application users</returns>
    public async Task<ICollection<UserInformationResponseModel>> GetAllUsersAsync()
    {
        return await repository.AllReadonly<ApplicationUser>()
            .Select(au => new UserInformationResponseModel()
            {
                UserId = au.Id.ToString(),            
                Username = au.UserName,
            })
            .ToListAsync();
    }

    /// <summary>
    ///  Retrieve details about a user
    /// </summary>
    /// <param name="userId">The user identifier</param>
    /// <returns>A model containing the user details</returns>
    public async Task<UserDetailsResponseModel?> GetUserDetailsByIdAsync(string userId)
    {
        UserDetailsResponseModel? user = await repository.AllReadonly<ApplicationUser>()
               .Where(au => au.Id == Guid.Parse(userId))
               .Select(au => new UserDetailsResponseModel()
               {
                   UserId = userId,
                   FirstName = au.FirstName,
                   LastName = au.LastName,
                   Username = au.UserName,
                   ServiceRecordsCount = au.ServiceRecords
                                         .Where(sr => sr.IsDeleted == false)
                                         .Count(),
                   TaxRecordsCount = au.TaxRecords
                                     .Where(tr => tr.IsDeleted == false)
                                     .Count(),
                   TripsCount = au.TripRecords
                                .Where(tr => tr.IsDeleted == false)
                                .Count(),
                   VehiclesCount = au.Vehicles
                                   .Where(v => v.IsDeleted == false).Count()
               })
               .FirstOrDefaultAsync();

        if(user == null)
        {
            return null;
        }
        user.IsAdmin = await identityService.IsUserInRole(userId, AdministratorRoleName);

        return user;
    }

    /// <summary>
    /// Retrieves the amount of recently joined users 
    /// </summary>
    /// <param name="days">The amount of days to look back</param>
    /// <returns>An integer containing the count of users</returns>
    public async Task<int> GetRecentlyJoinedUsersCountAsync(int days)
    {
        DateTime filterDateStart = DateTime.UtcNow;
        DateTime filterDateEnd = DateTime.UtcNow.AddDays(-days);

        return await repository.AllReadonly<ApplicationUser>()
                     .Where(ap => ap.CreatedOn >= filterDateStart && ap.CreatedOn <= filterDateEnd)
                     .CountAsync();
    }
}
