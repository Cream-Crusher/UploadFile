using Microsoft.AspNetCore.Mvc;
using StorageService.Persistence.Repositories;

namespace StorageService.Controllers;

[Route("api/[controller]")]
public class FilesController : ControllerBase
{
    private readonly IFileRepository _fileStorageService;

    public FilesController(IFileRepository fileRepository)
    {
        _fileStorageService = fileRepository;
    }

    [HttpPost]
    public async Task<IActionResult> UploadAsync(IFormFile file, string bucketName, string key)
    {
        await _fileStorageService.UploadAsync(file, bucketName, key, CancellationToken.None);
        return Ok($"File {file.FileName} uploaded to MinIO successfully!");
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync(string bucketName, string key, CancellationToken token)
    {
        var stream = await _fileStorageService.GetAsync(bucketName, key, token);
        return File(stream, "application/octet-stream", key);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteAsync(string bucketName, string key)
    {
        await _fileStorageService.DeleteAsync(bucketName, key, CancellationToken.None);
        return NoContent();
    }
}