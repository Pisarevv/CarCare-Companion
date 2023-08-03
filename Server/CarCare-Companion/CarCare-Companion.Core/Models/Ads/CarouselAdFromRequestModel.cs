namespace CarCare_Companion.Core.Models.Ads;

using System.ComponentModel.DataAnnotations;

using static Common.ValidationConstants.CarouselAdModel;

public class CarouselAdFromRequestModel
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
