  namespace CarCare_Companion.Infrastructure.Data
{
    using CarCare_Companion.Infrastructure.Data.Models.Identity;
    using CarCare_Companion.Infrastructure.Data.Models.Records;
    using CarCare_Companion.Infrastructure.Data.Models.Vehicle;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using System.Reflection;

    public class CarCareCompanionDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public CarCareCompanionDbContext(DbContextOptions<CarCareCompanionDbContext> options)
            : base(options)
        {
        }

        public DbSet<Vehicle> Vehicles { get; set; } = null!;
        public DbSet<VehicleType> VehicleTypes { get; set; } = null!;
        public DbSet<FuelType> FuelTypes { get; set; } = null!;
        public DbSet<ServiceRecord> ServiceRecords { get; set; } = null!;
        public DbSet<TripRecord> TripRecords { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(typeof(CarCareCompanionDbContext)) ??  Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);         
        }
    }
}