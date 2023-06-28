﻿namespace CarCare_Companion.Core.Models.Ads;

public class CarouselAdRequestModel
{
    public string Id { get; set; } = null!;

    public string UserFirstName { get; set; } = null!;
      
    public string Description { get; set; } = null!;

    public int ReviewStars { get; set; }
}
