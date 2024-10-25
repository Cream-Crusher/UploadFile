namespace LimetimePhotoUploadUI.Models.Repositories
{
    public interface ISelectelFileRepository
    {
        public Task Upload(string bucketName, string key, Stream content);
        public Task DeleteAsync(string bucketName, string key);
        public Task<Stream> GetAsync(string bucketName, string key);
    }
}
