using GeneratePhotoService.Models.ZelbikeChrono;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GeneratePhotoService.Persistence.EntitiesConfigurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(e => e.Guid);
        builder.ToTable("Order");

        builder.Property(e => e.Guid).ValueGeneratedNever();
        builder.Property(e => e.AccountBalanceLocked).HasColumnType("money");
        builder.Property(e => e.AccountBalanceUsed).HasColumnType("money");
        builder.Property(e => e.AlfabankPaymentId).HasMaxLength(50);
        builder.Property(e => e.AlfabankPaymentUrl).HasMaxLength(2000);
        builder.Property(e => e.BankPaymentProvider).HasMaxLength(50);
        builder.Property(e => e.BankPaymentSystem).HasMaxLength(50);
        builder.Property(e => e.BankPaymentWay).HasMaxLength(50);
        builder.Property(e => e.BankProductCategory).HasMaxLength(50);
        builder.Property(e => e.Campaign).HasMaxLength(250);
        builder.Property(e => e.CompletedDate).HasColumnType("datetime");
        builder.Property(e => e.CreatedDate).HasColumnType("datetime");
        builder.Property(e => e.Donation).HasColumnType("money");
        builder.Property(e => e.DonationProcessingRate).HasColumnType("decimal(18, 3)");
        builder.Property(e => e.LimetimePhotoAccessKey).HasMaxLength(50);
        builder.Property(e => e.MerchProgessingRate).HasColumnType("decimal(18, 3)");
        builder.Property(e => e.Notes).HasMaxLength(400);
        builder.Property(e => e.PageUrl).HasMaxLength(2000);
        builder.Property(e => e.Price).HasColumnType("money");
        builder.Property(e => e.ProcessingRate).HasColumnType("decimal(18, 3)");
        builder.Property(e => e.PromoCode).HasMaxLength(50);
        builder.Property(e => e.RecoveryDate).HasColumnType("datetime");
        builder.Property(e => e.ReferrerUrl).HasMaxLength(4000);
        builder.Property(e => e.RefundAmount).HasColumnType("money");
        builder.Property(e => e.RefundDate).HasColumnType("datetime");
        builder.Property(e => e.SberbankOrderId).HasMaxLength(50);
        builder.Property(e => e.SberbankPayUrl).HasMaxLength(2000);
        builder.Property(e => e.TinkoffPaymentId).HasMaxLength(50);
        builder.Property(e => e.TinkoffPaymentUrl).HasMaxLength(2000);
    }
}