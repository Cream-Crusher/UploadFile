using GeneratePhotoService.Models.ZelbikeChrono;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GeneratePhotoService.Persistence.EntitiesConfigurations;

public class OrganizationConfiguration : IEntityTypeConfiguration<Organization>
{
    public void Configure(EntityTypeBuilder<Organization> builder)
    {
        builder.HasKey(e => e.Guid);

        builder.ToTable("Organization");

        builder.Property(e => e.Guid).ValueGeneratedNever();
        builder.Property(e => e.Balance).HasColumnType("money");
        builder.Property(e => e.CustomDomain).HasMaxLength(250);
        builder.Property(e => e.DisplayName)
            .IsRequired()
            .HasMaxLength(250);
        builder.Property(e => e.DonationProcessingRate).HasColumnType("decimal(18, 3)");
        builder.Property(e => e.EmailPrice).HasColumnType("money");
        builder.Property(e => e.FacebookAppId).HasMaxLength(250);
        builder.Property(e => e.FacebookAppKey).HasMaxLength(250);
        builder.Property(e => e.Features).HasMaxLength(500);
        builder.Property(e => e.OrderProcessingRate).HasColumnType("decimal(18, 3)");
        builder.Property(e => e.PhotoProcessingPrice).HasColumnType("decimal(18, 3)");
        builder.Property(e => e.ReplyEmailAddress).HasMaxLength(150);
        builder.Property(e => e.SmsPrice).HasColumnType("money");
        builder.Property(e => e.UrlName).HasMaxLength(250);
        builder.Property(e => e.VisualTemplateName).HasMaxLength(150);
        builder.Property(e => e.VkontakteAppId).HasMaxLength(250);
        builder.Property(e => e.VkontakteAppKey).HasMaxLength(250);
    }
}