using System;
using System.Collections.Generic;

namespace LimetimePhotoUploadUI.Models.ZelbikeChrono;

public partial class OrderPhoto
{
    public Guid Guid { get; set; }

    public Guid OrderGuid { get; set; }

    public Guid RaceStagePhotoGuid { get; set; }

    public decimal Price { get; set; }

    public virtual Order Order { get; set; }

    public virtual RaceStagePhoto RaceStagePhoto { get; set; }
}
