using GeneratePhotoService.Models.ZelbikeChrono;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GeneratePhotoService.Persistence.EntitiesConfigurations;

public class RaceOrganizationConfiguration : IEntityTypeConfiguration<RaceOrganization>
{
    public void Configure(EntityTypeBuilder<RaceOrganization> builder)
    {
        builder.HasKey(e => new { e.OrganizationGuid, e.RaceGuid });

        builder.ToTable("RaceOrganization");

        builder.HasOne(d => d.Organization).WithMany(p => p.RaceOrganizations)
            .HasForeignKey(d => d.OrganizationGuid)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_RaceOrganization_Organization");

        builder.HasOne(d => d.Race).WithMany(p => p.RaceOrganizations)
            .HasForeignKey(d => d.RaceGuid)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_RaceOrganization_Race");
    }
}