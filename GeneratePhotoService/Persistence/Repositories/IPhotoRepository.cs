namespace GeneratePhotoService.Persistence.Repositories;

public interface IPhotoRepository
{
    Task UploadGeneratedPhotos(Guid photoId, Guid raceStageId, string bucketName, string key, CancellationToken token);
}