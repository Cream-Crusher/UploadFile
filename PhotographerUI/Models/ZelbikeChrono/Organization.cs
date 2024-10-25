using System;
using System.Collections.Generic;

namespace LimetimePhotoUploadUI.Models.ZelbikeChrono;

public partial class Organization
{
    public Guid Guid { get; set; }

    public string DisplayName { get; set; }

    public string UrlName { get; set; }

    public string CustomDomain { get; set; }

    public string VkontakteAppId { get; set; }

    public string VkontakteAppKey { get; set; }

    public string FacebookAppId { get; set; }

    public string FacebookAppKey { get; set; }

    public string VisualTemplateName { get; set; }

    public bool IsDeleted { get; set; }

    public Guid AdminInviteCode { get; set; }

    public int LevelId { get; set; }

    public int LimetimePaymentSystemStateId { get; set; }

    public decimal Balance { get; set; }

    public bool IsPageTrackingEnabled { get; set; }

    public string ReplyEmailAddress { get; set; }

    public string Features { get; set; }

    public decimal OrderProcessingRate { get; set; }

    public decimal DonationProcessingRate { get; set; }

    public int? CloudLimitMb { get; set; }

    public decimal? SmsPrice { get; set; }

    public decimal? EmailPrice { get; set; }

    public int SmsBalance { get; set; }

    public int EmailBalance { get; set; }

    public int? PhotoProcessingBalance { get; set; }

    public decimal? PhotoProcessingPrice { get; set; }

    public virtual ICollection<File> Files { get; set; } = new List<File>();

    public virtual ICollection<RaceOrganization> RaceOrganizations { get; set; } = new List<RaceOrganization>();
}
