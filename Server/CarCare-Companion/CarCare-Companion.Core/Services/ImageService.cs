namespace CarCare_Companion.Core.Services;

using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

using Amazon.S3;
using Amazon.S3.Model;

using CarCare_Companion.Core.Contracts;
using CarCare_Companion.Infrastructure.Data.Common;

using static Common.GlobalConstants;

/// <summary>
/// The ImageService is responsible for the CRUD operations of the user vehicle images or profile image.
/// </summary>
public class ImageService : IImageService
{
    private readonly IAmazonS3 s3Client;

    public ImageService(IAmazonS3 s3Client)
    {
        this.s3Client = s3Client;
    }

    /// <summary>
    /// Uploads the user vehicle image to Amazon S3 service.
    /// </summary>
    /// <param name="file">The input image passed as FormFIle</param>
    /// <returns>A string that identifies the record Id in the Amazon S3 service</returns>
    /// <exception cref="ArgumentNullException">Thrown if the target S3 bucket does not exist</exception>
    /// <exception cref="Exception">Thrown if an error occurs while uploading the image to Amazon S3 bucket</exception>
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

        else
        {
            throw new Exception("An error occurred while uploading image");
        }
    }

    /// <summary>
    /// Searches the Amazon S3 bucket for the wanted image by Id
    /// </summary>
    /// <param name="imageId">The image Id</param>
    /// <returns>A pre-signed URL from Amazon S3 service that the user will be able to access for a specific period</returns>
    /// <exception cref="ArgumentNullException">Thrown if the S3 bucket does not exist</exception>
    public async Task<string> GetImageUrlAsync(string imageId)
    {
        bool bucketExists = await Amazon.S3.Util.AmazonS3Util.DoesS3BucketExistV2Async(s3Client, AWSBucket);
        if (!bucketExists)
        {
            throw new ArgumentNullException("Bucket does not exist");
        }

        var urlRequest = new GetPreSignedUrlRequest()
        {
            BucketName = AWSBucket,
            Key = imageId,
            Expires = DateTime.UtcNow.AddDays(1)
        };

        return s3Client.GetPreSignedURL(urlRequest);
       
    }


}
