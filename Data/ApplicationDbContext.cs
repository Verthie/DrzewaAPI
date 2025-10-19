using DrzewaAPI.Extensions;
using DrzewaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DrzewaAPI.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
	public DbSet<RefreshToken> RefreshTokens { get; set; }
	public DbSet<User> Users { get; set; }
	public DbSet<TreeSubmission> TreeSubmissions { get; set; }
	public DbSet<TreeSpecies> TreeSpecies { get; set; }
	public DbSet<TreeVote> TreeVotes { get; set; }
	public DbSet<Application> Applications { get; set; }
	public DbSet<ApplicationTemplate> ApplicationTemplates { get; set; }
	public DbSet<Commune> Communes { get; set; }
	public DbSet<EmailVerificationToken> EmailVerificationTokens { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		// User Configuration
		modelBuilder.Entity<User>(entity =>
		{
			entity.HasKey(e => e.Id);
			entity.Property(e => e.RegistrationDate).HasDefaultValueSql("GETUTCDATE()");
			entity.HasIndex(e => e.Email).IsUnique();

			entity.OwnsOne(e => e.Organization, organization =>
			{
				organization.Property(o => o.Name).HasColumnName("Organization_Name");
				organization.Property(o => o.Address).HasColumnName("Organization_Address");
				organization.Property(o => o.PostalCode).HasColumnName("Organization_PostalCode");
				organization.Property(o => o.City).HasColumnName("Organization_City");
				organization.Property(o => o.Krs).HasColumnName("Organization_Krs");
				organization.Property(o => o.Regon).HasColumnName("Organization_Regon");
				organization.Property(o => o.Mail).HasColumnName("Organization_Mail");
				organization.Property(o => o.Phone).HasColumnName("Organization_Phone");

				organization.OwnsOne(o => o.Correspondence, correspondence =>
				{
					correspondence.Property(c => c.PoBox).HasColumnName("Organization_Correspondence_PoBox");
					correspondence.Property(c => c.Address).HasColumnName("Organization_Correspondence_Address");
					correspondence.Property(c => c.PostalCode).HasColumnName("Organization_Correspondence_PostalCode");
					correspondence.Property(c => c.City).HasColumnName("Organization_Correspondence_City");
				});
			});
		});

		// TreeSubmission Configuration
		modelBuilder.Entity<TreeSubmission>(entity =>
		{
			entity.HasKey(e => e.Id);
			entity.OwnsOne(e => e.Location, location =>
			{
				location.Property(l => l.Lat).IsRequired();
				location.Property(l => l.Lng).IsRequired();
				location.Property(l => l.Address);
				location.Property(l => l.PlotNumber);
				location.Property(l => l.District);
				location.Property(l => l.Province);
				location.Property(l => l.County);
				location.Property(l => l.Commune);
			});
			entity.Property(e => e.SubmissionDate).HasDefaultValueSql("GETUTCDATE()");

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
			entity.OwnsOne(e => e.SeasonalChanges);
			entity.OwnsOne(e => e.Traits);
			entity.OwnsMany(e => e.Images);
		});

		// Vote Configuration - Unique constraint
		modelBuilder.Entity<TreeVote>(entity =>
		{
			entity.HasKey(e => e.Id);
			entity.HasIndex(e => new { e.UserId, e.TreeSubmissionId }).IsUnique();
			entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

			entity.HasOne(e => e.User)
									.WithMany(e => e.TreeVotes)
									.HasForeignKey(e => e.UserId)
									.OnDelete(DeleteBehavior.Restrict);

			entity.HasOne(e => e.TreeSubmission)
									.WithMany(e => e.TreeVotes)
									.HasForeignKey(e => e.TreeSubmissionId)
									.OnDelete(DeleteBehavior.Cascade);
		});

		// Application Configuration
		modelBuilder.Entity<Application>(entity =>
		{
			entity.HasKey(e => e.Id);
			entity.Property(e => e.FormData).HasJsonConversion();
			entity.Property(e => e.Status).HasConversion<string>();
			entity.Property(e => e.CreatedDate).HasDefaultValueSql("GETUTCDATE()");

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
			entity.Property(e => e.Fields).HasJsonConversion();
			entity.Property(e => e.CreatedDate).HasDefaultValueSql("GETUTCDATE()");
			entity.HasIndex(e => new { e.CommuneId, e.Name }).IsUnique();

			entity.OwnsOne(e => e.Signature, signature =>
			{
				signature.Property(s => s.Height);
				signature.Property(s => s.Width);
				signature.Property(s => s.X);
				signature.Property(s => s.Y);
			});

			entity.HasOne(e => e.Commune)
								.WithMany(at => at.ApplicationTemplates)
								.HasForeignKey(e => e.CommuneId)
								.OnDelete(DeleteBehavior.Restrict);

		});

		// Commune configuration
		modelBuilder.Entity<Commune>(entity =>
		{
			entity.HasKey(e => e.Id);
			entity.Property(e => e.CreatedDate).HasDefaultValueSql("GETUTCDATE()");

			entity.HasIndex(e => e.Name).IsUnique();
		});

		modelBuilder.Entity<EmailVerificationToken>(entity =>
		{
			entity.HasKey(e => e.Id);
			entity.HasIndex(e => e.Token).IsUnique();

			entity.HasOne(e => e.User)
						.WithMany(u => u.EmailVerificationTokens)
						.HasForeignKey(e => e.UserId)
						.OnDelete(DeleteBehavior.Cascade);
		});

		// Seed data
		SeedInitialData(modelBuilder);
	}

	private void SeedInitialData(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<User>().HasData(
			new User { Id = Guid.Parse("c6d5f2b5-bc4a-4f3d-9b68-000000000001"), FirstName = "Adam", LastName = "Kowalski", Email = "mod@example.com", RegistrationDate = new DateTime(2024, 1, 15), PasswordHash = "AQAAAAIAAYagAAAAEHrSf4c5BhE6GMi8qlT3Q+oj6mJdQ2OAuPNUgxuc2sFGCxCeqhJwGOEUTqjSuPCFRw==", Role = UserRole.Moderator },
			new User { Id = Guid.Parse("c6d5f2b5-bc4a-4f3d-9b68-000000000002"), FirstName = "Jan", LastName = "Kowalski", Email = "user@example.com", RegistrationDate = new DateTime(2024, 1, 15), PasswordHash = "AQAAAAIAAYagAAAAEDk+b31OOCvyrUQRFQztUECMUI+lPATVktwSn0Uysc66qax8wCdiejpv2Rd1YuophQ==", Role = UserRole.User }
		);

		modelBuilder.Entity<User>().OwnsOne(e => e.Organization).HasData(
			new
			{
				UserId = Guid.Parse("c6d5f2b5-bc4a-4f3d-9b68-000000000002"),
				Name = "Fundacja Będzie Dziko",
				Address = "ul. Wiosenna 21/26",
				PostalCode = "12-202",
				City = "Rzeszów",
				Krs = "0000812345",
				Regon = "386123456",
				Mail = "fundacja.nazwa.@gmail.com",
				Phone = "123213321",
			});

		modelBuilder.Entity<User>().OwnsOne(u => u.Organization).OwnsOne(o => o.Correspondence).HasData(
			new
			{
				OrganizationDtoUserId = Guid.Parse("c6d5f2b5-bc4a-4f3d-9b68-000000000002"),
				PoBox = 123,
				Address = "Mleczna 12",
				PostalCode = "00-001",
				City = "Rzeszów"
			});

		modelBuilder.Entity<TreeSpecies>().HasData(new TreeSpecies
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

		modelBuilder.Entity<TreeSpecies>().OwnsOne(e => e.SeasonalChanges).HasData(
			new
			{
				TreeSpeciesId = Guid.Parse("c6d5f2b5-bc4a-4f3d-9b68-13e2a62f3ed8"),
				Spring = "Młode liście jasno-zielone, często z czerwonawym nalotem. Kwitnienie w maju - kotki męskie i niewielkie kwiaty żeńskie",
				Summer = "Liście ciemno-zielone, gęsta korona dająca dużo cienia. Rozwijają się żołędzie",
				Autumn = "Liście żółto-brązowe, opadają późno w sezonie. Dojrzałe żołędzie opadają i są zbierane przez zwierzęta",
				Winter = "Charakterystyczna sylwetka z grubym pniem i rozłożystymi gałęziami. Kora wyraźnie bruzdowna"
			});

		modelBuilder.Entity<TreeSpecies>().OwnsOne(e => e.Traits).HasData(
			new
			{
				TreeSpeciesId = Guid.Parse("c6d5f2b5-bc4a-4f3d-9b68-13e2a62f3ed8"),
				MaxHeight = 40,
				Lifespan = "Ponad 1000 lat",
				NativeToPoland = true
			});

		modelBuilder.Entity<Commune>().HasData(
			new Commune
			{
				Id = Guid.Parse("c6d5f2b5-bc4a-4f3d-9b68-000000000003"),
				Name = "Gmina Warszawa",
				Address = "Plac Bankowy 3/5",
				City = "Warszawa",
				Province = "Mazowieckie",
				PostalCode = "00-950",
				Phone = "+48 22 443 01 00",
				Email = "sekretariat@um.warszawa.pl",
				Website = "https://www.warszawa.pl",
				CreatedDate = new DateTime(2024, 1, 15)
			},
			new Commune
			{
				Id = Guid.Parse("c6d5f2b5-bc4a-4f3d-9b68-000000000004"),
				Name = "Gmina Kraków",
				Address = "Pl. Wszystkich Świętych 3-4",
				City = "Kraków",
				Province = "Małopolskie",
				PostalCode = "31-004",
				Phone = "+48 12 616 5555",
				Email = "ws.umk@um.krakow.pl",
				Website = "https://www.krakow.pl",
				CreatedDate = new DateTime(2024, 1, 15)
			},
			new Commune
			{
				Id = Guid.Parse("c6d5f2b5-bc4a-4f3d-9b68-000000000009"),
				Name = "Gmina Wrocław",
				Address = "Pl. Nowy Targ 1-8",
				City = "Wrocław",
				Province = "Dolnośląskie",
				PostalCode = "50-141",
				Phone = "+48 71 777 7777",
				Email = "urzad@um.wroc.pl",
				Website = "https://www.wroclaw.pl",
				CreatedDate = new DateTime(2024, 1, 15)
			},
			new Commune
			{
				Id = Guid.Parse("c6d5f2b5-bc4a-4f3d-9b68-000000000010"),
				Name = "Gmina Gdańsk",
				Address = "Ul. Nowe Ogrody 8/12",
				City = "Gdańsk",
				Province = "Pomorskie",
				PostalCode = "80-803",
				Phone = "+48 58 323 7000",
				Email = "prezydent@gedanopedia.pl",
				Website = "https://www.gdansk.pl",
				CreatedDate = new DateTime(2024, 1, 15)
			},
			new Commune
			{
				Id = Guid.Parse("c6d5f2b5-bc4a-4f3d-9b68-000000000011"),
				Name = "Gmina Poznań",
				Address = "Pl. Kolegiacki 17",
				City = "Poznań",
				Province = "Wielkopolskie",
				PostalCode = "61-841",
				Phone = "+48 61 878 5000",
				Email = "um@poznan.pl",
				Website = "https://www.poznan.pl",
				CreatedDate = new DateTime(2024, 1, 15)
			},
			new Commune
			{
				Id = Guid.Parse("c6d5f2b5-bc4a-4f3d-9b68-000000000012"),
				Name = "Gmina Łódź",
				Address = "Ul. Piotrkowska 104",
				City = "Łódź",
				Province = "Łódzkie",
				PostalCode = "90-926",
				Phone = "+48 42 638 4000",
				Email = "um@uml.lodz.pl",
				Website = "https://www.lodz.pl",
				CreatedDate = new DateTime(2024, 1, 15)
			},
			new Commune
			{
				Id = Guid.Parse("c6d5f2b5-bc4a-4f3d-9b68-000000000013"),
				Name = "Gmina Katowice",
				Address = "Ul. Młyńska 4",
				City = "Katowice",
				Province = "Śląskie",
				PostalCode = "40-098",
				Phone = "+48 32 259 3500",
				Email = "umkatowice@katowice.eu",
				Website = "https://www.katowice.eu",
				CreatedDate = new DateTime(2024, 1, 15)
			},
			new Commune
			{
				Id = Guid.Parse("c6d5f2b5-bc4a-4f3d-9b68-000000000014"),
				Name = "Gmina Rzeszów",
				Address = "Ul. Lubelska 9",
				City = "Rzeszów",
				Province = "Podkarpackie",
				PostalCode = "35-021",
				Phone = "+48 17 875 4000",
				Email = "um@erzeszow.pl",
				Website = "https://www.rzeszow.pl",
				CreatedDate = new DateTime(2024, 1, 15)
			}
		);

		modelBuilder.Entity<ApplicationTemplate>().HasData(
			new ApplicationTemplate
			{
				Id = Guid.Parse("c6d5f2b5-bc4a-4f3d-9b68-000000000005"),
				CommuneId = Guid.Parse("c6d5f2b5-bc4a-4f3d-9b68-000000000003"),
				CreatedDate = new DateTime(2024, 1, 15),
				Name = "Standardowy szablon wniosku o rejestrację drzewa jako pomnika przyrody",
				Description = "Wniosek Opisowy",
				HtmlTemplate = "<!DOCTYPE html><html><meta charset=UTF-8><title>Wniosek o ustanowienie ochroną pomnikową drzew</title><style>body{font-family:'Times New Roman',Times,serif;}.container-flex{display:flex;flex-direction:row;justify-content:space-between}.sender{text-align:left}.sender p{padding:0}.date{text-align:right}.recipient{text-align:center;margin-bottom:15px;margin-top:25px}.recipient p{margin:2px 0;font-weight:700}.title{font-weight:700;text-align:center;margin-bottom:15px}.content{text-align:justify;text-justify:inter-word}.paragraph{text-indent:30px;line-height:1.65}.paragraph.first{margin-bottom:10px;text-indent:30px;font-style:italic;text-align:center}</style><div><div class=container-flex><div class=sender>{{sender}}</div><div class=date>{{user_city}}, {{generation_date}}r.</div></div><div class=recipient><p>{{commune_name}}<p>{{commune_address}}<p>{{commune_postal_code}} {{commune_city}}</div><div class=title>Wniosek o ustanowienie ochroną pomnikową drzew</div><div class=content><p class=\"paragraph first\"><span>Na podstawie art. 221 i art. 241 KPA, składam wniosek o opisanego niżej drzewa za pomnik przyrody w trybie art. 40-45 ustawy o ochronie przyrody.</span><p class=paragraph>{{justification}}<p class=paragraph>Załącznik<p class=paragraph>1. {{tree_images}}<p class=paragraph>2. {{tree_map_image}}</div></div>",
				Fields = new List<ApplicationField>
						{
								new ApplicationField
								{
										Name = "justification",
										Label = "Uzasadnienie wniosku",
										Type = ApplicationFieldType.TextArea,
										IsRequired = true,
										Placeholder = "Proszę opisać dlaczego drzewo powinno zostać objęte ochroną...",
										Validation = new ApplicationFieldValidation
										{
												MinLength = 50,
												MaxLength = 1500,
												ValidationMessage = "Uzasadnienie musi mieć od 50 do 1500 znaków"
										},
										HelpText = "Opisz walory przyrodnicze, historyczne lub krajobrazowe drzewa",
										Order = 1
								}
						}
			},
			new ApplicationTemplate
			{
				Id = Guid.Parse("c6d5f2b5-bc4a-4f3d-9b68-000000000006"),
				CommuneId = Guid.Parse("c6d5f2b5-bc4a-4f3d-9b68-000000000004"),
				CreatedDate = new DateTime(2024, 1, 15),
				Name = "Szablon udostępniony przez gminę Kraków",
				Description = "Uznanie obiektu przyrodniczego za pomnik przyrody WS-13",
				GeminiResponseMinLength = 400,
				GeminiResponseMaxLength = 650,
				HtmlTemplate = "<!DOCTYPE html><html><meta charset=UTF-8><style>body{font-family:Arial,sans-serif;font-size:12px}.header{text-align:left;margin:0 5px 20px 5px;font-size:12px}.title{margin:42px 5px 42px 5px}.title h1{text-align:center;font-size:22px;font-weight:300;margin:0}.title h2{text-align:center;font-size:15px;margin:0 0 12px 0}.title p{font-size:14px}table{border-collapse:collapse;margin:auto}td{border:1px solid #000;padding:4px 8px 8px 8px;vertical-align:top}.number-col{width:30px;text-align:center}.question-col{width:42%;}.answer-col{width:55%;text-wrap:balance;word-break:break-word;overflow-wrap:break-word}.footer{display:flex;justify-content:space-between;align-items:flex-start;margin:100px 5px 0 5px}.footer *{margin:0}.signature{display:flex;flex-direction:column;text-align:center;align-items:center;justify-content:center;position:relative;}.signature-text{font-size:10px}.signature-field{position:absolute;bottom:17px;}</style><div class=header>Załącznik do procedury WS-13</div><div class=title><h1>Wniosek</h1><h2>o uznanie obiektu przyrodniczego za pomnik przyrody</h2><p>na podstawie Art. 6 ust. 1 pkt 6, art. 40, art. 44 ustawy z dnia 16 kwietnia 2004 r. o ochronie przyrody.</div><table><tr><td class=number-col>1.<td class=question-col>Imię i nazwisko wnioskodawcy / nazwa wnioskodawcy<br>Adres / siedziba wnioskodawcy<td class=answer-col>{{user_full_name}}<br>{{user_address}}<br>{{user_city}}, {{user_postal_code}}<tr><td class=number-col>2.<td class=question-col>Nazwa i rodzaj pomnika przyrody<td class=answer-col>Nazwa polska: {{tree_species_polish}}<br>Nazwa łacińska: {{tree_species_latin}}<br>Rodzaj: drzewo<tr><td class=number-col>3.<td class=question-col>Określenie położenia geograficznego i administracyjnego pomnika przyrody (działka, obręb ewidencyjny, jednostka ewidencyjna)<td class=answer-col>Położenie geograficzne: {{location_lat}} lat, {{location_long}} long<br>Działka: {{location_plot}}<br>Obręb ewidencyjny: {{location_district}}<br>Jednostka ewidencyjna: {{record_keeping_unit}}<tr><td class=number-col>4.<td class=question-col>Wskazanie formy własności i rodzajów gruntów<td class=answer-col>Forma własności: {{ownership_form}}<br>Rodzaj gruntów: {{land_type}}<tr><td class=number-col>5.<td class=question-col>Krótki opis pomnika przyrody<br>- dla pomników przyrody żywej gatunek, wiek, pierśnica, wysokość, rozpiętość korony, stan zdrowotny,<br>- dla pomników przyrody nieżywej typ, rodzaj, wielkość źródła, wodospadu, głazu, jaskini itp.<td class=answer-col>Wiek: {{tree_estimated_age}}<br>Pierśnica: {{tree_circumference}} cm<br>Wysokość: {{tree_height}} m<br>Rozpiętość: {{tree_spread}} m<br>Stan gleby: {{tree_soil}}<br>Stan zdrowia: {{tree_health}}<br>Stan środowiska: {{tree_environment}}<tr><td class=number-col>6.<td class=question-col>Nazwa, autor opracowania potwierdzającego wartości przyrodnicze obiektu<td class=answer-col>Nazwa opracowania: {{study_name}}<br>Autor: {{study_author}}<tr><td class=number-col>7.<td class=question-col>Uzasadnienie<td class=answer-col>{{justification}}<tr><td class=number-col>8.<td class=question-col>Załączniki<td class=answer-col>{{tree_images}}<br>{{tree_map_image}}</table><div class=footer><p>{{user_city}}, dn. {{generation_date}}<div class=signature><p>..............................................<p class=signature-text><em>(podpis)</em></div></div></body></html>",
				Fields = new List<ApplicationField>
				{
								new ApplicationField
								{
										Name = "justification",
										Label = "Uzasadnienie wniosku",
										Type = ApplicationFieldType.TextArea,
										IsRequired = true,
										Placeholder = "Proszę opisać dlaczego drzewo powinno zostać objęte ochroną...",
										Validation = new ApplicationFieldValidation
										{
												MinLength = 50,
												MaxLength = 1500,
												ValidationMessage = "Uzasadnienie musi mieć od 50 do 1500 znaków"
										},
										HelpText = "Opisz walory przyrodnicze, historyczne lub krajobrazowe drzewa",
										Order = 1
								},
								new ApplicationField
								{
										Name = "location_plot",
										Label = "Działka",
										Type = ApplicationFieldType.TextArea,
										IsRequired = true,
										Placeholder = "Proszę podać działkę na której znajduje się pomnik przyrody",
										Validation = new ApplicationFieldValidation
										{
												MinLength = 1,
												MaxLength = 100,
												ValidationMessage = "Tekst musi mieć od 1 do 100 znaków"
										},
										Order = 2
								},
								new ApplicationField
								{
										Name = "location_district",
										Label = "Obręb ewidencyjny",
										Type = ApplicationFieldType.TextArea,
										IsRequired = true,
										Placeholder = "Proszę podać obręb ewidencyjny",
										Validation = new ApplicationFieldValidation
										{
												MinLength = 1,
												MaxLength = 100,
												ValidationMessage = "Tekst musi mieć od 1 do 100 znaków"
										},
										Order = 3
								},
								new ApplicationField
								{
										Name = "record_keeping_unit",
										Label = "Jednostka ewidencyjna",
										Type = ApplicationFieldType.TextArea,
										IsRequired = true,
										Placeholder = "Proszę podać jednostkę ewidencyjną",
										Validation = new ApplicationFieldValidation
										{
												MinLength = 1,
												MaxLength = 100,
												ValidationMessage = "Tekst musi mieć od 1 do 100 znaków"
										},
										Order = 4
								},
								new ApplicationField
								{
										Name = "ownership_form",
										Label = "Forma własności",
										Type = ApplicationFieldType.TextArea,
										IsRequired = true,
										Placeholder = "Proszę podać formę własności",
										Validation = new ApplicationFieldValidation
										{
												MinLength = 1,
												MaxLength = 100,
												ValidationMessage = "Tekst musi mieć od 1 do 100 znaków"
										},
										Order = 5
								},
								new ApplicationField
								{
										Name = "land_type",
										Label = "Rodzaj gruntów",
										Type = ApplicationFieldType.TextArea,
										IsRequired = true,
										Placeholder = "Proszę podać rodzaj gruntów",
										Validation = new ApplicationFieldValidation
										{
												MinLength = 1,
												MaxLength = 100,
												ValidationMessage = "Tekst musi mieć od 1 do 100 znaków"
										},
										Order = 6
								},
								new ApplicationField
								{
										Name = "study_name",
										Label = "Nazwa opracowania",
										Type = ApplicationFieldType.TextArea,
										IsRequired = true,
										Placeholder = "Proszę podać nazwę opracowania",
										Validation = new ApplicationFieldValidation
										{
												MinLength = 1,
												MaxLength = 100,
												ValidationMessage = "Tekst musi mieć od 1 do 100 znaków"
										},
										Order = 7
								},
								new ApplicationField
								{
										Name = "study_author",
										Label = "Autor",
										Type = ApplicationFieldType.TextArea,
										IsRequired = true,
										Placeholder = "Proszę podać imię i nazwisko autora opracowania",
										Validation = new ApplicationFieldValidation
										{
												MinLength = 1,
												MaxLength = 100,
												ValidationMessage = "Tekst musi mieć od 1 do 100 znaków"
										},
										Order = 8
								},
						}
			}
		);

		modelBuilder.Entity<ApplicationTemplate>().OwnsOne(e => e.Signature).HasData(
			new
			{
				ApplicationTemplateId = Guid.Parse("c6d5f2b5-bc4a-4f3d-9b68-000000000005"),
				Height = 80.0F,
				Width = 120.0F,
				X = 410.0F,
				Y = 100.0F,
			},
			new
			{
				ApplicationTemplateId = Guid.Parse("c6d5f2b5-bc4a-4f3d-9b68-000000000006"),
				Height = 80.0F,
				Width = 120.0F,
				X = 410.0F,
				Y = 100.0F,
			}
		);

		modelBuilder.Entity<TreeSubmission>().HasData(
		new
		{
			Id = Guid.Parse("c6d5f2b5-bc4a-4f3d-9b68-000000000007"),
			UserId = Guid.Parse("c6d5f2b5-bc4a-4f3d-9b68-000000000002"),
			SpeciesId = Guid.Parse("c6d5f2b5-bc4a-4f3d-9b68-13e2a62f3ed8"),
			Circumference = 100.0,
			Height = 20.0,
			Condition = "Dobra",
			IsAlive = true,
			EstimatedAge = 100,
			CrownSpread = 150.0,
			Description = "Pomnik przyrody",
			Status = SubmissionStatus.Approved,
			IsMonument = true,
			SubmissionDate = new DateTime(2024, 1, 15),
			ApprovalDate = new DateTime(2024, 1, 16)
		},
		new
		{
			Id = Guid.Parse("c6d5f2b5-bc4a-4f3d-9b68-000000000008"),
			UserId = Guid.Parse("c6d5f2b5-bc4a-4f3d-9b68-000000000002"),
			SpeciesId = Guid.Parse("c6d5f2b5-bc4a-4f3d-9b68-13e2a62f3ed8"),
			Circumference = 115.0,
			Height = 27.0,
			Condition = "Zła",
			IsAlive = true,
			EstimatedAge = 350,
			CrownSpread = 150.0,
			Description = "Dąb Pomnik przyrody",
			IsMonument = true,
			Status = SubmissionStatus.Pending,
			SubmissionDate = new DateTime(2024, 1, 17)
		});

		modelBuilder.Entity<TreeSubmission>().OwnsOne(e => e.Location).HasData(
		new
		{
			TreeSubmissionId = Guid.Parse("c6d5f2b5-bc4a-4f3d-9b68-000000000007"),
			Lat = 52.5269614,
			Lng = 17.1284842,
			Address = "Nieznany",
			PlotNumber = "112/2",
			District = "string",
			Province = "string",
			County = "string",
			Commune = "string",
		},
		new
		{
			TreeSubmissionId = Guid.Parse("c6d5f2b5-bc4a-4f3d-9b68-000000000008"),
			Lat = 51.5369179,
			Lng = 17.8664067,
			Address = "Nieznany",
			PlotNumber = "11/1",
			District = "string",
			Province = "string",
			County = "string",
			Commune = "string",
		});

		modelBuilder.Entity<Application>().HasData(
			new Application
			{
				Id = Guid.Parse("c6d5f2b5-bc4a-4f3d-9b68-000000000015"),
				UserId = Guid.Parse("c6d5f2b5-bc4a-4f3d-9b68-000000000002"),
				TreeSubmissionId = Guid.Parse("c6d5f2b5-bc4a-4f3d-9b68-000000000007"),
				ApplicationTemplateId = Guid.Parse("c6d5f2b5-bc4a-4f3d-9b68-000000000006"),
				FormData = new Dictionary<string, object>(),
				Status = ApplicationStatus.Draft,
				CreatedDate = new DateTime(2024, 1, 15)
			},
			new Application
			{
				Id = Guid.Parse("c6d5f2b5-bc4a-4f3d-9b68-000000000016"),
				UserId = Guid.Parse("c6d5f2b5-bc4a-4f3d-9b68-000000000002"),
				TreeSubmissionId = Guid.Parse("c6d5f2b5-bc4a-4f3d-9b68-000000000008"),
				ApplicationTemplateId = Guid.Parse("c6d5f2b5-bc4a-4f3d-9b68-000000000005"),
				FormData = new Dictionary<string, object>(),
				Status = ApplicationStatus.Draft,
				CreatedDate = new DateTime(2024, 1, 15)
			}
			);
	}
}
