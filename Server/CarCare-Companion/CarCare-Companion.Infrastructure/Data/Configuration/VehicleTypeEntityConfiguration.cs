namespace CarCare_Companion.Infrastructure.Data.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using CarCare_Companion.Infrastructure.Data.Models.Vehicle;

using static Seeding.EntityGenerator;

internal class VehicleTypeEntityConfiguration : IEntityTypeConfiguration<VehicleType>
{
    public void Configure(EntityTypeBuilder<VehicleType> builder)
    {
        builder.HasData(GenerateVehicleTypes());
    }
}
