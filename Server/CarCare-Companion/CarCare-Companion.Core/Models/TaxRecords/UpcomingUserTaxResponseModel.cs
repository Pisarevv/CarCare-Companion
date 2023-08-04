namespace CarCare_Companion.Core.Models.TaxRecords;


public class UpcomingUserTaxResponseModel
{
    public string Email { get; set; } = null!;

    public string? FirstName { get; set; }

    public string? LastName { get; set;}

    public string TaxName { get; set; } = null!;

    public string TaxValidTo { get; set; } = null!;

    public string VehicleMake { get; set; } = null!;

    public string VehicleModel { get; set; } = null!;
}
