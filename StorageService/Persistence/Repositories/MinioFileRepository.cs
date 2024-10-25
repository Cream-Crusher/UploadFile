using Minio;
using Minio.DataModel.Args;

namespace StorageService.Persistence.Repositories;

public class MinioFileRepository : IFileRepository
{
    private readonly IMinioClient _minioClient;
    
    public MinioFileRepository(IConfiguration configuration)
    {
        var apiEndpoint = configuration.GetSection("Minio:Endpoint").Value;
        var accessKey = configuration.GetSection("Minio:AccessKey").Value;
        var secretKey = configuration.GetSection("Minio:SecretKey").Value;

        _minioClient = new MinioClient()
        # warning todo: delete test creds 
            .WithEndpoint(apiEndpoint)
            .WithCredentials(accessKey, secretKey)
            .Build();
    }

    public async Task<bool> UploadAsync(IFormFile file, string bucketName, string key, CancellationToken token)
    {
        var putObjectArgs = new PutObjectArgs()
            .WithBucket(bucketName)
            .WithObject(key)
            .WithStreamData(file.OpenReadStream())
            .WithObjectSize(file.Length)
            .WithContentType(file.ContentType);
    
        var response = await _minioClient.PutObjectAsync(putObjectArgs, token);
        return true;
    }

    public async Task<Stream> GetAsync(string bucketName, string key, CancellationToken token)
    {
        var memoryStream = new MemoryStream();

        var getObjectArgs = new GetObjectArgs()
            .WithBucket(bucketName)
            .WithObject(key)
            .WithCallbackStream(stream => stream.CopyTo(memoryStream));
        
        await _minioClient.GetObjectAsync(getObjectArgs, token);
        memoryStream.Position = 0;
        return memoryStream;
    }

    public async Task<bool> DeleteAsync(string bucketName, string key, CancellationToken token)
    {
        var deleteObjectArgs = new RemoveObjectArgs()
            .WithBucket(bucketName)
            .WithObject(key);

        await _minioClient.RemoveObjectAsync(deleteObjectArgs, token);
        return true;
    }
}