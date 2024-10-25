using GeneratePhotoService.Models.ZelbikeChrono;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GeneratePhotoService.Persistence.EntitiesConfigurations;

public class PhotographerConfiguration : IEntityTypeConfiguration<Photographer>
{
    public void Configure(EntityTypeBuilder<Photographer> builder)
    {
        builder.HasKey(e => e.Guid);

        builder.ToTable("Photographer");

        builder.Property(e => e.Guid).ValueGeneratedNever();
        builder.Property(e => e.AccessKey)
            .IsRequired()
            .HasMaxLength(50);
        builder.Property(e => e.DisplayName)
            .IsRequired()
            .HasMaxLength(150);
        builder.Property(e => e.EditableTillDate).HasColumnType("datetime");
        builder.Property(e => e.TolokaPoolId).HasMaxLength(50);

        builder.HasOne(d => d.RaceStage).WithMany(p => p.Photographers)
            .HasForeignKey(d => d.RaceStageGuid)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Photographer_RaceStage");
    }
}