using System;
using DrzewaAPI.Models;
using DrzewaAPI.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace DrzewaAPI.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
	public DbSet<User> Users { get; set; }
	public DbSet<TreeSubmission> TreeSubmissions { get; set; }
	public DbSet<TreeSpecies> TreeSpecies { get; set; }
	public DbSet<Application> Applications { get; set; }
	public DbSet<Municipality> Municipalities { get; set; }
	public DbSet<Comment> Comments { get; set; }
	public DbSet<Vote> Votes { get; set; }
	public DbSet<Notification> Notifications { get; set; }
	public DbSet<TreeSubmissionAttachment> TreeSubmissionAttachments { get; set; }
	public DbSet<SpeciesImage> SpeciesImages { get; set; }
	public DbSet<TreeSpeciesImages> TreeSpeciesImages { get; set; }
	public DbSet<Tag> Tags { get; set; }
	public DbSet<SpeciesAdditionRequest> SpeciesAdditionRequests { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		// User Configuration
		modelBuilder.Entity<User>(entity =>
		{
			entity.HasKey(e => e.Id);
			entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
			entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
			entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
			entity.Property(e => e.PasswordHash).IsRequired();
			entity.Property(e => e.Phone).HasMaxLength(15);
			entity.Property(e => e.Avatar).HasMaxLength(500);
			entity.Property(e => e.RegistrationDate).HasDefaultValueSql("GETUTCDATE()");
			entity.Property(e => e.Role).HasDefaultValue(UserRole.User);
			entity.Property(e => e.SubmissionsCount).HasDefaultValue(0);
			entity.Property(e => e.VerificationsCount).HasDefaultValue(0);
			entity.HasIndex(e => e.Email).IsUnique();
		});

		// TreeReport Configuration
		modelBuilder.Entity<TreeSubmission>(entity =>
		{
			entity.HasKey(e => e.Id);
			entity.Property(e => e.Circumference).HasMaxLength(6);
			entity.Property(e => e.Description).HasMaxLength(2000);

			entity.HasOne(e => e.User)
									.WithMany(e => e.TreeSubmissions)
									.HasForeignKey(e => e.UserId)
									.OnDelete(DeleteBehavior.Restrict);

			entity.HasOne(e => e.Species)
									.WithMany(e => e.TreeSubmissions)
									.HasForeignKey(e => e.SpeciesId)
									.OnDelete(DeleteBehavior.Restrict);
		});

		// TreeSpecies Configuration
		modelBuilder.Entity<TreeSpecies>(entity =>
		{
			entity.HasKey(e => e.Id);
			entity.Property(e => e.PolishName).IsRequired().HasMaxLength(200);
			entity.Property(e => e.LatinName).IsRequired().HasMaxLength(200);
			entity.Property(e => e.Description).HasMaxLength(2000);
			entity.Property(e => e.IdentificationGuide).HasMaxLength(5000);
			entity.Property(e => e.SeasonalChanges).HasMaxLength(2000);
		});

		// Vote Configuration - Unique constraint
		modelBuilder.Entity<Vote>(entity =>
		{
			entity.HasKey(e => e.Id);
			entity.HasIndex(e => new { e.UserId, e.TreeSubmissionId }).IsUnique();

			entity.HasOne(e => e.User)
									.WithMany(e => e.Votes)
									.HasForeignKey(e => e.UserId)
									.OnDelete(DeleteBehavior.Cascade);

			entity.HasOne(e => e.TreeSubmission)
									.WithMany(e => e.Votes)
									.HasForeignKey(e => e.TreeSubmissionId)
									.OnDelete(DeleteBehavior.Cascade);
		});

		// Many-to-Many Relationships
		modelBuilder.Entity<TreeConditionTags>(entity =>
		{
			entity.HasKey(e => new { e.TagId, e.TreeSubmissionId });

			entity.HasOne(e => e.Tags)
									.WithMany(e => e.ConditionTags)
									.HasForeignKey(e => e.TagId);

			entity.HasOne(e => e.TreeSubmissions)
									.WithMany(e => e.ConditionTags)
									.HasForeignKey(e => e.TreeSubmissionId);
		});

		modelBuilder.Entity<TreeSpeciesImages>(entity =>
		{
			entity.HasKey(e => new { e.TreeSpeciesId, e.ImageId });

			entity.HasOne(e => e.TreeSpecies)
									.WithMany(e => e.TreeSpeciesImages)
									.HasForeignKey(e => e.TreeSpeciesId);

			entity.HasOne(e => e.SpeciesImage)
									.WithMany(e => e.TreeSpeciesImages)
									.HasForeignKey(e => e.ImageId);
		});
	}
};
