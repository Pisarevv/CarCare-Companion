namespace CarCare_Companion.Core.Contracts;

using Microsoft.AspNetCore.Http;

public interface IImageService
{
    public Task<string> UploadVehicleImage(IFormFile file);

    public Task<string> GetImageUrlAsync(string stringKey);
}
