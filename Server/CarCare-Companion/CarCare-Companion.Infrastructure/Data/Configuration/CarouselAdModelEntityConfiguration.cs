namespace CarCare_Companion.Infrastructure.Data.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using CarCare_Companion.Infrastructure.Data.Models.Ads;

using static Seeding.EntityGenerator;

internal class CarouselAdModelEntityConfiguration : IEntityTypeConfiguration<CarouselAdModel>
{
    public void Configure(EntityTypeBuilder<CarouselAdModel> builder)
    {
        builder.HasData(GenerateCarouselAdModels());
    }
}
