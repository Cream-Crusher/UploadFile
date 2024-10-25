using System;
using System.Collections.Generic;

namespace GeneratePhotoService.Models.ZelbikeChrono;

public class Photographer
{
    public Guid Guid { get; set; }

    public Guid RaceStageGuid { get; set; }

    public string DisplayName { get; set; }

    public string AccessKey { get; set; }

    public int? UploadLimitMb { get; set; }

    public DateTime? EditableTillDate { get; set; }

    public bool IsDeleted { get; set; }

    public string TolokaPoolId { get; set; }

    public Guid? TolokaLastPoolPhotoGuid { get; set; }

    public virtual RaceStage RaceStage { get; set; }

    public virtual ICollection<RaceStagePhoto> RaceStagePhotos { get; set; } = new List<RaceStagePhoto>();
}
