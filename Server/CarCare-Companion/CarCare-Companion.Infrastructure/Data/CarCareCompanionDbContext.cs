namespace CarCare_Companion.Infrastructure.Data
{
    using System.Reflection;

    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    using CarCare_Companion.Infrastructure.Data.Models.Ads;
    using CarCare_Companion.Infrastructure.Data.Models.Identity;
    using CarCare_Companion.Infrastructure.Data.Models.Records;
    using CarCare_Companion.Infrastructure.Data.Models.Vehicle;
 

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
        public DbSet<CarouselAdModel> CarouselAdModels { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(typeof(CarCareCompanionDbContext)) ??  Assembly.GetExecutingAssembly());

            var entityTypes = builder.Model.GetEntityTypes().ToList();

            var foreignKeys = entityTypes
                .SelectMany(e => e.GetForeignKeys().Where(f => f.DeleteBehavior == DeleteBehavior.Cascade));
            foreach (var foreignKey in foreignKeys)
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }

            base.OnModelCreating(builder);         
        }
    }
}