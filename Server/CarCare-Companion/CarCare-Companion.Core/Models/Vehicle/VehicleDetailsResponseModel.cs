namespace CarCare_Companion.Core.Models.Vehicle;

public class VehicleDetailsResponseModel
{
    public string Id { get; set; } = null!;

    public string Make { get; set; } = null!;

    public string Model { get; set; } = null!;

    public double Mileage { get; set; }

    public int Year { get; set; }

    public string? ImageUrl { get; set; } 

    public string FuelType { get; set; } = null!;

    public string VehicleType { get; set; } = null!;

}
