using System.Data.Entity;
using LimetimePhotoUploadUI.Models.Repositories;
using LimetimePhotoUploadUI.Models.ZelbikeChrono;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SixLabors.Fonts;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;
using File = LimetimePhotoUploadUI.Models.ZelbikeChrono.File;

namespace LimetimePhotoUploadUI.Controllers
{
    public class HomeController : GenericController
    {
        private readonly ISelectelFileRepository _selectelFileRepository;

        public HomeController(IConfiguration configuration, ZelbikeChronoContext entities, ISelectelFileRepository selectelFileRepository) : base(configuration, entities)
        {
            this._selectelFileRepository = selectelFileRepository;
        }

        public IActionResult Index(string accessKey)
        {
            if (string.IsNullOrWhiteSpace(accessKey))
            {
                return this.View();
            }

            Photographer? photographer = this.Entities.Photographers.FirstOrDefault(p => p.AccessKey == accessKey);

            if (photographer == null || photographer.EditableTillDate.HasValue && photographer.EditableTillDate > DateTime.UtcNow)
            {
                return this.View();
            }

            this.ViewBag.Photographer = photographer;


            this.ViewBag.DataObject = new
            {
                photos = photographer.RaceStagePhotos
                    .OrderByDescending(p => p.FullFile.LastUpdatedDate)
                    .Select(rsp => new
                    {
                        rsp.Guid,
                        rsp.StateId,
                        RegistrationNumbers = (rsp.RegistrationNumbers ?? "").Trim(','),
                        isEditable = rsp.AiVisionPhotoSetGuid == null,
                        isVertical = rsp.IsVertical,
                        urlThumbnail = rsp.ThumbnailFile.SelectelFileKey != null
                            ? "https://photos.ltcdn.ru/" + rsp.ThumbnailFile.SelectelFileKey
                            : null,
                        urlOpenThumbnail = rsp.OpenThumbnailFile.SelectelFileKey != null
                            ? "https://photos.ltcdn.ru/" + rsp.OpenThumbnailFile.SelectelFileKey
                            : null,
                        urlPreview = rsp.PreviewFile.SelectelFileKey != null
                            ? "https://photos.ltcdn.ru/" + rsp.PreviewFile.SelectelFileKey
                            : null,
                        urlFull = rsp.FullFile.SelectelFileKey != null
                            ? "https://photos.ltcdn.ru/" + rsp.FullFile.SelectelFileKey
                            : null
                    })
                    .ToList()
            };


            return View();
        }

        public async Task<IActionResult> Delete(string accessKey, Guid photoGuid)
        {
            if (string.IsNullOrWhiteSpace(accessKey))
            {
                return this.FailResult("Invalid key");
            }

            Photographer? photographer = this.Entities.Photographers.FirstOrDefault(p => p.AccessKey == accessKey);

            if (photographer == null || photographer.EditableTillDate.HasValue && photographer.EditableTillDate > DateTime.UtcNow)
            {
                return this.FailResult("Invalid key");
            }

            RaceStagePhoto raceStagePhoto = this.Entities.RaceStagePhotos
                .Include("OrderPhotos")
                .Include("RaceStagePhotoPeople")
                .FirstOrDefault(p => p.Guid == photoGuid && p.PhotographerGuid == photographer.Guid);

            if (raceStagePhoto == null)
            {
                return this.FailResult();
            }

            if (raceStagePhoto.OrderPhotos.Any(x => x.Order.StateId == 20))
            {
                return this.FailResult("This photo is already purchased");
            }

            foreach (OrderPhoto orderPhoto in raceStagePhoto.OrderPhotos.ToList())
            {
                this.Entities.OrderPhotos.Remove(orderPhoto);
            }

            foreach (RaceStagePhotoPerson raceStagePhotoPerson in raceStagePhoto.RaceStagePhotoPeople.ToList())
            {
                this.Entities.RaceStagePhotoPeople.Remove(raceStagePhotoPerson);
            }

            this.Entities.RaceStagePhotos.Remove(raceStagePhoto);

            if (raceStagePhoto.FullFile != null)
            {
                await this._selectelFileRepository.DeleteAsync(raceStagePhoto.FullFile.SelectelBucketName, raceStagePhoto.FullFile.SelectelFileKey);
                this.Entities.Files.Remove(raceStagePhoto.FullFile);
            }

            if (raceStagePhoto.PreviewFile != null)
            {
                await this._selectelFileRepository.DeleteAsync(raceStagePhoto.PreviewFile.SelectelBucketName, raceStagePhoto.PreviewFile.SelectelFileKey);
                this.Entities.Files.Remove(raceStagePhoto.PreviewFile);
            }

            if (raceStagePhoto.ThumbnailFile != null)
            {
                await this._selectelFileRepository.DeleteAsync(raceStagePhoto.ThumbnailFile.SelectelBucketName, raceStagePhoto.ThumbnailFile.SelectelFileKey);
                this.Entities.Files.Remove(raceStagePhoto.ThumbnailFile);
            }

            if (raceStagePhoto.OpenThumbnailFile != null)
            {
                await this._selectelFileRepository.DeleteAsync(raceStagePhoto.OpenThumbnailFile.SelectelBucketName, raceStagePhoto.OpenThumbnailFile.SelectelFileKey);
                this.Entities.Files.Remove(raceStagePhoto.OpenThumbnailFile);
            }


            this.Entities.SaveChanges();

            return this.SuccessResult();
        }


        public async Task<ActionResult> Upload(string accessKey, IFormFile? file)
        {
            Photographer? photographer = this.Entities.Photographers.FirstOrDefault(p => p.AccessKey == accessKey);

            if (photographer == null || photographer.EditableTillDate.HasValue && photographer.EditableTillDate > DateTime.UtcNow)
            {
                return this.FailResult("Invalid key");
            }

            if (file == null)
            {
                return this.FailResult("file is empty");
            }

            RaceStage raceStage = photographer.RaceStage;

            long diskUsageMb = photographer.RaceStagePhotos.Sum(p => p.FullFile.Size + p.PreviewFile.Size + p.ThumbnailFile.Size) / 1024 / 1024;

            if (photographer.UploadLimitMb.HasValue && photographer.UploadLimitMb < diskUsageMb)
            {
                return this.FailResult("Превышен лимит по загрузке для фотографа: " + (photographer.UploadLimitMb /1024) + " мб");
            }

            try
            {
                using Stream fileStream = file.OpenReadStream();
                Image image = Image.Load(fileStream);

                string fileFileName = file.FileName;

                RaceStagePhoto raceStagePhoto = this.Entities.RaceStagePhotos.FirstOrDefault(rsp =>
                    rsp.RaceStageGuid == raceStage.Guid &&
                    rsp.PhotographerGuid == photographer.Guid &&
                    rsp.FullFile.Name == fileFileName);

                bool isNew = raceStagePhoto == null;
                Guid organizationGuid = raceStage.Race.RaceOrganizations.First().OrganizationGuid;
                string bucketName = this.Configuration["Selectel:Bucket"];

                if (raceStagePhoto == null)
                {
                    Guid fullImageGuid = Guid.NewGuid();

                    File fullImage = new File()
                    {
                        Guid = fullImageGuid,
                        OrganizationGuid = organizationGuid,
                        Name = fileFileName,
                        ContentType = "image/jpeg",
                        LastUpdatedDate = DateTime.UtcNow,
                        Extension = "jpg",
                        SelectelBucketName = bucketName,
                        SelectelFileKey = organizationGuid + "/" + raceStage.Guid + "/" + fullImageGuid + "." + "jpg"
                    };

                    Guid thumbnailImageGuid = Guid.NewGuid();

                    File thumbnailImage = new File()
                    {
                        Guid = thumbnailImageGuid,
                        OrganizationGuid = organizationGuid,
                        Name = fileFileName + "-small",
                        ContentType = "image/jpeg",
                        LastUpdatedDate = DateTime.UtcNow,
                        Extension = "jpg",
                        SelectelBucketName = bucketName,
                        SelectelFileKey = organizationGuid + "/" + raceStage.Guid + "/" + thumbnailImageGuid + "." + "jpg"
                    };

                    Guid previewFileGuid = Guid.NewGuid();

                    File previewImage = new File()
                    {
                        Guid = previewFileGuid,
                        OrganizationGuid = organizationGuid,
                        Name = fileFileName + "-preview",
                        ContentType = "image/jpeg",
                        LastUpdatedDate = DateTime.UtcNow,
                        Extension = "jpg",
                        SelectelBucketName = bucketName,
                        SelectelFileKey = organizationGuid + "/" + raceStage.Guid + "/" + previewFileGuid + "." + "jpg"
                    };

                    Guid openThumbnailGuid = Guid.NewGuid();

                    File openThumbnail = new File()
                    {
                        Guid = openThumbnailGuid,
                        OrganizationGuid = organizationGuid,
                        Name = fileFileName + "-small-open",
                        ContentType = "image/jpeg",
                        LastUpdatedDate = DateTime.UtcNow,
                        Extension = "jpg",
                        SelectelBucketName = bucketName,
                        SelectelFileKey = organizationGuid + "/" + raceStage.Guid + "/" + openThumbnailGuid + "." + "jpg"
                    };

                    raceStagePhoto = new RaceStagePhoto()
                    {
                        Guid = Guid.NewGuid(),
                        RaceStageGuid = raceStage.Guid,
                        Photographer = photographer,
                        FullFile = fullImage,
                        ThumbnailFile = thumbnailImage,
                        PreviewFile = previewImage,
                        OpenThumbnailFile = openThumbnail,
                        RegistrationNumbers = null,
                        IsVertical = image.Height > image.Width,
                    };

                    this.Entities.Files.Add(fullImage);
                    this.Entities.Files.Add(thumbnailImage);
                    this.Entities.Files.Add(previewImage);
                    this.Entities.RaceStagePhotos.Add(raceStagePhoto);
                }

                MemoryStream originalImageStream = new MemoryStream();
                MemoryStream previewImageStream = new MemoryStream();
                MemoryStream thumbnailImageStream = new MemoryStream();
                MemoryStream openThumbnailImageStream = new MemoryStream();

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

                    this.ApplyWatermark(image);

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
                                Size = new Size(4300,2867)
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

                    this.ApplyWatermark(image);

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


                }

                var FullFileSize = originalImageStream.Length;
                var PreviewFileSize = previewImageStream.Length;
                var ThumbnailFileSize = thumbnailImageStream.Length;
                var OpenThumbnailFileSize = openThumbnailImageStream.Length;

                await this._selectelFileRepository.Upload(
                    raceStagePhoto.FullFile.SelectelBucketName,
                    raceStagePhoto.FullFile.SelectelFileKey,
                    originalImageStream);

                await this._selectelFileRepository.Upload(
                    raceStagePhoto.PreviewFile.SelectelBucketName,
                    raceStagePhoto.PreviewFile.SelectelFileKey,
                    previewImageStream);

                await this._selectelFileRepository.Upload(
                    raceStagePhoto.ThumbnailFile.SelectelBucketName,
                    raceStagePhoto.ThumbnailFile.SelectelFileKey,
                    thumbnailImageStream);

                await this._selectelFileRepository.Upload(
                    raceStagePhoto.OpenThumbnailFile.SelectelBucketName,
                    raceStagePhoto.OpenThumbnailFile.SelectelFileKey,
                    openThumbnailImageStream);


                raceStagePhoto.FullFile.Size = FullFileSize;
                raceStagePhoto.PreviewFile.Size = PreviewFileSize;
                raceStagePhoto.ThumbnailFile.Size = ThumbnailFileSize;
                raceStagePhoto.OpenThumbnailFile.Size = OpenThumbnailFileSize;
                raceStagePhoto.RegistrationNumbers = null;
                raceStagePhoto.StateId = 0;
                raceStagePhoto.AiVisionPhotoSetGuid = raceStagePhoto.RaceStageGuid;

                this.Entities.SaveChanges();
                
                // RabbitMQBroker rabbitMQBroker = new RabbitMQBroker(
                //     configuration["RabbitMQ:HostName"], 
                //     configuration["RabbitMQ:UserName"], 
                //     configuration["RabbitMQ:Password"]);
                //
                // IQueueSender sender = rabbitMQBroker.CreateSender("", configuration["RabbitMQ:Queue"]);
                //
                // sender.Push(JsonConvert.SerializeObject(new
                // {
                //     raceStageGuid = raceStagePhoto.RaceStageGuid,
                //     photoGuid = raceStagePhoto.Guid,
                //     photoUrl = "https://photos.ltcdn.ru/" + raceStagePhoto.FullFile.SelectelFileKey
                // }));
                
                return this.SuccessResult(new
                {
                    guid = raceStagePhoto.Guid,
                    isNew,
                    urlThumbnail = raceStagePhoto.ThumbnailFile.SelectelFileKey != null
                        ? "https://photos.ltcdn.ru/" + raceStagePhoto.ThumbnailFile.SelectelFileKey
                        : null,
                    urlPreview = raceStagePhoto.PreviewFile.SelectelFileKey != null
                        ? "https://photos.ltcdn.ru/" + raceStagePhoto.PreviewFile.SelectelFileKey
                        : null,
                    urlFull = raceStagePhoto.FullFile.SelectelFileKey != null
                        ? "https://photos.ltcdn.ru/" + raceStagePhoto.FullFile.SelectelFileKey
                        : null
                });
            }
            catch (Exception e)
            {
                if (e is ThreadAbortException)
                {
                    throw;
                }

                /*
                LoggeryClient loggeryClient = new LoggeryClient(
                    ConfigurationManager.AppSettings["Loggery.URL"],
                    ConfigurationManager.AppSettings["Loggery.ApiKey"]);

                loggeryClient.Write(e.ToLogRecord());*/

                return this.FailResult("unable to save file");
            }
        }

        private void ApplyWatermark(Image image)
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

                IPath path = pathBuilder.Build();

                Font font = SystemFonts.CreateFont("Segoe UI", 14, FontStyle.Bold);
                string text = string.Join("   ", Enumerable.Repeat("LIMETIME.PHOTO", 45));

                // Draw the text along the path wrapping at the end of the line
                var textOptions = new TextOptions(font)
                {
                    WrappingLength = path.ComputeLength(),
                    VerticalAlignment = VerticalAlignment.Bottom,
                    HorizontalAlignment = HorizontalAlignment.Left,
                };

                // Let's generate the text as a set of vectors drawn along the path
                var glyphs = TextBuilder.GenerateGlyphs(text, path, textOptions);

                image.Mutate(ctx => ctx.Fill(Color.White, glyphs));
            }
        }
    }
}