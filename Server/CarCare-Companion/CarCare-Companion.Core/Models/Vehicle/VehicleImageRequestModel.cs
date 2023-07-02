namespace CarCare_Companion.Core.Models.Vehicle;

using Microsoft.AspNetCore.Http;

public class VehicleImageRequestModel
{
    public IFormFile Image { get; set; } = null!;
}
