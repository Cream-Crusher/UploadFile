namespace StorageService.Persistence.Repositories;

public interface IFileRepository
{
    Task<bool> UploadAsync(IFormFile file, string bucketName, string key, CancellationToken token);
    Task<Stream> GetAsync(string bucketName, string key, CancellationToken token);
    Task<bool> DeleteAsync(string bucketName, string key, CancellationToken token);
}