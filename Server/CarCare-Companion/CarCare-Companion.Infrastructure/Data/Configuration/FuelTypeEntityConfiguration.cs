namespace CarCare_Companion.Infrastructure.Data.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using CarCare_Companion.Infrastructure.Data.Models.Vehicle;

using static Seeding.EntityGenerator;

internal class FuelTypeEntityConfiguration : IEntityTypeConfiguration<FuelType>
{
    public void Configure(EntityTypeBuilder<FuelType> builder)
    {
        builder.HasData(GenerateFuelTypes());
    }
}
