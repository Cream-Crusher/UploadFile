using System;
using System.Collections.Generic;

namespace GeneratePhotoService.Models.ZelbikeChrono;

public class PhotoFile
{
    public Guid Guid { get; set; }

    public Guid? OrganizationGuid { get; set; }

    public Guid? UploadedByGuid { get; set; }

    public string Name { get; set; }

    public string Extension { get; set; }

    public DateTime LastUpdatedDate { get; set; }

    public string ContentType { get; set; }

    public long Size { get; set; }

    public int TypeId { get; set; }

    public string? ExternalUrl { get; set; }

    public string SelectelBucketName { get; set; }

    public string SelectelFileKey { get; set; }

    public DateTime? ExpireDate { get; set; }

#warning todo: uncoment
    // public virtual Organization Organization { get; set; }

    public virtual ICollection<RaceStagePhoto> RaceStagePhotoFullFiles { get; set; } = new List<RaceStagePhoto>();

    public virtual ICollection<RaceStagePhoto> RaceStagePhotoOpenThumbnailFiles { get; set; } = new List<RaceStagePhoto>();

    public virtual ICollection<RaceStagePhoto> RaceStagePhotoPreviewFiles { get; set; } = new List<RaceStagePhoto>();

    public virtual ICollection<RaceStagePhoto> RaceStagePhotoThumbnailFiles { get; set; } = new List<RaceStagePhoto>();
}
