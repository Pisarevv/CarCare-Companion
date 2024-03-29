﻿namespace CarCare_Companion.Core.Models.TaxRecords;


public class TaxRecordResponseModel
{
    public string Id { get; set; } = null!;

    public string Title { get; set; } = null!;

    public DateTime ValidFrom { get; set; }

    public DateTime ValidTo { get; set; }

    public decimal Cost { get; set; }

    public string VehicleId { get; set; } = null!;

    public string? Description { get; set; }

}
