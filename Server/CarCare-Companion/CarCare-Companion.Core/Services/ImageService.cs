namespace CarCare_Companion.Core.Services;

using Amazon.S3;
using Amazon.S3.Model;
using CarCare_Companion.Core.Contracts;
using CarCare_Companion.Infrastructure.Data.Common;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

using static Common.GlobalConstants;

public class ImageService : IImageService
{
    private readonly IAmazonS3 s3Client;

    public ImageService(IAmazonS3 s3Client, IRepository repository)
    {
        this.s3Client = s3Client;
    }

    public async Task<string> UploadVehicleImage(IFormFile file)
    {
        bool bucketExists = await Amazon.S3.Util.AmazonS3Util.DoesS3BucketExistV2Async(s3Client, AWSBucket);
        if (!bucketExists)
        {
            throw new ArgumentNullException("Bucket does not exist");
        }

        var imageKey = Guid.NewGuid().ToString().ToUpper();

        var request = new PutObjectRequest()
        {
            BucketName = AWSBucket,
            Key = imageKey,
            InputStream = file.OpenReadStream()
        };

        request.Metadata.Add("Content-type", file.ContentType);

        var response = await s3Client.PutObjectAsync(request);

        if(response.HttpStatusCode == System.Net.HttpStatusCode.OK) 
        {
            return imageKey;
        }

        return "invalidKey";
    }

    public async Task<string> GetImageUrlAsync(string stringKey)
    {
        bool bucketExists = await Amazon.S3.Util.AmazonS3Util.DoesS3BucketExistV2Async(s3Client, AWSBucket);
        if (!bucketExists)
        {
            throw new ArgumentNullException("Bucket does not exist");
        }

        var urlRequest = new GetPreSignedUrlRequest()
        {
            BucketName = AWSBucket,
            Key = stringKey,
            Expires = DateTime.UtcNow.AddDays(1)
        };

        return s3Client.GetPreSignedURL(urlRequest);
       
    }


}
