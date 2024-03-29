﻿using CarCare_Companion.Infrastructure.Data.Models.Contracts;

namespace CarCare_Companion.Core.Models.Search;

public class TaxRecordDetailsQueryResponseModel : ICostable
{
    public string Type { get; } = "TaxRecord";

    public string Id { get; set; } = null!;

    public string Title { get; set; } = null!;

    public DateTime ValidFrom { get; set; }

    public DateTime ValidTo { get; set; }

    public decimal Cost { get; set; }

    public string VehicleMake { get; set; } = null!;

    public string VehicleModel { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime DateCreated { get; set; }
}
