using System;
using System.Collections.Generic;

namespace LimetimePhotoUploadUI.Models.ZelbikeChrono;

public partial class RaceStage
{
    public Guid Guid { get; set; }

    public Guid RaceGuid { get; set; }

    public int? TypeId { get; set; }

    public string DisplayNamePrimary { get; set; }

    public string DisplayNameSecondary { get; set; }

    public string UrlName { get; set; }

    public DateTime? StartDateTime { get; set; }

    public string LocationShort { get; set; }

    public string LocationDetailed { get; set; }

    public string DrivingDirectionsLink { get; set; }

    public bool IsDeleted { get; set; }

    public int OrderIndex { get; set; }

    public string LocationLatLong { get; set; }

    public int TimeZone { get; set; }

    public Guid? ImportFileGuid { get; set; }

    public string TrackConfigurationJson { get; set; }

    public string ExpressAccessKey { get; set; }

    public decimal? SinglePhotoPrice { get; set; }

    public decimal? AllPersonPhotosPrice { get; set; }

    public decimal? PhotoSetPrice { get; set; }

    public int? PhotoSetSize { get; set; }

    public decimal? PhotoProcessingRate { get; set; }

    public DateTime? ProfileChangesTillDateTime { get; set; }

    public string City { get; set; }

    public string Region { get; set; }

    public string Settlement { get; set; }

    public bool IsPhotosPublished { get; set; }

    public DateTime? CreatedDate { get; set; }

    public Guid? ContainerGuid { get; set; }

    public virtual ICollection<Photographer> Photographers { get; set; } = new List<Photographer>();

    public virtual Race Race { get; set; }

    public virtual ICollection<RaceStagePhoto> RaceStagePhotos { get; set; } = new List<RaceStagePhoto>();
}
