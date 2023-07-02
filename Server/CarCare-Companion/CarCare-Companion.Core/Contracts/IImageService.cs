namespace CarCare_Companion.Core.Contracts;

using Microsoft.AspNetCore.Http;

public interface IImageService
{
    public Task<string> UploadVehicleImage(IFormFile file);
}
