using GeneratePhotoService.Models.ZelbikeChrono;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GeneratePhotoService.Persistence.EntitiesConfigurations;

public class RaceStagePhotoPersonConfiguration : IEntityTypeConfiguration<RaceStagePhotoPerson>
{
    public void Configure(EntityTypeBuilder<RaceStagePhotoPerson> builder)
    {
        builder.HasKey(e => e.Guid);
        builder.ToTable("RaceStagePhotoPerson");
        builder.Property(e => e.Guid).ValueGeneratedNever();
        builder.HasOne(d => d.RaceStagePhoto).WithMany(p => p.RaceStagePhotoPeople)
            .HasForeignKey(d => d.RaceStagePhotoGuid)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_RaceStagePhotoPerson_RaceStagePhoto");
    }
}