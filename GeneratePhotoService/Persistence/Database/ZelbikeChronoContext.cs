using GeneratePhotoService.Models.ZelbikeChrono;
using GeneratePhotoService.Persistence.EntitiesConfigurations;
using Microsoft.EntityFrameworkCore;

namespace GeneratePhotoService.Persistence.Database;

public class ZelbikeChronoContext : DbContext
{
    public ZelbikeChronoContext(DbContextOptions<ZelbikeChronoContext> options) : base(options)
    {
    }

    public DbSet<PhotoFile> Files { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderPhoto> OrderPhotos { get; set; }
    public DbSet<Organization> Organizations { get; set; }
    public DbSet<Photographer> Photographers { get; set; }
    public DbSet<Race> Races { get; set; }
    public DbSet<RaceOrganization> RaceOrganizations { get; set; }
    public DbSet<RaceStage> RaceStages { get; set; }
    public DbSet<RaceStagePhoto> RaceStagePhotos { get; set; }
    public DbSet<RaceStagePhotoPerson> RaceStagePhotoPeople { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new OrderConfiguration());
        builder.ApplyConfiguration(new OrderPhotoConfiguration());
        
        builder.ApplyConfiguration(new OrganizationConfiguration());
        builder.ApplyConfiguration(new PhotoFileConfiguration());
        builder.ApplyConfiguration(new PhotographerConfiguration());
        
        builder.ApplyConfiguration(new RaceConfiguration());
        builder.ApplyConfiguration(new RaceOrganizationConfiguration());
        builder.ApplyConfiguration(new RaceStageConfiguration());
        builder.ApplyConfiguration(new RaceStagePhotoConfiguration());
        builder.ApplyConfiguration(new RaceStagePhotoPersonConfiguration());
        
        base.OnModelCreating(builder);
    }
}
