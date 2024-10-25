using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace LimetimePhotoUploadUI.Models.ZelbikeChrono;

public partial class ZelbikeChronoContext : DbContext
{
    public ZelbikeChronoContext()
    {
    }

    public ZelbikeChronoContext(DbContextOptions<ZelbikeChronoContext> options)
        : base(options)
    {
    }

    public virtual DbSet<File> Files { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderPhoto> OrderPhotos { get; set; }

    public virtual DbSet<Organization> Organizations { get; set; }

    public virtual DbSet<Photographer> Photographers { get; set; }

    public virtual DbSet<Race> Races { get; set; }

    public virtual DbSet<RaceOrganization> RaceOrganizations { get; set; }

    public virtual DbSet<RaceStage> RaceStages { get; set; }

    public virtual DbSet<RaceStagePhoto> RaceStagePhotos { get; set; }

    public virtual DbSet<RaceStagePhotoPerson> RaceStagePhotoPeople { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<File>(entity =>
        {
            entity.HasKey(e => e.Guid);

            entity.ToTable("File");

            entity.Property(e => e.Guid).ValueGeneratedNever();
            entity.Property(e => e.ContentType)
                .IsRequired()
                .HasMaxLength(250);
            entity.Property(e => e.ExpireDate).HasColumnType("datetime");
            entity.Property(e => e.Extension).HasMaxLength(50);
            entity.Property(e => e.ExternalUrl).HasMaxLength(1000);
            entity.Property(e => e.LastUpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(500);
            entity.Property(e => e.SelectelBucketName).HasMaxLength(50);
            entity.Property(e => e.SelectelFileKey).HasMaxLength(1000);

            entity.HasOne(d => d.Organization).WithMany(p => p.Files)
                .HasForeignKey(d => d.OrganizationGuid)
                .HasConstraintName("FK_File_Organization");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Guid);

            entity.ToTable("Order");

            entity.Property(e => e.Guid).ValueGeneratedNever();
            entity.Property(e => e.AccountBalanceLocked).HasColumnType("money");
            entity.Property(e => e.AccountBalanceUsed).HasColumnType("money");
            entity.Property(e => e.AlfabankPaymentId).HasMaxLength(50);
            entity.Property(e => e.AlfabankPaymentUrl).HasMaxLength(2000);
            entity.Property(e => e.BankPaymentProvider).HasMaxLength(50);
            entity.Property(e => e.BankPaymentSystem).HasMaxLength(50);
            entity.Property(e => e.BankPaymentWay).HasMaxLength(50);
            entity.Property(e => e.BankProductCategory).HasMaxLength(50);
            entity.Property(e => e.Campaign).HasMaxLength(250);
            entity.Property(e => e.CompletedDate).HasColumnType("datetime");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Donation).HasColumnType("money");
            entity.Property(e => e.DonationProcessingRate).HasColumnType("decimal(18, 3)");
            entity.Property(e => e.LimetimePhotoAccessKey).HasMaxLength(50);
            entity.Property(e => e.MerchProgessingRate).HasColumnType("decimal(18, 3)");
            entity.Property(e => e.Notes).HasMaxLength(400);
            entity.Property(e => e.PageUrl).HasMaxLength(2000);
            entity.Property(e => e.Price).HasColumnType("money");
            entity.Property(e => e.ProcessingRate).HasColumnType("decimal(18, 3)");
            entity.Property(e => e.PromoCode).HasMaxLength(50);
            entity.Property(e => e.RecoveryDate).HasColumnType("datetime");
            entity.Property(e => e.ReferrerUrl).HasMaxLength(4000);
            entity.Property(e => e.RefundAmount).HasColumnType("money");
            entity.Property(e => e.RefundDate).HasColumnType("datetime");
            entity.Property(e => e.SberbankOrderId).HasMaxLength(50);
            entity.Property(e => e.SberbankPayUrl).HasMaxLength(2000);
            entity.Property(e => e.TinkoffPaymentId).HasMaxLength(50);
            entity.Property(e => e.TinkoffPaymentUrl).HasMaxLength(2000);
        });

        modelBuilder.Entity<OrderPhoto>(entity =>
        {
            entity.HasKey(e => e.Guid);

            entity.ToTable("OrderPhoto");

            entity.Property(e => e.Guid).ValueGeneratedNever();
            entity.Property(e => e.Price).HasColumnType("money");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderPhotos)
                .HasForeignKey(d => d.OrderGuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderPhoto_Order");

            entity.HasOne(d => d.RaceStagePhoto).WithMany(p => p.OrderPhotos)
                .HasForeignKey(d => d.RaceStagePhotoGuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderPhoto_RaceStagePhoto");
        });

        modelBuilder.Entity<Organization>(entity =>
        {
            entity.HasKey(e => e.Guid);

            entity.ToTable("Organization");

            entity.Property(e => e.Guid).ValueGeneratedNever();
            entity.Property(e => e.Balance).HasColumnType("money");
            entity.Property(e => e.CustomDomain).HasMaxLength(250);
            entity.Property(e => e.DisplayName)
                .IsRequired()
                .HasMaxLength(250);
            entity.Property(e => e.DonationProcessingRate).HasColumnType("decimal(18, 3)");
            entity.Property(e => e.EmailPrice).HasColumnType("money");
            entity.Property(e => e.FacebookAppId).HasMaxLength(250);
            entity.Property(e => e.FacebookAppKey).HasMaxLength(250);
            entity.Property(e => e.Features).HasMaxLength(500);
            entity.Property(e => e.OrderProcessingRate).HasColumnType("decimal(18, 3)");
            entity.Property(e => e.PhotoProcessingPrice).HasColumnType("decimal(18, 3)");
            entity.Property(e => e.ReplyEmailAddress).HasMaxLength(150);
            entity.Property(e => e.SmsPrice).HasColumnType("money");
            entity.Property(e => e.UrlName).HasMaxLength(250);
            entity.Property(e => e.VisualTemplateName).HasMaxLength(150);
            entity.Property(e => e.VkontakteAppId).HasMaxLength(250);
            entity.Property(e => e.VkontakteAppKey).HasMaxLength(250);
        });

        modelBuilder.Entity<Photographer>(entity =>
        {
            entity.HasKey(e => e.Guid);

            entity.ToTable("Photographer");

            entity.Property(e => e.Guid).ValueGeneratedNever();
            entity.Property(e => e.AccessKey)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.DisplayName)
                .IsRequired()
                .HasMaxLength(150);
            entity.Property(e => e.EditableTillDate).HasColumnType("datetime");
            entity.Property(e => e.TolokaPoolId).HasMaxLength(50);

            entity.HasOne(d => d.RaceStage).WithMany(p => p.Photographers)
                .HasForeignKey(d => d.RaceStageGuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Photographer_RaceStage");
        });

        modelBuilder.Entity<Race>(entity =>
        {
            entity.HasKey(e => e.Guid);

            entity.ToTable("Race");

            entity.Property(e => e.Guid).ValueGeneratedNever();
            entity.Property(e => e.BannerBackgroundColor).HasMaxLength(10);
            entity.Property(e => e.BannerContentHorizontalPosition).HasMaxLength(50);
            entity.Property(e => e.BannerContentOpacity).HasMaxLength(50);
            entity.Property(e => e.BannerContentStyle).HasMaxLength(50);
            entity.Property(e => e.BannerContentType).HasMaxLength(50);
            entity.Property(e => e.BannerContentVerticalPosition).HasMaxLength(50);
            entity.Property(e => e.BannerImageUrl).HasMaxLength(500);
            entity.Property(e => e.BannerPosition).HasMaxLength(50);
            entity.Property(e => e.BannerStyle).HasMaxLength(50);
            entity.Property(e => e.BannerThumbnailUrl).HasMaxLength(500);
            entity.Property(e => e.ContactEmail).HasMaxLength(50);
            entity.Property(e => e.ContactPhone).HasMaxLength(50);
            entity.Property(e => e.DisplayNamePrimary)
                .IsRequired()
                .HasMaxLength(250);
            entity.Property(e => e.DisplayNameSecondary).HasMaxLength(250);
            entity.Property(e => e.UrlName).HasMaxLength(250);
        });

        modelBuilder.Entity<RaceOrganization>(entity =>
        {
            entity.HasKey(e => new { e.OrganizationGuid, e.RaceGuid });

            entity.ToTable("RaceOrganization");

            entity.HasOne(d => d.Organization).WithMany(p => p.RaceOrganizations)
                .HasForeignKey(d => d.OrganizationGuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RaceOrganization_Organization");

            entity.HasOne(d => d.Race).WithMany(p => p.RaceOrganizations)
                .HasForeignKey(d => d.RaceGuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RaceOrganization_Race");
        });

        modelBuilder.Entity<RaceStage>(entity =>
        {
            entity.HasKey(e => e.Guid);

            entity.ToTable("RaceStage");

            entity.Property(e => e.Guid).ValueGeneratedNever();
            entity.Property(e => e.AllPersonPhotosPrice).HasColumnType("money");
            entity.Property(e => e.City).HasMaxLength(100);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.DisplayNamePrimary).HasMaxLength(250);
            entity.Property(e => e.DisplayNameSecondary).HasMaxLength(250);
            entity.Property(e => e.DrivingDirectionsLink).HasMaxLength(250);
            entity.Property(e => e.ExpressAccessKey).HasMaxLength(50);
            entity.Property(e => e.LocationDetailed).HasMaxLength(500);
            entity.Property(e => e.LocationLatLong).HasMaxLength(50);
            entity.Property(e => e.LocationShort).HasMaxLength(150);
            entity.Property(e => e.PhotoProcessingRate).HasColumnType("decimal(18, 3)");
            entity.Property(e => e.PhotoSetPrice).HasColumnType("money");
            entity.Property(e => e.ProfileChangesTillDateTime).HasColumnType("datetime");
            entity.Property(e => e.Region).HasMaxLength(100);
            entity.Property(e => e.Settlement).HasMaxLength(100);
            entity.Property(e => e.SinglePhotoPrice).HasColumnType("money");
            entity.Property(e => e.StartDateTime).HasColumnType("datetime");
            entity.Property(e => e.UrlName).HasMaxLength(250);

            entity.HasOne(d => d.Race).WithMany(p => p.RaceStages)
                .HasForeignKey(d => d.RaceGuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RaceStage_Race1");
        });

        modelBuilder.Entity<RaceStagePhoto>(entity =>
        {
            entity.HasKey(e => e.Guid);

            entity.ToTable("RaceStagePhoto");

            entity.Property(e => e.Guid).ValueGeneratedNever();
            entity.Property(e => e.AiVisionJson).HasMaxLength(4000);
            entity.Property(e => e.AitaggedRegistrationNumbers)
                .HasMaxLength(500)
                .HasColumnName("AITaggedRegistrationNumbers");
            entity.Property(e => e.HumanTaggedRegistrationNumbers).HasMaxLength(500);
            entity.Property(e => e.RegistrationNumbers).HasMaxLength(100);
            entity.Property(e => e.Tags).HasMaxLength(500);
            entity.Property(e => e.TeamNumbers).HasMaxLength(100);

            entity.HasOne(d => d.FullFile).WithMany(p => p.RaceStagePhotoFullFiles)
                .HasForeignKey(d => d.FullFileGuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RaceStagePhoto_File");

            entity.HasOne(d => d.OpenThumbnailFile).WithMany(p => p.RaceStagePhotoOpenThumbnailFiles)
                .HasForeignKey(d => d.OpenThumbnailFileGuid)
                .HasConstraintName("FK_RaceStagePhoto_File1");

            entity.HasOne(d => d.Photographer).WithMany(p => p.RaceStagePhotos)
                .HasForeignKey(d => d.PhotographerGuid)
                .HasConstraintName("FK_RaceStagePhoto_Photographer");

            entity.HasOne(d => d.PreviewFile).WithMany(p => p.RaceStagePhotoPreviewFiles)
                .HasForeignKey(d => d.PreviewFileGuid)
                .HasConstraintName("FK_RaceStagePhoto_File2");

            entity.HasOne(d => d.RaceStage).WithMany(p => p.RaceStagePhotos)
                .HasForeignKey(d => d.RaceStageGuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RaceStagePhoto_RaceStage");

            entity.HasOne(d => d.ThumbnailFile).WithMany(p => p.RaceStagePhotoThumbnailFiles)
                .HasForeignKey(d => d.ThumbnailFileGuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RaceStagePhoto_File3");
        });

        modelBuilder.Entity<RaceStagePhotoPerson>(entity =>
        {
            entity.HasKey(e => e.Guid);

            entity.ToTable("RaceStagePhotoPerson");

            entity.Property(e => e.Guid).ValueGeneratedNever();

            entity.HasOne(d => d.RaceStagePhoto).WithMany(p => p.RaceStagePhotoPeople)
                .HasForeignKey(d => d.RaceStagePhotoGuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RaceStagePhotoPerson_RaceStagePhoto");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
