using GeneratePhotoService.Models.ZelbikeChrono;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GeneratePhotoService.Persistence.EntitiesConfigurations;

public class RaceStageConfiguration : IEntityTypeConfiguration<RaceStage>
{
    public void Configure(EntityTypeBuilder<RaceStage> builder)
    {
        builder.HasKey(e => e.Guid);

        builder.ToTable("RaceStage");

        builder.Property(e => e.Guid).ValueGeneratedNever();
        builder.Property(e => e.AllPersonPhotosPrice).HasColumnType("money");
        builder.Property(e => e.City).HasMaxLength(100);
        builder.Property(e => e.CreatedDate).HasColumnType("datetime");
        builder.Property(e => e.DisplayNamePrimary).HasMaxLength(250);
        builder.Property(e => e.DisplayNameSecondary).HasMaxLength(250);
        builder.Property(e => e.DrivingDirectionsLink).HasMaxLength(250);
        builder.Property(e => e.ExpressAccessKey).HasMaxLength(50);
        builder.Property(e => e.LocationDetailed).HasMaxLength(500);
        builder.Property(e => e.LocationLatLong).HasMaxLength(50);
        builder.Property(e => e.LocationShort).HasMaxLength(150);
        builder.Property(e => e.PhotoProcessingRate).HasColumnType("decimal(18, 3)");
        builder.Property(e => e.PhotoSetPrice).HasColumnType("money");
        builder.Property(e => e.ProfileChangesTillDateTime).HasColumnType("datetime");
        builder.Property(e => e.Region).HasMaxLength(100);
        builder.Property(e => e.Settlement).HasMaxLength(100);
        builder.Property(e => e.SinglePhotoPrice).HasColumnType("money");
        builder.Property(e => e.StartDateTime).HasColumnType("datetime");
        builder.Property(e => e.UrlName).HasMaxLength(250);

        builder.HasOne(d => d.Race).WithMany(p => p.RaceStages)
            .HasForeignKey(d => d.RaceGuid)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_RaceStage_Race1");
    }
}