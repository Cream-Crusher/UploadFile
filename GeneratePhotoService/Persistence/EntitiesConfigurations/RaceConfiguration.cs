using GeneratePhotoService.Models.ZelbikeChrono;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GeneratePhotoService.Persistence.EntitiesConfigurations;

public class RaceConfiguration : IEntityTypeConfiguration<Race>
{
    public void Configure(EntityTypeBuilder<Race> builder)
    {
        builder.HasKey(e => e.Guid);

        builder.ToTable("Race");

        builder.Property(e => e.Guid).ValueGeneratedNever();
        builder.Property(e => e.BannerBackgroundColor).HasMaxLength(10);
        builder.Property(e => e.BannerContentHorizontalPosition).HasMaxLength(50);
        builder.Property(e => e.BannerContentOpacity).HasMaxLength(50);
        builder.Property(e => e.BannerContentStyle).HasMaxLength(50);
        builder.Property(e => e.BannerContentType).HasMaxLength(50);
        builder.Property(e => e.BannerContentVerticalPosition).HasMaxLength(50);
        builder.Property(e => e.BannerImageUrl).HasMaxLength(500);
        builder.Property(e => e.BannerPosition).HasMaxLength(50);
        builder.Property(e => e.BannerStyle).HasMaxLength(50);
        builder.Property(e => e.BannerThumbnailUrl).HasMaxLength(500);
        builder.Property(e => e.ContactEmail).HasMaxLength(50);
        builder.Property(e => e.ContactPhone).HasMaxLength(50);
        builder.Property(e => e.DisplayNamePrimary)
            .IsRequired()
            .HasMaxLength(250);
        builder.Property(e => e.DisplayNameSecondary).HasMaxLength(250);
        builder.Property(e => e.UrlName).HasMaxLength(250);
    }
}