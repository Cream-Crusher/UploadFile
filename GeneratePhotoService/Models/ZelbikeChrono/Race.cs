using System;
using System.Collections.Generic;

namespace GeneratePhotoService.Models.ZelbikeChrono;

public class Race
{
    public Guid Guid { get; set; }

    public string DisplayNamePrimary { get; set; }

    public string DisplayNameSecondary { get; set; }

    public string UrlName { get; set; }

    public string ContactPhone { get; set; }

    public string ContactEmail { get; set; }

    public string BannerImageUrl { get; set; }

    public string BannerStyle { get; set; }

    public string BannerPosition { get; set; }

    public int? BannerHeight { get; set; }

    public string BannerContentStyle { get; set; }

    public bool IsDeleted { get; set; }

    public int CategoryMatchTypeId { get; set; }

    public string BannerThumbnailUrl { get; set; }

    public string BannerContentType { get; set; }

    public string BannerContentVerticalPosition { get; set; }

    public string BannerContentHorizontalPosition { get; set; }

    public string BannerContentOpacity { get; set; }

    public int? CupTypeId { get; set; }

    public string BannerBackgroundColor { get; set; }

    public Guid? YandexWalletSettingsGuid { get; set; }

    public Guid? YandexKassaSettingsGuid { get; set; }

    public bool IsHidden { get; set; }

    public Guid? SberbankPaymentSettingsGuid { get; set; }

    public int ResultPrecisionId { get; set; }

    public int VisibilityLevelId { get; set; }

    public bool IsDonationsEnabled { get; set; }

    public int RegistrationMode { get; set; }

    public int RegistrationModeId { get; set; }

    public int RegistrationDuplicatesPolicyId { get; set; }

    public virtual ICollection<RaceOrganization> RaceOrganizations { get; set; } = new List<RaceOrganization>();

    public virtual ICollection<RaceStage> RaceStages { get; set; } = new List<RaceStage>();
}
