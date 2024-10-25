using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;

namespace StorageService.Persistence.Repositories;

public class SelectelFileRepository : IFileRepository
{
    private readonly BasicAWSCredentials _credentials;
    private readonly AmazonS3Config _config;

    public SelectelFileRepository(IConfiguration configuration)
    {
        var apiEndpoint = configuration.GetSection("Storage:Endpoint").Value;
        var accessKey = configuration.GetSection("Storage:AccessKey").Value;
        var secretKey = configuration.GetSection("Storage:SecretKey").Value;
        
        _credentials = new BasicAWSCredentials(accessKey, secretKey);
        _config = new AmazonS3Config
        {
            ServiceURL = apiEndpoint,
            ForcePathStyle = true,
        };
    }

    public async Task<bool> UploadAsync(IFormFile file, string bucketName, string key, CancellationToken token)
    {
        using var s3Client = new AmazonS3Client(_credentials, _config);
        using var transferUtility = new TransferUtility(s3Client);

        await transferUtility.UploadAsync(file.OpenReadStream(), bucketName, key, token);
        return true;
    }

    public async Task<Stream> GetAsync(string bucketName, string key, CancellationToken token)
    {
        using var s3Client = new AmazonS3Client(_credentials, _config);

        var request = new GetObjectRequest
        {
            BucketName = bucketName,
            Key = key,
        };
        var response = await s3Client.GetObjectAsync(request, token);

        return response.ResponseStream;
    }

    public async Task<bool> DeleteAsync(string bucketName, string key, CancellationToken token)
    {
        using var s3Client = new AmazonS3Client(_credentials, _config);
        
        var request = new DeleteObjectRequest
        {
            BucketName = bucketName,
            Key = key,
        };
        await s3Client.DeleteObjectAsync(request, token);
        return true;
    }
}