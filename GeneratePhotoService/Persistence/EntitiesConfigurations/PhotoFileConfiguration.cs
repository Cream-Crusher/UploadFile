using GeneratePhotoService.Models.ZelbikeChrono;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GeneratePhotoService.Persistence.EntitiesConfigurations;

public class PhotoFileConfiguration : IEntityTypeConfiguration<PhotoFile>
{
    public void Configure(EntityTypeBuilder<PhotoFile> builder)
    {
        builder.HasKey(e => e.Guid);
        builder.ToTable("File");

        builder.Property(e => e.Guid).ValueGeneratedNever();
        builder.Property(e => e.ContentType)
            .IsRequired()
            .HasMaxLength(250);
        builder.Property(e => e.ExpireDate)
            .HasColumnType("datetime");
        builder.Property(e => e.Extension)
            .HasMaxLength(50);
        builder.Property(e => e.ExternalUrl)
            .HasMaxLength(1000);
        builder.Property(e => e.LastUpdatedDate)
            .HasColumnType("datetime");
        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(500);
        builder.Property(e => e.SelectelBucketName)
            .HasMaxLength(50);
        builder.Property(e => e.SelectelFileKey)
            .HasMaxLength(1000);

        // todo: rework virtual collections and props
        // builder.HasOne(d => d.Organization).WithMany(p => p.Files)
        //     .HasForeignKey(d => d.OrganizationGuid)
        //     .HasConstraintName("FK_File_Organization");
    }
}