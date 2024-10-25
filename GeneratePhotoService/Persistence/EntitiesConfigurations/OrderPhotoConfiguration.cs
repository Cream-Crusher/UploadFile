using GeneratePhotoService.Models.ZelbikeChrono;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GeneratePhotoService.Persistence.EntitiesConfigurations;

public class OrderPhotoConfiguration : IEntityTypeConfiguration<OrderPhoto>
{
    public void Configure(EntityTypeBuilder<OrderPhoto> builder)
    {
        builder.HasKey(e => e.Guid);
        builder.ToTable("OrderPhoto");

        builder.Property(e => e.Guid).ValueGeneratedNever();
        builder.Property(e => e.Price).HasColumnType("money");

        builder.HasOne(d => d.Order).WithMany(p => p.OrderPhotos)
            .HasForeignKey(d => d.OrderGuid)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_OrderPhoto_Order");

        builder.HasOne(d => d.RaceStagePhoto).WithMany(p => p.OrderPhotos)
            .HasForeignKey(d => d.RaceStagePhotoGuid)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_OrderPhoto_RaceStagePhoto");
    }
}