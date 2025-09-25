using System.Text.Json;
using DrzewaAPI.Extensions;
using DrzewaAPI.Models;
using DrzewaAPI.Models.Enums;
using DrzewaAPI.Utils;
using Microsoft.EntityFrameworkCore;

namespace DrzewaAPI.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
	public DbSet<RefreshToken> RefreshTokens { get; set; }
	public DbSet<User> Users { get; set; }
	public DbSet<TreeSubmission> TreeSubmissions { get; set; }
	public DbSet<TreeSpecies> TreeSpecies { get; set; }
	public DbSet<TreeVote> TreeVotes { get; set; }
	public DbSet<CommentVote> CommentVotes { get; set; }
	public DbSet<Comment> Comments { get; set; }
	public DbSet<Application> Applications { get; set; }
	public DbSet<ApplicationTemplate> ApplicationTemplates { get; set; }
	public DbSet<Municipality> Municipalities { get; set; }
	public DbSet<EmailVerificationToken> EmailVerificationTokens { get; set; }

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

			entity.HasData(new TreeSpecies
			{
				Id = Guid.Parse("c6d5f2b5-bc4a-4f3d-9b68-13e2a62f3ed8"),
				PolishName = "Dąb szypułkowy",
				LatinName = "Quercus Robur",
				Family = "Fagaceae",
				Description = "Dąb szypułkowy to jeden z najważniejszych gatunków drzew w Polsce. Może żyć ponad 1000 lat i osiągać wysokość do 40 metrów. Jest symbolem siły, trwałości i mądrości w kulturze słowiańskiej. Drewno dębu było używane do budowy statków, domów i mebli przez wieki.",
				IdentificationGuide = ["Liście z wyraźnymi wcięciami, bez szypułek lub z bardzo krótkimi szypułkami",
					"Żołędzie na długich szypułkach (2-8 cm), dojrzewają jesienią",
					"Kora szara, głęboko bruzdowna u starych okazów, gładka u młodych",
					"Korona szeroka, rozłożysta, charakterystyczny pokrój \"parasola\"",
					"Pąki skupione na końcach pędów, jajowate, brązowe"],
			});

			entity.OwnsOne(e => e.SeasonalChanges).HasData(
				new
				{
					TreeSpeciesId = Guid.Parse("c6d5f2b5-bc4a-4f3d-9b68-13e2a62f3ed8"),
					Spring = "Młode liście jasno-zielone, często z czerwonawym nalotem. Kwitnienie w maju - kotki męskie i niewielkie kwiaty żeńskie",
					Summer = "Liście ciemno-zielone, gęsta korona dająca dużo cienia. Rozwijają się żołędzie",
					Autumn = "Liście żółto-brązowe, opadają późno w sezonie. Dojrzałe żołędzie opadają i są zbierane przez zwierzęta",
					Winter = "Charakterystyczna sylwetka z grubym pniem i rozłożystymi gałęziami. Kora wyraźnie bruzdowna"
				});

			entity.OwnsOne(e => e.Traits).HasData(
				new
				{
					TreeSpeciesId = Guid.Parse("c6d5f2b5-bc4a-4f3d-9b68-13e2a62f3ed8"),
					MaxHeight = 40,
					Lifespan = "Ponad 1000 lat",
					NativeToPoland = true
				});
		});

		// Vote Configuration - Unique constraint
		modelBuilder.Entity<TreeVote>(entity =>
		{
			entity.HasKey(e => e.Id);
			entity.HasIndex(e => new { e.UserId, e.TreeSubmissionId }).IsUnique();

			entity.HasOne(e => e.User)
									.WithMany(e => e.TreeVotes)
									.HasForeignKey(e => e.UserId)
									.OnDelete(DeleteBehavior.Restrict);

			entity.HasOne(e => e.TreeSubmission)
									.WithMany(e => e.TreeVotes)
									.HasForeignKey(e => e.TreeSubmissionId)
									.OnDelete(DeleteBehavior.Cascade);
		});

		modelBuilder.Entity<CommentVote>(entity =>
		{
			entity.HasKey(e => e.Id);
			entity.HasIndex(e => new { e.UserId, e.CommentId }).IsUnique();

			entity.HasOne(e => e.User)
									.WithMany(e => e.CommentVotes)
									.HasForeignKey(e => e.UserId)
									.OnDelete(DeleteBehavior.Restrict);

			entity.HasOne(e => e.Comment)
									.WithMany(e => e.CommentVotes)
									.HasForeignKey(e => e.CommentId)
									.OnDelete(DeleteBehavior.Cascade);
		});

		// Comment Configuration
		modelBuilder.Entity<Comment>(entity =>
		{
			entity.HasKey(e => e.Id);
			entity.Property(e => e.Content).IsRequired().HasMaxLength(3000);

			entity.HasOne(e => e.User)
									.WithMany(e => e.Comments)
									.HasForeignKey(e => e.UserId)
									.OnDelete(DeleteBehavior.Restrict);

			entity.HasOne(e => e.TreeSubmission)
									.WithMany(e => e.Comments)
									.HasForeignKey(e => e.TreeSubmissionId)
									.OnDelete(DeleteBehavior.Cascade);
		});

		// Application Configuration
		modelBuilder.Entity<Application>(entity =>
		{
			entity.HasKey(e => e.Id);
			entity.Property(e => e.FormData).HasJsonConversion();
			entity.Property(e => e.Status).HasConversion<string>();

			entity.HasOne(e => e.User)
								.WithMany(u => u.Applications)
								.HasForeignKey(e => e.UserId)
								.OnDelete(DeleteBehavior.Restrict);

			entity.HasOne(e => e.TreeSubmission)
								.WithMany(ts => ts.Applications)
								.HasForeignKey(e => e.TreeSubmissionId)
								.OnDelete(DeleteBehavior.Cascade);

			entity.HasOne(e => e.ApplicationTemplate)
								.WithMany(at => at.Applications)
								.HasForeignKey(e => e.ApplicationTemplateId)
								.OnDelete(DeleteBehavior.Restrict);
		});

		// ApplicationTemplate Configuration
		modelBuilder.Entity<ApplicationTemplate>(entity =>
		{
			entity.HasKey(e => e.Id);
			entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
			entity.Property(e => e.Description).HasMaxLength(500);
			entity.Property(e => e.HtmlTemplate).IsRequired();
			entity.Property(e => e.Fields).HasJsonConversion();
			entity.HasIndex(e => new { e.MunicipalityId, e.Name }).IsUnique();

			entity.HasOne(e => e.Municipality)
								.WithMany(at => at.ApplicationTemplates)
								.HasForeignKey(e => e.MunicipalityId)
								.OnDelete(DeleteBehavior.Restrict);

		});

		// Municipality configuration
		modelBuilder.Entity<Municipality>(entity =>
		{
			entity.HasKey(e => e.Id);
			entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
			entity.Property(e => e.Address).IsRequired().HasMaxLength(200);
			entity.Property(e => e.City).IsRequired().HasMaxLength(100);
			entity.Property(e => e.Province).IsRequired().HasMaxLength(100);
			entity.Property(e => e.PostalCode).IsRequired().HasMaxLength(10);
			entity.Property(e => e.Phone).IsRequired().HasMaxLength(20);
			entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
			entity.Property(e => e.Website).HasMaxLength(100);

			entity.HasIndex(e => e.Name).IsUnique();
		});

		modelBuilder.Entity<EmailVerificationToken>(entity =>
		{
			entity.HasKey(e => e.Id);
			entity.Property(e => e.Token).IsRequired().HasMaxLength(100);
			entity.HasIndex(e => e.Token).IsUnique();

			entity.HasOne(e => e.User)
						.WithMany(u => u.EmailVerificationTokens)
						.HasForeignKey(e => e.UserId)
						.OnDelete(DeleteBehavior.Cascade);
		});

		// Seed data
		// modelBuilder.Entity<User>().HasData(
		// new User { Id = Guid.Parse("c6d5f2b5-bc4a-4f3d-9b68-13e2a62f3ed5"), FirstName = "Adam", LastName = "Kowalski", Email = "mod@example.com", RegistrationDate = new DateTime(2024, 1, 15), PasswordHash = _hasher.HashPassword(user1, "Passw0rd!") },
		// );
	}
};
