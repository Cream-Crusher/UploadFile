using System.Net.Http.Headers;
using GeneratePhotoService.Persistence.Database;
using Microsoft.EntityFrameworkCore;
using SixLabors.Fonts;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace GeneratePhotoService.Persistence.Repositories;

public class PhotoRepository : IPhotoRepository
{
    private readonly ZelbikeChronoContext _context;
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public PhotoRepository(
        IConfiguration configuration,
        ZelbikeChronoContext context,
        HttpClient httpClient)
    {
        _context = context;
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task UploadGeneratedPhotos(
        Guid photoId,
        Guid raceStageId,
        string bucketName,
        string key,
        CancellationToken token
    )
    {
        var raceStagePhoto = await _context.RaceStagePhotos
            .FirstOrDefaultAsync(p => p.Guid == raceStageId, token);

        var stream = await GetFileFromStorage(bucketName, key, token);

        var image = await Image.LoadAsync(stream, token);
        var originalImageStream = new MemoryStream();
        var previewImageStream = new MemoryStream();
        var thumbnailImageStream = new MemoryStream();
        var openThumbnailImageStream = new MemoryStream();

        if (raceStagePhoto.IsVertical)
        {
            if (image.Height > 4300)
            {
                image.Mutate(x => x
                    .Resize(new ResizeOptions()
                    {
                        Position = AnchorPositionMode.Center,
                        Mode = ResizeMode.Min,
                        Size = new Size(2867, 4300),

                    })
                    .Resize(new ResizeOptions
                    {
                        Mode = ResizeMode.Crop,
                        Size = new Size(2867, 4300)
                    }));
            }

            image.SaveAsJpeg(originalImageStream, new JpegEncoder()
            {
                Quality = 95,
            });

            Image openThumb = image.Clone(x => x
                .Resize(new ResizeOptions()
                {
                    Position = AnchorPositionMode.Center,
                    Mode = ResizeMode.Min,
                    Size = new Size(150, 150)
                })
                .Resize(new ResizeOptions
                {
                    Mode = ResizeMode.Crop,
                    Size = new Size(150, 150)
                }));

            openThumb.SaveAsJpeg(openThumbnailImageStream, new JpegEncoder()
            {
                Quality = 80,
            });

            image.Mutate(x => x
                .Resize(new ResizeOptions()
                {
                    Position = AnchorPositionMode.Center,
                    Mode = ResizeMode.Min,
                    Size = new Size(707, 1000)
                })
                .Resize(new ResizeOptions
                {
                    Mode = ResizeMode.Crop,
                    Size = new Size(707, 1000)
                }));

            ApplyWatermark(image);

            image.SaveAsJpeg(previewImageStream, new JpegEncoder()
            {
                Quality = 80,
            });

            image.Mutate(x => x
                .Resize(new ResizeOptions()
                {
                    Position = AnchorPositionMode.Center,
                    Mode = ResizeMode.Min,
                    Size = new Size(360, 511)
                })
                .Resize(new ResizeOptions
                {
                    Mode = ResizeMode.Crop,
                    Size = new Size(360, 511)
                }));

            image.SaveAsJpeg(thumbnailImageStream, new JpegEncoder()
            {
                Quality = 80,
            });
        }
        else
        {
            if (image.Width > 4300)
            {
                image.Mutate(x => x
                    .Resize(new ResizeOptions()
                    {
                        Position = AnchorPositionMode.Center,
                        Mode = ResizeMode.Min,
                        Size = new Size(4300, 2867),

                    })
                    .Resize(new ResizeOptions
                    {
                        Mode = ResizeMode.Crop,
                        Size = new Size(4300, 2867)
                    }));
            }

            image.SaveAsJpeg(originalImageStream, new JpegEncoder()
            {
                Quality = 95,
            });

            Image openThumb = image.Clone(x => x
                .Resize(new ResizeOptions()
                {
                    Position = AnchorPositionMode.Center,
                    Mode = ResizeMode.Min,
                    Size = new Size(150, 150)
                })
                .Resize(new ResizeOptions
                {
                    Mode = ResizeMode.Crop,
                    Size = new Size(150, 150)
                }));

            openThumb.SaveAsJpeg(openThumbnailImageStream, new JpegEncoder()
            {
                Quality = 80,
            });

            image.Mutate(x => x
                .Resize(new ResizeOptions()
                {
                    Position = AnchorPositionMode.Center,
                    Mode = ResizeMode.Min,
                    Size = new Size(1000, 707)
                })
                .Resize(new ResizeOptions
                {
                    Mode = ResizeMode.Crop,
                    Size = new Size(1000, 707)
                }));

            ApplyWatermark(image);

            image.SaveAsJpeg(previewImageStream, new JpegEncoder()
            {
                Quality = 80,
            });

            image.Mutate(x => x
                .Resize(new ResizeOptions()
                {
                    Position = AnchorPositionMode.Center,
                    Mode = ResizeMode.Min,
                    Size = new Size(360, 249)
                })
                .Resize(new ResizeOptions
                {
                    Mode = ResizeMode.Crop,
                    Size = new Size(360, 249)
                }));

            image.SaveAsJpeg(thumbnailImageStream, new JpegEncoder()
            {
                Quality = 80,
            });
            var FullFileSize = originalImageStream.Length;
            var PreviewFileSize = previewImageStream.Length;
            var ThumbnailFileSize = thumbnailImageStream.Length;
            var OpenThumbnailFileSize = openThumbnailImageStream.Length;

            var myTasks = new List<Task>
            {
                UploadFile(
                    previewImageStream,
                    raceStagePhoto.PreviewPhotoFile.SelectelBucketName,
                    raceStagePhoto.PreviewPhotoFile.SelectelFileKey
                ),
                UploadFile(
                    previewImageStream,
                    raceStagePhoto.ThumbnailPhotoFile.SelectelBucketName,
                    raceStagePhoto.ThumbnailPhotoFile.SelectelFileKey
                ),
                UploadFile(
                    openThumbnailImageStream,
                    raceStagePhoto.OpenThumbnailPhotoFile.SelectelBucketName,
                    raceStagePhoto.OpenThumbnailPhotoFile.SelectelFileKey
                )
            };

            raceStagePhoto.FullPhotoFile.Size = FullFileSize;
            raceStagePhoto.PreviewPhotoFile.Size = PreviewFileSize;
            raceStagePhoto.ThumbnailPhotoFile.Size = ThumbnailFileSize;
            raceStagePhoto.OpenThumbnailPhotoFile.Size = OpenThumbnailFileSize;
            raceStagePhoto.RegistrationNumbers = null;
            raceStagePhoto.StateId = 0;
            raceStagePhoto.AiVisionPhotoSetGuid = raceStagePhoto.RaceStageGuid;
            await _context.SaveChangesAsync(token);
            
            await Task.WhenAll(myTasks);
        }
    }

    private async Task UploadFile(Stream fileStream, string bucketName, string key)
    {
        using var content = new MultipartFormDataContent();
        var url = _configuration["S3API:url"];

        var streamContent = new StreamContent(fileStream);
        streamContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/octet-stream");

        content.Add(streamContent, "file", key);

        var buildUrl = $"{url}?bucketName={bucketName}&key={key}";
        await _httpClient.PostAsync(buildUrl, content);
    }

    private static void ApplyWatermark(Image image)
    {
        for (int i = -2; i < 5; i++)
        {
            PathBuilder pathBuilder = new PathBuilder();
            pathBuilder.SetOrigin(new PointF(image.Width * i / 18, image.Height * i * 2 / 9));
            pathBuilder.AddCubicBezier(
                new PointF(0, image.Height),
                new PointF(image.Width * 2 / 9, 50),
                new PointF(image.Width * 4 / 9, 50),
                new PointF(image.Width * 6 / 9, image.Height * 1 / 3));

            pathBuilder.AddCubicBezier(
                new PointF(image.Width * 6 / 9, image.Height * 1 / 3),
                new PointF(image.Width * 8 / 9, image.Height * 2 / 3),
                new PointF(image.Width * 10 / 9, image.Height * 2 / 3),
                new PointF(image.Width * 12 / 9, 0));

            var path = pathBuilder.Build();

            Font font = SystemFonts.CreateFont("Segoe UI", 14, FontStyle.Bold);
            var text = string.Join("   ", Enumerable.Repeat("LIMETIME.PHOTO", 45));

            var textOptions = new TextOptions(font)
            {
                WrappingLength = path.ComputeLength(),
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Left,
            };

            var glyphs = TextBuilder.GenerateGlyphs(text, path, textOptions);

            image.Mutate(ctx => ctx.Fill(Color.White, glyphs));
        }
    }

    private async Task<Stream> GetFileFromStorage(string bucketName, string key, CancellationToken token)
    {
        var url = _configuration["S3API:url"];
        var buildedUrl = $"{url}?bucketName={bucketName}&key={key}";

        using var response = await _httpClient.GetAsync(buildedUrl, token);
        var stream = await response.Content.ReadAsStreamAsync(token);
        return stream;
    }
}