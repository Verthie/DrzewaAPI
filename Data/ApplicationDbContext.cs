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
	public DbSet<Vote> Votes { get; set; }

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
			entity.OwnsOne(e => e.Location, loc =>
			{
				loc.Property(p => p.Lat).HasColumnName("Lat").IsRequired();
				loc.Property(p => p.Lng).HasColumnName("Lng").IsRequired();
				loc.Property(p => p.Address).HasColumnName("Address").IsRequired();
			});

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
	}
};
