namespace CarCare_Companion.Core.Contracts;

using Microsoft.AspNetCore.Http;

public interface IFileService
{
    public Task<string> UploadFileAsync(IFormFile file, string bucketName);
}
