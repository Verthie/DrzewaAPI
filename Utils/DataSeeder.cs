using System;
using DrzewaAPI.Data;
using DrzewaAPI.Models;
using DrzewaAPI.Models.Enums;
using DrzewaAPI.Models.ValueObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DrzewaAPI.Utils;

public class DataSeeder(ApplicationDbContext _db, IPasswordHasher<User> _hasher, ILogger<DataSeeder> _logger) : IDataSeeder
{
	public async Task SeedAsync(CancellationToken ct = default)
	{
		await _db.Database.MigrateAsync(ct);

		List<User> users = GetMockUsers();
		if (!await _db.Users.AnyAsync(ct)) _db.Users.AddRange(users);
		else users = await _db.Users.ToListAsync();   // idempotent guard

		TreeSpecies[] species = GetMockSpecies();
		if (!await _db.TreeSpecies.AnyAsync(ct)) _db.TreeSpecies.AddRange(species);
		else species = await _db.TreeSpecies.ToArrayAsync();

		TreeSpeciesImage[] images = GetMockImages(species);
		if (!await _db.TreeSpeciesImages.AnyAsync(ct)) _db.TreeSpeciesImages.AddRange(images);
		// else images = await _db.TreeSpeciesImages.ToArrayAsync();

		TreeSubmission[] submissions = GetMockSubmissions(users, species);
		if (!await _db.TreeSubmissions.AnyAsync(ct)) _db.TreeSubmissions.AddRange(submissions);
		else submissions = await _db.TreeSubmissions.ToArrayAsync();

		Vote[] votes = GetMockVotes(submissions, users);
		if (!await _db.Votes.AnyAsync(ct)) _db.Votes.AddRange(votes);
		// else votes = await _db.Votes.ToArrayAsync();


		if (!_db.ChangeTracker.Entries().Any()) return;

		await _db.SaveChangesAsync(ct);

		_logger.LogInformation("Seeding finished");
	}

	private List<User> GetMockUsers()
	{
		List<User> users = [];

		User user1 = new User
		{
			Id = Guid.NewGuid(),
			FirstName = "Adam",
			LastName = "Kowalski",
			Email = "adam.wolkin@email.com",
			Avatar = "https://images.pexels.com/photos/220453/pexels-photo-220453.jpeg?w=100&h=100&fit=crop",
			RegistrationDate = new DateTime(2024, 1, 15),
		};
		user1.PasswordHash = _hasher.HashPassword(user1, "Passw0rd!");

		User user2 = new User
		{
			Id = Guid.NewGuid(),
			FirstName = "Maria",
			LastName = "Nowak",
			Email = "maria.kowalska@email.com",
			Avatar = "https://images.pexels.com/photos/415829/pexels-photo-415829.jpeg?w=100&h=100&fit=crop",
			RegistrationDate = new DateTime(2024, 2, 20),
		};
		user2.PasswordHash = _hasher.HashPassword(user2, "VerySafe");

		User user3 = new User
		{
			Id = Guid.NewGuid(),
			FirstName = "Piotr",
			LastName = "Wiśniewski",
			Email = "piotr.nowak@email.com",
			Avatar = "https://images.pexels.com/photos/614810/pexels-photo-614810.jpeg?w=100&h=100&fit=crop",
			RegistrationDate = new DateTime(2024, 3, 10),
		};
		user3.PasswordHash = _hasher.HashPassword(user3, "HelloWorld");

		User user4 = new User
		{
			Id = Guid.NewGuid(),
			FirstName = "Anna",
			LastName = "Zielińska",
			Email = "anna.wisniowska@email.com",
			Avatar = "https://images.pexels.com/photos/733872/pexels-photo-733872.jpeg?w=100&h=100&fit=crop",
			RegistrationDate = new DateTime(2024, 1, 5),
		};
		user4.PasswordHash = _hasher.HashPassword(user4, "123456789");

		users.Add(user1);
		users.Add(user2);
		users.Add(user3);
		users.Add(user4);

		return users;
	}

	private TreeSpeciesImage[] GetMockImages(TreeSpecies[] species)
	{
		return [
			new TreeSpeciesImage {
				Id = Guid.NewGuid(),
				TreeSpeciesId = species[0].Id,
				ImageUrl = "https://images.pexels.com/photos/1172675/pexels-photo-1172675.jpeg?w=800&h=600&fit=crop",
				Type = ImageType.Tree
			},
			new TreeSpeciesImage {
				Id = Guid.NewGuid(),
				TreeSpeciesId = species[0].Id,
				ImageUrl = "https://images.pexels.com/photos/1379636/pexels-photo-1379636.jpeg?w=800&h=600&fit=crop",
				Type = ImageType.Leaf
			},
			new TreeSpeciesImage {
				Id = Guid.NewGuid(),
				TreeSpeciesId = species[0].Id,
				ImageUrl = "https://images.pexels.com/photos/1448055/pexels-photo-1448055.jpeg?w=800&h=600&fit=crop",
				Type = ImageType.Bark
			},
			new TreeSpeciesImage {
				Id = Guid.NewGuid(),
				TreeSpeciesId = species[0].Id,
				ImageUrl = "https://images.pexels.com/photos/33109/fall-autumn-red-season.jpg?w=800&h=600&fit=crop",
				Type = ImageType.Fruit
			},
		];
	}

	private TreeSpecies[] GetMockSpecies()
	{
		return [
			new TreeSpecies() {
				Id = Guid.NewGuid(),
				PolishName = "Dąb szypułkowy",
				LatinName = "Quercus Robur",
				Family = "Fagaceae",
				Description = "Dąb szypułkowy to jeden z najważniejszych gatunków drzew w Polsce. Może żyć ponad 1000 lat i osiągać wysokość do 40 metrów. Jest symbolem siły, trwałości i mądrości w kulturze słowiańskiej. Drewno dębu było używane do budowy statków, domów i mebli przez wieki.",
				IdentificationGuide = ["Liście z wyraźnymi wcięciami, bez szypułek lub z bardzo krótkimi szypułkami",
				"Żołędzie na długich szypułkach (2-8 cm), dojrzewają jesienią",
				"Kora szara, głęboko bruzdowna u starych okazów, gładka u młodych",
				"Korona szeroka, rozłożysta, charakterystyczny pokrój \"parasola\"",
				"Pąki skupione na końcach pędów, jajowate, brązowe"],
				SeasonalChanges = new SeasonalChanges {
					Spring = "Młode liście jasno-zielone, często z czerwonawym nalotem. Kwitnienie w maju - kotki męskie i niewielkie kwiaty żeńskie",
					Summer = "Liście ciemno-zielone, gęsta korona dająca dużo cienia. Rozwijają się żołędzie",
					Autumn = "Liście żółto-brązowe, opadają późno w sezonie. Dojrzałe żołędzie opadają i są zbierane przez zwierzęta",
					Winter = "Charakterystyczna sylwetka z grubym pniem i rozłożystymi gałęziami. Kora wyraźnie bruzdowna"
				},
				Traits = new Traits {
					MaxHeight = 40,
					Lifespan = "Ponad 1000 lat",
					NativeToPoland = true
				}
			}
		];
	}

	private TreeSubmission[] GetMockSubmissions(List<User> users, TreeSpecies[] species)
	{
		return [
			new TreeSubmission(){
				Id = Guid.NewGuid(),
				UserId = users[0].Id,
				SpeciesId = species[0].Id,
				Location = new Location {
					Lat = 52.2297,
					Lng = 21.0122,
					Address = "Warszawa, Park Łazienkowski, przy Pałacu na Wyspie"
				},
				Circumference = 520,
				Height = 28,
				Condition = "excellent",
				IsMonument = true,
				Description = "Wspaniały okaz dębu szypułkowego w Parku Łazienkowskim. Ten majestatyczny dąb rośnie tuż przy Pałacu na Wyspie i jest jednym z najstarszych drzew w parku. Szacowany wiek około 300 lat. Drzewo ma charakterystyczną, rozłożystą koronę i imponujący pień. W cieniu tego dęba odpoczywali już królowie polscy.",
				Images = ["https://images.pexels.com/photos/1172675/pexels-photo-1172675.jpeg?w=800&h=600&fit=crop", "https://images.pexels.com/photos/1179229/pexels-photo-1179229.jpeg?w=800&h=600&fit=crop", "https://images.pexels.com/photos/1448055/pexels-photo-1448055.jpeg?w=800&h=600&fit=crop",],
				Status = SubmissionStatus.Monument,
				SubmissionDate = new DateTime(2024, 1, 15),
				ApprovalDate = new DateTime(2024, 1, 20),
			},
		];
	}

	private Vote[] GetMockVotes(TreeSubmission[] submissions, List<User> users)
	{
		return [
			new Vote {
				Id = Guid.NewGuid(),
				TreeSubmissionId = submissions[0].Id,
				UserId = users[0].Id,
				Type = VoteType.Approve,
				CreatedAt = new DateTime(2024, 1, 17)
			},
			new Vote {
				Id = Guid.NewGuid(),
				TreeSubmissionId = submissions[0].Id,
				UserId = users[1].Id,
				Type = VoteType.Approve,
				CreatedAt = new DateTime(2024, 1, 19)
			},
			new Vote {
				Id = Guid.NewGuid(),
				TreeSubmissionId = submissions[0].Id,
				UserId = users[2].Id,
				Type = VoteType.Reject,
				CreatedAt = new DateTime(2024, 2, 21)
			}
		];
	}
}