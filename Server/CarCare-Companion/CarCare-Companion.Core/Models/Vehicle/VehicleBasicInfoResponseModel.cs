namespace CarCare_Companion.Core.Models.Vehicle;

public class VehicleBasicInfoResponseModel
{
    public string Id { get; set; } = null!;

    public string Make { get; set; } = null!;

    public string Model { get; set; } = null!;

    public string? ImageUrl { get; set; }
}
