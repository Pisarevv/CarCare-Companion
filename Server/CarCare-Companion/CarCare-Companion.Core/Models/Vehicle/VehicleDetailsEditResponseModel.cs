namespace CarCare_Companion.Core.Models.Vehicle;

public class VehicleDetailsEditResponseModel
{
    public string Id { get; set; } = null!;

    public string Make { get; set; } = null!;

    public string Model { get; set; } = null!;

    public double Mileage { get; set; }

    public int Year { get; set; }

    public string? ImageUrl { get; set; } 

    public int FuelTypeId { get; set; }

    public int VehicleTypeId { get; set;}

}
