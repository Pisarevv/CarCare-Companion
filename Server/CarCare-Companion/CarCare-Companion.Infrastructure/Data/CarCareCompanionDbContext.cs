using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CarCare_Companion.Infrastructure.Data
{
    public class CarCareCompanionDbContext : IdentityDbContext
    {
        public CarCareCompanionDbContext(DbContextOptions<CarCareCompanionDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);         
        }
    }
}