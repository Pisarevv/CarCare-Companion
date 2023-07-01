namespace CarCare_Companion.Core.Services;

using Amazon.S3;
using Amazon.S3.Model;
using CarCare_Companion.Core.Contracts;
using CarCare_Companion.Infrastructure.Data.Common;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

public class FileService : IFileService
{
    private readonly IAmazonS3 s3Client;
    private readonly IRepository repository;

    public FileService(IAmazonS3 s3Client, IRepository repository)
    {
        this.s3Client = s3Client;
        this.repository = repository;
    }

    public async Task<string> UploadFileAsync(IFormFile file, string bucketName)
    {
        bool bucketExists = await Amazon.S3.Util.AmazonS3Util.DoesS3BucketExistV2Async(s3Client, bucketName);
        if (!bucketExists)
        {
            throw new ArgumentNullException("Bucket does not exist");
        }

        var request = new PutObjectRequest()
        {
            BucketName = bucketName,
            Key = GenerateKey(),
            InputStream = file.OpenReadStream()
        };

        request.Metadata.Add("Content-type", file.ContentType);

        var response = await s3Client.PutObjectAsync(request);

        if(response.HttpStatusCode == System.Net.HttpStatusCode.OK) 
        {
            return request.Key;
        }

        return "invalidKey";
    }

    private string GenerateKey()
    {
        return $"UserImages/{Guid.NewGuid().ToString()}";
    }
}
