using System;
using System.Collections.Generic;

namespace GeneratePhotoService.Models.ZelbikeChrono;

public class RaceStagePhoto
{
    public Guid Guid { get; set; }

    public Guid RaceStageGuid { get; set; }

    public Guid ThumbnailFileGuid { get; set; }

    public Guid FullFileGuid { get; set; }

    public Guid? PreviewFileGuid { get; set; }

    public Guid? OpenThumbnailFileGuid { get; set; }

    public string RegistrationNumbers { get; set; }

    public string TeamNumbers { get; set; }

    public string Tags { get; set; }

    public int StateId { get; set; }

    public Guid? AiVisionPhotoSetGuid { get; set; }

    public string AiVisionJson { get; set; }

    public bool IsVertical { get; set; }

    public Guid? PhotographerGuid { get; set; }

    public string HumanTaggedRegistrationNumbers { get; set; }

    public string AitaggedRegistrationNumbers { get; set; }

    public virtual PhotoFile FullPhotoFile { get; set; }

    public virtual PhotoFile OpenThumbnailPhotoFile { get; set; }

    public virtual ICollection<OrderPhoto> OrderPhotos { get; set; } = new List<OrderPhoto>();

    public virtual Photographer Photographer { get; set; }

    public virtual PhotoFile PreviewPhotoFile { get; set; }

    public virtual RaceStage RaceStage { get; set; }

    public virtual ICollection<RaceStagePhotoPerson> RaceStagePhotoPeople { get; set; } = new List<RaceStagePhotoPerson>();

    public virtual PhotoFile ThumbnailPhotoFile { get; set; }
}
