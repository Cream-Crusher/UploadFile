using System;
using System.Collections.Generic;

namespace LimetimePhotoUploadUI.Models.ZelbikeChrono;

public partial class RaceStagePhotoPerson
{
    public Guid Guid { get; set; }

    public Guid RaceStagePhotoGuid { get; set; }

    public Guid? AccountGuid { get; set; }

    public Guid FaceGuid { get; set; }

    public string MetaJson { get; set; }

    public int? Bib { get; set; }

    public virtual RaceStagePhoto RaceStagePhoto { get; set; }
}
