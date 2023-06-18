namespace CarCare_Companion.Infrastructure.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

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