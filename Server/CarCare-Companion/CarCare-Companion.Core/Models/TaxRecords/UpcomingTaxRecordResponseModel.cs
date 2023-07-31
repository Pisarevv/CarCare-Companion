namespace CarCare_Companion.Core.Models.TaxRecords;
using System;


public class UpcomingTaxRecordResponseModel
{
    public string Id { get; set; } = null!;

    public string Title { get; set; } = null!;

    public DateTime ValidTo { get; set; }

    public string VehicleMake { get; set; } = null!;

    public string VehicleModel { get; set; } = null!;

}
