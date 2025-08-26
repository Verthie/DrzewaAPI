using DrzewaAPI.Models;
using DrzewaAPI.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace DrzewaAPI.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
	public DbSet<User> Users { get; set; }
	public DbSet<TreeSubmission> TreeSubmissions { get; set; }
	public DbSet<TreeSpecies> TreeSpecies { get; set; }
	public DbSet<TreeSpeciesImage> TreeSpeciesImages { get; set; }
	public DbSet<Vote> Votes { get; set; }
	public DbSet<Comment> Comments { get; set; }
	public DbSet<Like> Likes { get; set; }

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
			entity.HasIndex(e => e.Email).IsUnique();
		});

		// TreeReport Configuration
		modelBuilder.Entity<TreeSubmission>(entity =>
		{
			entity.HasKey(e => e.Id);
			entity.Property(e => e.Circumference).HasMaxLength(6);
			entity.Property(e => e.Description).HasMaxLength(2000);
			entity.OwnsOne(e => e.Location);

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
			entity.OwnsOne(e => e.SeasonalChanges);
			entity.OwnsOne(e => e.Traits);
		});

		// TreeSpeciesImage Configuration 
		modelBuilder.Entity<TreeSpeciesImage>(entity =>
		{
			entity.HasKey(e => e.Id);
			entity.Property(e => e.ImageUrl).IsRequired();
			entity.HasOne(e => e.TreeSpecies)
									.WithMany(e => e.Images)
									.HasForeignKey(e => e.TreeSpeciesId)
									.OnDelete(DeleteBehavior.Restrict);
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

		// Comment Configuration
		modelBuilder.Entity<Comment>(entity =>
		{
			entity.HasKey(e => e.Id);
			entity.Property(e => e.Content).IsRequired().HasMaxLength(300);

			entity.HasOne(e => e.User)
									.WithMany(e => e.Comments)
									.HasForeignKey(e => e.UserId)
									.OnDelete(DeleteBehavior.Cascade);

			entity.HasOne(e => e.TreeSubmission)
									.WithMany(e => e.Comments)
									.HasForeignKey(e => e.TreeSubmissionId)
									.OnDelete(DeleteBehavior.Cascade);
		});

		// Like Configuration
		modelBuilder.Entity<Like>(entity =>
		{
			entity.HasKey(e => e.Id);
			entity.HasIndex(e => new { e.UserId, e.CommentId }).IsUnique();

			entity.HasOne(e => e.User)
									.WithMany(e => e.Likes)
									.HasForeignKey(e => e.UserId)
									.OnDelete(DeleteBehavior.NoAction);

			entity.HasOne(e => e.Comment)
									.WithMany(e => e.Likes)
									.HasForeignKey(e => e.CommentId)
									.OnDelete(DeleteBehavior.Cascade);
		});
	}
};
