namespace CarCare_Companion.Core.Models.Vehicle;

using System.ComponentModel.DataAnnotations;



public class VehicleEditResponseModel
{
    public string Make { get; set; } = null!;

    public string Model { get; set; } = null!;

    public double Mileage { get; set; }


    public int Year { get; set; }

    public int FuelTypeId { get; set; }


    public int VehicleTypeId { get; set; }

}
