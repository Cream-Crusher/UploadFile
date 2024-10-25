using GeneratePhotoService.Models.ZelbikeChrono;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GeneratePhotoService.Persistence.EntitiesConfigurations;

public class RaceStagePhotoConfiguration : IEntityTypeConfiguration<RaceStagePhoto>
{
    public void Configure(EntityTypeBuilder<RaceStagePhoto> builder)
    {
        builder.HasKey(e => e.Guid);

        builder.ToTable("RaceStagePhoto");

        builder.Property(e => e.Guid).ValueGeneratedNever();
        builder.Property(e => e.AiVisionJson).HasMaxLength(4000);
        builder.Property(e => e.AitaggedRegistrationNumbers)
            .HasMaxLength(500)
            .HasColumnName("AITaggedRegistrationNumbers");
        builder.Property(e => e.HumanTaggedRegistrationNumbers).HasMaxLength(500);
        builder.Property(e => e.RegistrationNumbers).HasMaxLength(100);
        builder.Property(e => e.Tags).HasMaxLength(500);
        builder.Property(e => e.TeamNumbers).HasMaxLength(100);

        builder.HasOne(d => d.FullPhotoFile).WithMany(p => p.RaceStagePhotoFullFiles)
            .HasForeignKey(d => d.FullFileGuid)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_RaceStagePhoto_File");

        builder.HasOne(d => d.OpenThumbnailPhotoFile).WithMany(p => p.RaceStagePhotoOpenThumbnailFiles)
            .HasForeignKey(d => d.OpenThumbnailFileGuid)
            .HasConstraintName("FK_RaceStagePhoto_File1");

        builder.HasOne(d => d.Photographer).WithMany(p => p.RaceStagePhotos)
            .HasForeignKey(d => d.PhotographerGuid)
            .HasConstraintName("FK_RaceStagePhoto_Photographer");

        builder.HasOne(d => d.PreviewPhotoFile).WithMany(p => p.RaceStagePhotoPreviewFiles)
            .HasForeignKey(d => d.PreviewFileGuid)
            .HasConstraintName("FK_RaceStagePhoto_File2");

        builder.HasOne(d => d.RaceStage).WithMany(p => p.RaceStagePhotos)
            .HasForeignKey(d => d.RaceStageGuid)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_RaceStagePhoto_RaceStage");

        builder.HasOne(d => d.ThumbnailPhotoFile).WithMany(p => p.RaceStagePhotoThumbnailFiles)
            .HasForeignKey(d => d.ThumbnailFileGuid)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_RaceStagePhoto_File3");
    }
}