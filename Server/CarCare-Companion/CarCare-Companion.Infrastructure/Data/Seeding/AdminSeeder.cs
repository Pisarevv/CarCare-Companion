namespace CarCare_Companion.Infrastructure.Data.Seeding;

using CarCare_Companion.Common;
using CarCare_Companion.Infrastructure.Data.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

public class AdminSeeder : ISeeder
{
    public async Task SeedAsync(CarCareCompanionDbContext dbContext, IServiceProvider serviceProvider)
    {

        var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        await GenerateAdminProfileAsync(userManager, roleManager, GlobalConstants.AdministratorRoleName);
    }

    private static async Task GenerateAdminProfileAsync(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, string roleName)
    {
        var admin = await userManager.FindByNameAsync("admin@carcare.com");

        if (admin == null)
        {
            const string password = "admin123";

            ApplicationUser adminUser = new ApplicationUser();
            adminUser.UserName = "admin@carcare.com";
            adminUser.FirstName = "admin";
            adminUser.LastName = "admin";
            adminUser.Email = "admin@carcare.com";
            adminUser.SecurityStamp = Guid.NewGuid().ToString();
            adminUser.CreatedOn = DateTime.UtcNow;

            var registerResult = await userManager.CreateAsync(adminUser, password);
            if (!registerResult.Succeeded)
            {
                throw new Exception(string.Join(Environment.NewLine, registerResult.Errors.Select(e => e.Description)));
            }

            var roleResult = await userManager.AddToRoleAsync(adminUser, roleName);

            if (!roleResult.Succeeded)
            {
                throw new Exception(string.Join(Environment.NewLine, roleResult.Errors.Select(e => e.Description)));
            }
        }
        

    }
}
