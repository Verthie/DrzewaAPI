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
		// await _db.Database.MigrateAsync(ct);
		// return; // uncomment to stop seeder

		List<User> users = GetMockUsers();
		if (!await _db.Users.AnyAsync(ct)) _db.Users.AddRange(users);
		else users = await _db.Users.ToListAsync(ct);

		TreeSpecies[] species = GetMockSpecies();
		if (!await _db.TreeSpecies.AnyAsync(ct)) _db.TreeSpecies.AddRange(species);
		else species = await _db.TreeSpecies.ToArrayAsync(ct);

		TreeSpeciesImage[] images = GetMockImages(species);
		if (!await _db.TreeSpeciesImages.AnyAsync(ct)) _db.TreeSpeciesImages.AddRange(images);
		// else images = await _db.TreeSpeciesImages.ToArrayAsync(ct);

		TreeSubmission[] submissions = GetMockSubmissions(users, species);
		if (!await _db.TreeSubmissions.AnyAsync(ct)) _db.TreeSubmissions.AddRange(submissions);
		else submissions = await _db.TreeSubmissions.ToArrayAsync(ct);

		Vote[] votes = GetMockVotes(submissions, users);
		if (!await _db.Votes.AnyAsync(ct)) _db.Votes.AddRange(votes);
		// else votes = await _db.Votes.ToArrayAsync(ct);


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

		User user5 = new User
		{
			Id = Guid.NewGuid(),
			FirstName = "Test",
			LastName = "User",
			Email = "user@example.com",
			Avatar = "https://images.pexels.com/users/avatars/268385455/photo-dog-681.png?fit=crop&h=100&w=100",
			RegistrationDate = DateTime.UtcNow,
		};
		user5.PasswordHash = _hasher.HashPassword(user5, "string");

		users.Add(user1);
		users.Add(user2);
		users.Add(user3);
		users.Add(user4);
		users.Add(user5);

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
			new TreeSpeciesImage {
				Id = Guid.NewGuid(),
				TreeSpeciesId = species[1].Id,
				ImageUrl = "https://images.pexels.com/photos/1658967/pexels-photo-1658967.jpeg?w=800&h=600&fit=crop",
				Type = ImageType.Tree
			},
			new TreeSpeciesImage {
				Id = Guid.NewGuid(),
				TreeSpeciesId = species[1].Id,
				ImageUrl = "https://images.pexels.com/photos/1375849/pexels-photo-1375849.jpeg?w=800&h=600&fit=crop",
				Type = ImageType.Leaf
			},
			new TreeSpeciesImage {
				Id = Guid.NewGuid(),
				TreeSpeciesId = species[1].Id,
				ImageUrl = "https://images.pexels.com/photos/1448055/pexels-photo-1448055.jpeg?w=800&h=600&fit=crop",
				Type = ImageType.Bark
			},
			new TreeSpeciesImage {
				Id = Guid.NewGuid(),
				TreeSpeciesId = species[2].Id,
				ImageUrl = "https://images.pexels.com/photos/1632790/pexels-photo-1632790.jpeg?w=800&h=600&fit=crop",
				Type = ImageType.Tree
			},
			new TreeSpeciesImage {
				Id = Guid.NewGuid(),
				TreeSpeciesId = species[2].Id,
				ImageUrl = "https://images.pexels.com/photos/1379636/pexels-photo-1379636.jpeg?w=800&h=600&fit=crop",
				Type = ImageType.Leaf
			},
			new TreeSpeciesImage {
				Id = Guid.NewGuid(),
				TreeSpeciesId = species[2].Id,
				ImageUrl = "https://images.pexels.com/photos/1172341/pexels-photo-1172341.jpeg?w=800&h=600&fit=crop",
				Type = ImageType.Bark
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
			},
			new TreeSpecies() {
				Id = Guid.NewGuid(),
				PolishName = "Lipa drobnolistna",
				LatinName = "Tilia cordata",
				Family = "Malvaceae",
				Description = "Lipa drobnolistna to drzewo o wielkiej wartości kulturowej i przyrodniczej. Od wieków sadzona w centrach miast i przy dworkach. W tradycji słowiańskiej lipa była drzewem świętym, symbolem miłości i sprawiedliwości. Pod lipami odbywały się sądy i zgromadzenia wiejskie.",
				IdentificationGuide = ["Małe, sercowate liście z ząbkowanymi brzegami (3-6 cm długości)",
					"Kwiaty żółtawe, bardzo pachnące, zebrane w baldachogrona (czerwiec-lipiec)",
					"Owoce kuliste z charakterystyczną przysadką - skrzydełkiem",
					"Gładka kora, u starszych drzew lekko spękana w płytkie bruzdy",
					"Korona gęsta, jajowata lub okrągła"],
				SeasonalChanges = new SeasonalChanges {
					Spring = "Młode liście jasno-zielone, często z czerwonawymi nasadami ogonków. Pąki czerwonawe",
					Summer = "Intensywnie pachnące kwiaty przyciągają pszczoły - lipiec to \"miesiąc lipowy\" pszczelarzy",
					Autumn = "Liście żółte, opadają wcześnie. Dojrzałe owoce z przysadkami unoszą się na wietrze",
					Winter = "Charakterystyczne rozgałęzienie, często przystrzyżone w parkach. Pąki czerwonawe"
				},
				Traits = new Traits {
					MaxHeight = 30,
					Lifespan = "800-1000 lat",
					NativeToPoland = true
				}
			},
			new TreeSpecies() {
				Id = Guid.NewGuid(),
				PolishName = "Buk zwyczajny",
				LatinName = "Fagus sylvatica",
				Family = "Fagaceae",
				Description = "Buk zwyczajny to \"król lasów liściastych\" w Polsce. Tworzy charakterystyczne \"katedry bukowe\" - lasy o wysokich, prostych pniach i zwartym sklepieniu koron. Drewno buka jest bardzo twarde i było używane do produkcji narzędzi, mebli i węgla drzewnego.",
				IdentificationGuide = ["Liście owalne, faliste brzegi, wyraźne nerwowanie (6-12 cm długości)",
					"Kora gładka, szara, charakterystyczna przez całe życie drzewa",
					"Owoce - trójkątne orzeszki w kolczastych okrywach (żołędzie bukowe)",
					"Pąki długie, spiczaste, brązowe, charakterystyczne dla gatunku",
					"Korona gęsta, jajowata, rzuca głęboki cień"],
				SeasonalChanges = new SeasonalChanges {
					Spring = "Młode liście jasnozielone, jedwabiste, z delikatnym meszkiem. Kwitnienie w maju",
					Summer = "Ciemnozielone liście tworzą gęsty baldachim. Pod bukami panuje charakterystyczny półmrok",
					Autumn = "Spektakularne żółto-brązowo-miedziane ubarwienie. Opadające liście tworzą grubą ściółkę",
					Winter = "Gładka, szara kora wyraźnie widoczna. Długie, spiczaste pąki na końcach gałązek"
				},
				Traits = new Traits {
					MaxHeight = 45,
					Lifespan = "300-500 lat",
					NativeToPoland = true
				}
			},
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
			new TreeSubmission(){
				Id = Guid.NewGuid(),
				UserId = users[1].Id,
				SpeciesId = species[2].Id,
				Location = new Location {
					Lat = 54.3520,
					Lng = 18.6466,
					Address = "Gdańsk, Park Oliwski, Aleja Bukowa 15"
				},
				Circumference = 420,
				Height = 30,
				Condition = "excellent",
				IsMonument = false,
				Description = "Potężny buk w Parku Oliwskim, jeden z najstarszych okazów w regionie. Drzewo rośnie w malowniczej alei bukowej, która jesienią zamienia się w złoty tunel. Ten konkretny okaz wyróżnia się niezwykle gładką, srebrzystą korą i idealnie symetryczną koroną. Miejscowi biegacze często zatrzymują się pod nim, aby odpocząć.",
				Images = ["https://images.pexels.com/photos/1375849/pexels-photo-1375849.jpeg?w=800&h=600&fit=crop", "https://images.pexels.com/photos/1658967/pexels-photo-1658967.jpeg?w=800&h=600&fit=crop",],
				Status = SubmissionStatus.Approved,
				SubmissionDate = new DateTime(2024, 12, 20),
				ApprovalDate = new DateTime(2024, 1, 25),
			},
			new TreeSubmission(){
				Id = Guid.NewGuid(),
				UserId = users[2].Id,
				SpeciesId = species[1].Id,
				Location = new Location {
					Lat = 50.0647,
					Lng = 19.9450,
					Address = "Kraków, Rynek Główny, przy Sukiennicach"
				},
				Circumference = 380,
				Height = 22,
				Condition = "good",
				IsMonument = false,
				Description = "Piękna lipa na Rynku Głównym w Krakowie, rosnąca w pobliżu Sukiennic. To drzewo pamięta czasy średniowiecza i było świadkiem wielu historycznych wydarzeń. Każdego lata jej kwiaty wypełniają powietrze cudownym aromatem, przyciągając pszczoły z całej okolicy. Lokalni mieszkańcy nazywają ją \"Lipą Mariacką\".",
				Images = ["https://images.pexels.com/photos/1172341/pexels-photo-1172341.jpeg?w=800&h=600&fit=crop", "https://images.pexels.com/photos/1379636/pexels-photo-1379636.jpeg?w=800&h=600&fit=crop",],
				Status = SubmissionStatus.Pending,
				SubmissionDate = new DateTime(2024, 11, 10),
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
				CreatedAt = new DateTime(2024, 2, 21, 15, 50, 00)
			},
			new Vote {
				Id = Guid.NewGuid(),
				TreeSubmissionId = submissions[1].Id,
				UserId = users[0].Id,
				Type = VoteType.Approve,
				CreatedAt = new DateTime(2024, 1, 20)
			},
			new Vote {
				Id = Guid.NewGuid(),
				TreeSubmissionId = submissions[1].Id,
				UserId = users[1].Id,
				Type = VoteType.Approve,
				CreatedAt = new DateTime(2024, 12, 22)
			},
			new Vote {
				Id = Guid.NewGuid(),
				TreeSubmissionId = submissions[1].Id,
				UserId = users[2].Id,
				Type = VoteType.Approve,
				CreatedAt = new DateTime(2024, 12, 23, 13, 10, 52)
			},
			new Vote {
				Id = Guid.NewGuid(),
				TreeSubmissionId = submissions[1].Id,
				UserId = users[3].Id,
				Type = VoteType.Approve,
				CreatedAt = new DateTime(2024, 12, 24)
			},
		];
	}
}