using System;
using DrzewaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DrzewaAPI.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
	public DbSet<User> Users { get; set; }
	public DbSet<TreeReport> TreeReports { get; set; }
	public DbSet<TreeSpecies> TreeSpecies { get; set; }
	public DbSet<Application> Applications { get; set; }
	public DbSet<Municipality> Municipalities { get; set; }
	public DbSet<Comment> Comments { get; set; }
	public DbSet<Vote> Votes { get; set; }
	public DbSet<Notification> Notifications { get; set; }
	public DbSet<TreeReportAttachment> TreeReportAttachments { get; set; }
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
			entity.Property(e => e.Phone).HasMaxLength(20);
			entity.HasIndex(e => e.Email).IsUnique();
		});

		// TreeReport Configuration
		modelBuilder.Entity<TreeReport>(entity =>
		{
			entity.HasKey(e => e.Id);
			entity.Property(e => e.Circumference).HasColumnType("int(10)");
			entity.Property(e => e.LocationDescription).HasMaxLength(500);
			entity.Property(e => e.Description).HasMaxLength(2000);
			entity.Property(e => e.FeaturedLegend).HasMaxLength(1000);

			entity.HasOne(e => e.User)
									.WithMany(e => e.TreeReports)
									.HasForeignKey(e => e.UserId)
									.OnDelete(DeleteBehavior.Restrict);

			entity.HasOne(e => e.Species)
									.WithMany(e => e.TreeReports)
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
			entity.HasIndex(e => new { e.UserId, e.TreeReportId }).IsUnique();

			entity.HasOne(e => e.User)
									.WithMany(e => e.Votes)
									.HasForeignKey(e => e.UserId)
									.OnDelete(DeleteBehavior.Cascade);

			entity.HasOne(e => e.TreeReport)
									.WithMany(e => e.Votes)
									.HasForeignKey(e => e.TreeReportId)
									.OnDelete(DeleteBehavior.Cascade);
		});

		// Many-to-Many Relationships
		modelBuilder.Entity<TreeConditionTags>(entity =>
		{
			entity.HasKey(e => new { e.TagId, e.TreeReportId });

			entity.HasOne(e => e.Tags)
									.WithMany(e => e.ConditionTags)
									.HasForeignKey(e => e.TagId);

			entity.HasOne(e => e.TreeReports)
									.WithMany(e => e.ConditionTags)
									.HasForeignKey(e => e.TreeReportId);
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

		// Seed initial data
		SeedData(modelBuilder);
	}

	private void SeedData(ModelBuilder modelBuilder)
	{
		// Seed Users
		Guid userId = Guid.NewGuid();
		SetUserData(modelBuilder, userId);

		// Seed Tree Species
		Guid oakId = Guid.NewGuid();
		Guid pineId = Guid.NewGuid();

		SetSpeciesData(modelBuilder, oakId, pineId);

		// Seed Reports
		Guid treeReportOneId = Guid.NewGuid();
		Guid treeReportTwoId = Guid.NewGuid();

		SetReportData(modelBuilder, treeReportOneId, treeReportTwoId, oakId, pineId, userId);

		// Seed sample municipalities
		modelBuilder.Entity<Municipality>().HasData(
				new Municipality
				{
					Id = Guid.NewGuid(),
					Name = "GminaWarszawska",
					City = "Warszawa",
					Province = "Mazowieckie",
					Address = "Street 15",
					ZipCode = "24-040",
					Email = "urząd@warszawa.pl"
				}
		);
	}


	private void SetUserData(ModelBuilder modelBuilder, Guid userId)
	{
		modelBuilder.Entity<User>().HasData(
				new User
				{
					Id = userId,
					FirstName = "Eko",
					LastName = "Wojownik",
					Email = "ekowojownik@gmail.com",
					PasswordHash = "VerySafe",
					CreatedAt = DateTime.Now,
				}
		);
	}

	private void SetSpeciesData(ModelBuilder modelBuilder, Guid oakId, Guid pineId)
	{
		modelBuilder.Entity<TreeSpecies>().HasData(
					new TreeSpecies
					{
						Id = oakId,
						PolishName = "Dąb szypułkowy",
						LatinName = "Quercus robur",
						Description = "Dąb szypułkowy to jeden z najpopularniejszych gatunków drzew w Polsce.",
						Category = Utils.SpeciesCategory.Coniferous,
					},
					new TreeSpecies
					{
						Id = pineId,
						PolishName = "Sosna zwyczajna",
						LatinName = "Pinus sylvestris",
						Description = "Sosna zwyczajna to charakterystyczne drzewo iglaste występujące w Polsce.",
						Category = Utils.SpeciesCategory.Deciduous,
					}
			);

		// Seed Images
		Guid imageOneId = Guid.NewGuid();
		Guid imageTwoId = Guid.NewGuid();

		modelBuilder.Entity<SpeciesImage>().HasData(
				new SpeciesImage
				{
					Id = imageOneId,
					ImageUrl = "fileUrl",
					Type = Utils.ImageType.Tree,
					Description = "Zdjęcie dęba szypułkowego"
				},
				new SpeciesImage
				{
					Id = imageTwoId,
					ImageUrl = "fileUrl",
					Type = Utils.ImageType.Leaf,
					Description = "Zdjęcie liścia sosny zwyczajnej"
				}
		);

		// Connection between TreeSpecies and Images
		modelBuilder.Entity<TreeSpeciesImages>().HasData(
				new TreeSpeciesImages
				{
					TreeSpeciesId = oakId,
					ImageId = imageOneId
				},
				new TreeSpeciesImages
				{
					TreeSpeciesId = pineId,
					ImageId = imageTwoId
				}
		);
	}

	private void SetReportData(ModelBuilder modelBuilder, Guid treeReportOneId, Guid treeReportTwoId, Guid oakId, Guid pineId, Guid userId)
	{
		// Seed Report
		modelBuilder.Entity<TreeReport>().HasData(
				new TreeReport
				{
					Id = treeReportOneId,
					SpeciesId = oakId,
					UserId = userId,
					Latitude = 500,
					Longitude = 100,
					LocationDescription = "Miasto: Warszawa\nUlica: 3 maja",
					Circumference = 300,
					IsAlive = true,
					EstimatedAge = 150,
					Description = "Fajne drzewo",
					IsNatureMonument = true,
					Status = Utils.ReportStatus.Approved,
					CreatedAt = new DateTime(2025, 6, 15),
					IsVerified = true,
				},
				new TreeReport
				{
					Id = treeReportTwoId,
					SpeciesId = pineId,
					UserId = userId,
					Latitude = 400,
					Longitude = 210,
					LocationDescription = "Miasto: Warszawa\nUlica: Wyspiańskiego",
					Circumference = 220,
					IsAlive = false,
					EstimatedAge = 240,
					Description = "Martwe drzewo",
					CreatedAt = new DateTime(2025, 4, 1),
				}
		);

		// Seed Attachments
		Guid attachmentOneId = Guid.NewGuid();
		Guid attachmentTwoId = Guid.NewGuid();
		Guid attachmentThreeId = Guid.NewGuid();

		modelBuilder.Entity<TreeReportAttachment>().HasData(
			new TreeReportAttachment
			{
				Id = attachmentOneId,
				TreeReportId = treeReportOneId,
				FileName = "Obrazek pnia dębu",
				FileUrl = "fileUrl",
			},
			new TreeReportAttachment
			{
				Id = attachmentTwoId,
				TreeReportId = treeReportOneId,
				FileName = "Obrazek liścia dębu",
				FileUrl = "fileUrl",
			},
			new TreeReportAttachment
			{
				Id = attachmentThreeId,
				TreeReportId = treeReportTwoId,
				FileName = "Obrazek pnia sosny",
				FileUrl = "fileUrl",
			}
		);

		// Seed Tags
		Guid droughtId = Guid.NewGuid();
		Guid unstableId = Guid.NewGuid();

		modelBuilder.Entity<Tag>().HasData(
				new Tag { Id = droughtId, Name = "Posusz" },
				new Tag { Id = unstableId, Name = "Niestabilny" }
		);

		// Connection between Reports and Tags
		modelBuilder.Entity<TreeConditionTags>().HasData(
			new TreeConditionTags
			{
				TreeReportId = treeReportOneId,
				TagId = droughtId
			},
			new TreeConditionTags
			{
				TreeReportId = treeReportOneId,
				TagId = unstableId
			}
		);
	}
};