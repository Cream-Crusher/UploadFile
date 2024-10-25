using System;
using System.Collections.Generic;

namespace LimetimePhotoUploadUI.Models.ZelbikeChrono;

public partial class Order
{
    public Guid Guid { get; set; }

    public Guid AccountGuid { get; set; }

    public int Id { get; set; }

    public DateTime CreatedDate { get; set; }

    public int StateId { get; set; }

    public DateTime? CompletedDate { get; set; }

    public int? PaymentMethodId { get; set; }

    public Guid? PaidByAccountGuid { get; set; }

    public decimal Price { get; set; }

    public decimal ProcessingRate { get; set; }

    public bool IsBalanceUpdate { get; set; }

    public decimal? AccountBalanceUsed { get; set; }

    public string ReferrerUrl { get; set; }

    public string Campaign { get; set; }

    public string PromoCode { get; set; }

    public Guid? YandexKassaSettingsGuid { get; set; }

    public Guid? YandexWalletSettingsGuid { get; set; }

    public Guid? ManualPaymentSystemSettingsGuid { get; set; }

    public decimal? AccountBalanceLocked { get; set; }

    public Guid? OrganizationGuid { get; set; }

    public Guid? SberbankPaymentSettingsGuid { get; set; }

    public string SberbankOrderId { get; set; }

    public string SberbankPayUrl { get; set; }

    public string TinkoffPaymentId { get; set; }

    public string TinkoffPaymentUrl { get; set; }

    public decimal? Donation { get; set; }

    public decimal? DonationProcessingRate { get; set; }

    public decimal? MerchProgessingRate { get; set; }

    public decimal? RefundAmount { get; set; }

    public string AlfabankPaymentId { get; set; }

    public string AlfabankPaymentUrl { get; set; }

    public string PageUrl { get; set; }

    public DateTime? RefundDate { get; set; }

    public string BankPaymentWay { get; set; }

    public string BankPaymentSystem { get; set; }

    public string BankPaymentProvider { get; set; }

    public string BankProductCategory { get; set; }

    public Guid? PromoCodeGuid { get; set; }

    public string LimetimePhotoAccessKey { get; set; }

    public string DataJson { get; set; }

    public string ReceiptJson { get; set; }

    public int? ReceiptStateId { get; set; }

    public Guid? LegalEntityRusGuid { get; set; }

    public DateTime? RecoveryDate { get; set; }

    public string Notes { get; set; }

    public int? ReceiptOriginType { get; set; }

    public virtual ICollection<OrderPhoto> OrderPhotos { get; set; } = new List<OrderPhoto>();
}
