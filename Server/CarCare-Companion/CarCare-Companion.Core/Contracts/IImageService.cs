namespace CarCare_Companion.Core.Contracts;

using Microsoft.AspNetCore.Http;

public interface IImageService
{
    /// <summary>
    /// Uploads a vehicle image asynchronously.
    /// </summary>
    /// <param name="file">The image file to upload.</param>
    /// <returns>The URL or identifier of the uploaded image.</returns>
    public Task<string> UploadVehicleImage(IFormFile file);

    /// <summary>
    /// Retrieves the URL of a vehicle image based on its key asynchronously.
    /// </summary>
    /// <param name="stringKey">The key or identifier of the image.</param>
    /// <returns>The URL of the image.</returns>
    public Task<string> GetImageUrlAsync(string stringKey);
}
