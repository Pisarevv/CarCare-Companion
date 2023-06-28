namespace CarCare_Companion.Infrastructure.Data.Models.Ads;

using CarCare_Companion.Infrastructure.Data.Models.BaseModels;
using System.ComponentModel.DataAnnotations;

using static CarCare_Companion.Common.ValidationConstants.CarouselAdModel;

public class CarouselAdModel : BaseDeletableModel<CarouselAdModel>
{
    [Required]
    [MinLength(MinNameLength)]
    [MaxLength(MaxNameLength)]
    public string UserFirstName { get; set; } = null!;

    [Required]
    [MinLength(MinDescriptionLength)]
    [MaxLength(MaxDescriptionLength)]
    public string Description { get; set; } = null!;

    [Required]
    [Range(MinStarsRating, MaxStarsRating)]
    public int StarsRating { get; set; }

} 
