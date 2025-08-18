using System;
using DrzewaAPI.Models;
using DrzewaAPI.Models.Enums;
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

	// Static GUIDs (constant across builds)
	private static readonly Guid SeedUserId = new Guid("11111111-1111-1111-1111-111111111111");
	private static readonly Guid SeedOakId = new Guid("22222222-2222-2222-2222-222222222222");
	private static readonly Guid SeedPineId = new Guid("33333333-3333-3333-3333-333333333333");
	private static readonly Guid SeedTreeReportOneId = new Guid("44444444-4444-4444-4444-444444444444");
	private static readonly Guid SeedTreeReportTwoId = new Guid("55555555-5555-5555-5555-555555555555");
	private static readonly Guid SeedImageOneId = new Guid("66666666-6666-6666-6666-666666666666");
	private static readonly Guid SeedImageTwoId = new Guid("77777777-7777-7777-7777-777777777777");
	private static readonly Guid SeedAttachmentOneId = new Guid("88888888-8888-8888-8888-888888888888");
	private static readonly Guid SeedAttachmentTwoId = new Guid("99999999-9999-9999-9999-999999999999");
	private static readonly Guid SeedAttachmentThreeId = new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
	private static readonly Guid SeedDroughtTagId = new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");
	private static readonly Guid SeedUnstableTagId = new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc");
	private static readonly Guid SeedMunicipalityId = new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd");

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
			entity.Property(e => e.Circumference).HasMaxLength(6);
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
		SetUserData(modelBuilder);

		// Seed Tree Species
		SetSpeciesData(modelBuilder);

		// Seed Reports
		SetReportData(modelBuilder);

		// Seed sample municipalities
		modelBuilder.Entity<Municipality>().HasData(
				new Municipality
				{
					Id = SeedMunicipalityId,
					Name = "GminaWarszawska",
					City = "Warszawa",
					Province = "Mazowieckie",
					Address = "Street 15",
					ZipCode = "24-040",
					Email = "urząd@warszawa.pl"
				}
		);
	}


	private void SetUserData(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<User>().HasData(
				new User
				{
					Id = SeedUserId,
					FirstName = "Eko",
					LastName = "Wojownik",
					Email = "ekowojownik@gmail.com",
					PasswordHash = "VerySafe",
					RegistrationDate = new DateTime(2025, 1, 1),
				}
		);
	}

	private void SetSpeciesData(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<TreeSpecies>().HasData(
					new TreeSpecies
					{
						Id = SeedOakId,
						PolishName = "Dąb szypułkowy",
						LatinName = "Quercus robur",
						Family = "Fagaceae",
						Description = "Dąb szypułkowy to jeden z najpopularniejszych gatunków drzew w Polsce.",
						Category = SpeciesCategory.Coniferous,
					},
					new TreeSpecies
					{
						Id = SeedPineId,
						PolishName = "Sosna zwyczajna",
						LatinName = "Pinus sylvestris",
						Family = "Pinaceae",
						Description = "Sosna zwyczajna to charakterystyczne drzewo iglaste występujące w Polsce.",
						Category = SpeciesCategory.Deciduous,
					}
			);

		// Seed Images
		modelBuilder.Entity<SpeciesImage>().HasData(
				new SpeciesImage
				{
					Id = SeedImageOneId,
					ImageUrl = "fileUrl",
					Type = ImageType.Tree,
					Description = "Zdjęcie dęba szypułkowego"
				},
				new SpeciesImage
				{
					Id = SeedImageTwoId,
					ImageUrl = "fileUrl",
					Type = ImageType.Leaf,
					Description = "Zdjęcie liścia sosny zwyczajnej"
				}
		);

		// Connection between TreeSpecies and Images
		modelBuilder.Entity<TreeSpeciesImages>().HasData(
				new TreeSpeciesImages
				{
					TreeSpeciesId = SeedOakId,
					ImageId = SeedImageOneId
				},
				new TreeSpeciesImages
				{
					TreeSpeciesId = SeedPineId,
					ImageId = SeedImageTwoId
				}
		);
	}

	private void SetReportData(ModelBuilder modelBuilder)
	{
		// Seed Report
		modelBuilder.Entity<TreeReport>().HasData(
				new TreeReport
				{
					Id = SeedTreeReportOneId,
					SpeciesId = SeedOakId,
					UserId = SeedUserId,
					Latitude = 500,
					Longitude = 100,
					LocationDescription = "Miasto: Warszawa\nUlica: 3 maja",
					Circumference = 300,
					IsAlive = true,
					EstimatedAge = 150,
					Description = "Fajne drzewo",
					IsNatureMonument = true,
					Status = ReportStatus.Approved,
					CreatedAt = new DateTime(2025, 6, 15),
					IsVerified = true,
				},
				new TreeReport
				{
					Id = SeedTreeReportTwoId,
					SpeciesId = SeedPineId,
					UserId = SeedUserId,
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
		modelBuilder.Entity<TreeReportAttachment>().HasData(
			new TreeReportAttachment
			{
				Id = SeedAttachmentOneId,
				TreeReportId = SeedTreeReportOneId,
				FileName = "Obrazek pnia dębu",
				FileUrl = "fileUrl",
			},
			new TreeReportAttachment
			{
				Id = SeedAttachmentTwoId,
				TreeReportId = SeedTreeReportOneId,
				FileName = "Obrazek liścia dębu",
				FileUrl = "fileUrl",
			},
			new TreeReportAttachment
			{
				Id = SeedAttachmentThreeId,
				TreeReportId = SeedTreeReportTwoId,
				FileName = "Obrazek pnia sosny",
				FileUrl = "fileUrl",
			}
		);

		// Seed Tags
		modelBuilder.Entity<Tag>().HasData(
				new Tag { Id = SeedDroughtTagId, Name = "Posusz" },
				new Tag { Id = SeedUnstableTagId, Name = "Niestabilny" }
		);

		// Connection between Reports and Tags
		modelBuilder.Entity<TreeConditionTags>().HasData(
			new TreeConditionTags
			{
				TreeReportId = SeedTreeReportOneId,
				TagId = SeedDroughtTagId
			},
			new TreeConditionTags
			{
				TreeReportId = SeedTreeReportOneId,
				TagId = SeedUnstableTagId
			}
		);
	}
};