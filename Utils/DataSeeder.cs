using System;
using System.Text.Json;
using DrzewaAPI.Data;
using DrzewaAPI.Models;
using DrzewaAPI.Models.Enums;
using DrzewaAPI.Models.ValueObjects;
using DrzewaAPI.Utils.Mocks;
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

		TreeSubmission[] submissions = GetMockSubmissionsFromJson(users, "Utils\\Mocks\\pomniki_przyrody_to_post.json");
		if (!await _db.TreeSubmissions.AnyAsync(ct)) _db.TreeSubmissions.AddRange(submissions);
		else submissions = await _db.TreeSubmissions.ToArrayAsync(ct);

		TreeVote[] treeVotes = GetMockTreeVotes(submissions, users);
		if (!await _db.TreeVotes.AnyAsync(ct)) _db.TreeVotes.AddRange(treeVotes);
		// else votes = await _db.Votes.ToArrayAsync(ct);

		Comment[] comments = GetMockComments(submissions, users);
		if (!await _db.Comments.AnyAsync(ct)) _db.Comments.AddRange(comments);
		else comments = await _db.Comments.ToArrayAsync(ct);

		CommentVote[] commentVotes = GetMockCommentVotes(comments, users);
		if (!await _db.CommentVotes.AnyAsync(ct)) _db.CommentVotes.AddRange(commentVotes);
		// else likes = await _db.Likes.ToArrayAsync(ct);

		Municipality[] municipalities = GetMockMunicipalities();
		if (!await _db.Municipalities.AnyAsync(ct)) _db.Municipalities.AddRange(municipalities);
		else municipalities = await _db.Municipalities.ToArrayAsync(ct);

		ApplicationTemplate[] applicationTemplates = GetMockTemplates(municipalities);
		if (!await _db.ApplicationTemplates.AnyAsync(ct)) _db.ApplicationTemplates.AddRange(applicationTemplates);
		else applicationTemplates = await _db.ApplicationTemplates.ToArrayAsync(ct);

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

		User user6 = new User
		{
			Id = Guid.NewGuid(),
			FirstName = "Mod",
			LastName = "Erator",
			Email = "mod@example.com",
			Avatar = "https://images.pexels.com/users/avatars/2942047/natureday-com-m-434.jpeg?fit=crop&h=100&w=100",
			RegistrationDate = DateTime.UtcNow,
			Role = UserRole.Moderator
		};
		user6.PasswordHash = _hasher.HashPassword(user6, "string");

		users.Add(user1);
		users.Add(user2);
		users.Add(user3);
		users.Add(user4);
		users.Add(user5);
		users.Add(user6);

		return users;
	}

	private TreeSpecies[] GetMockSpecies()
	{
		return [
			new TreeSpecies() {
				Id = Guid.Parse("e677fa5e-876f-44f4-9542-8d90533ea4fa"),
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
				Id = Guid.Parse("0dc0ebe4-b07f-4724-9dae-88b3823907a9"),
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
				Id = Guid.Parse("9b61a499-4928-45c2-83ea-4906df3a3c32"),
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

	private TreeSubmission[] GetMockSubmissionsFromJson(List<User> users, string jsonFilePath)
	{
		try
		{
			// Read JSON file
			string jsonContent = File.ReadAllText(jsonFilePath);

			// Deserialize JSON to TreeJsonData array
			TreeJsonData[]? treeData = JsonSerializer.Deserialize<TreeJsonData[]>(jsonContent, new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true
			});

			if (treeData == null) throw new ArgumentNullException();

			// Create Random instance for user selection
			Random random = new Random();

			// Convert to TreeSubmission array
			return treeData.Select(data => new TreeSubmission
			{
				Id = Guid.NewGuid(),
				UserId = users[random.Next(users.Count)].Id, // Random user assignment
				SpeciesId = Guid.Parse(data.SpeciesId),
				Location = new Location
				{
					Lat = data.Location.Lat,
					Lng = data.Location.Lng,
					Address = data.Location.Address
				},
				Circumference = data.Circumference,
				Height = data.Height,
				Condition = data.Condition ?? "Unknown",
				IsMonument = data.IsMonument,
				EstimatedAge = data.EstimatedAge,
				Description = data.Description,
				Images = data.Images,
				Status = data.IsMonument ? SubmissionStatus.Monument : SubmissionStatus.Pending,
				SubmissionDate = DateTime.Now.AddDays(-random.Next(1, 365)), // Random date within last year
				ApprovalDate = DateTime.Now.AddDays(-random.Next(1, 30)) // Random approval date within last month
			}).ToArray();
		}
		catch (Exception ex)
		{
			// Handle exceptions (file not found, JSON parsing errors, etc.)
			throw new InvalidOperationException($"Failed to load mock submissions from {jsonFilePath}: {ex.Message}", ex);
		}
	}

	private TreeVote[] GetMockTreeVotes(TreeSubmission[] submissions, List<User> users)
	{
		List<TreeVote> votes = new List<TreeVote>();
		Random random = new Random();

		foreach (TreeSubmission submission in submissions)
		{
			// Generate random number of votes per submission (0 to 5 votes)
			int voteCount = random.Next(0, 6);

			// Keep track of users who already voted for this submission to avoid duplicates
			HashSet<Guid> usersWhoVoted = new HashSet<Guid>();

			for (int i = 0; i < voteCount; i++)
			{
				// Select a random user who hasn't voted yet for this submission
				List<User> availableUsers = users.Where(u => !usersWhoVoted.Contains(u.Id)).ToList();

				// If no users left to vote, break out of the loop
				if (availableUsers.Count == 0) break;

				User selectedUser = availableUsers[random.Next(availableUsers.Count)];
				usersWhoVoted.Add(selectedUser.Id);

				// Random vote type (70% chance for Like, 30% for Dislike)
				VoteType voteType = random.NextDouble() < 0.7 ? VoteType.Like : VoteType.Dislike;

				// Generate random date after submission date but before approval date (if exists)
				DateTime voteDate;
				if (submission.ApprovalDate.HasValue)
				{
					// Vote between submission date and approval date
					TimeSpan timeSpan = submission.ApprovalDate.Value - submission.SubmissionDate;
					TimeSpan randomTimeSpan = new TimeSpan((long)(random.NextDouble() * timeSpan.Ticks));
					voteDate = submission.SubmissionDate.Add(randomTimeSpan);
				}
				else
				{
					// Vote between submission date and now
					TimeSpan timeSpan = DateTime.Now - submission.SubmissionDate;
					TimeSpan randomTimeSpan = new TimeSpan((long)(random.NextDouble() * timeSpan.Ticks));
					voteDate = submission.SubmissionDate.Add(randomTimeSpan);
				}

				votes.Add(new TreeVote
				{
					Id = Guid.NewGuid(),
					TreeSubmissionId = submission.Id,
					UserId = selectedUser.Id,
					Type = voteType,
					CreatedAt = voteDate
				});
			}
		}

		return votes.ToArray();
	}

	private Comment[] GetMockComments(TreeSubmission[] submissions, List<User> users)
	{
		List<Comment> comments = new List<Comment>();
		Random random = new Random();

		// Sample comment templates
		string[] commentTemplates = [
				"Piękny okaz! Widziałem podobne drzewo w dzieciństwie.",
				"To drzewo ma niesamowitą historię. Mieszkam tu od {0} lat i zawsze mnie fascynowało.",
				"Wspaniały pomnik przyrody. Czy ktoś wie, jaki jest dokładny wiek tego drzewa?",
				"Często spaceruję obok tego drzewa. To prawdziwa perła naszego miasta.",
				"Niesamowite, jak potężne może być drzewo! Trzeba je chronić dla przyszłych pokoleń.",
				"Czy są jakieś plany ochrony tego drzewa? Powinno być lepiej zabezpieczone.",
				"To drzewo pamięta więcej niż my wszyscy razem wzięci.",
				"Fascynujące! Czy ktoś zna legendy związane z tym miejscem?",
				"Polecam odwiedzić to miejsce jesienią - widoki są przepiękne!",
				"To jeden z najstarszych okazów w okolicy. Warto go odwiedzić.",
				"Moje dzieci uwielbiają bawić się w pobliżu tego drzewa.",
				"Według lokalnej tradycji, to drzewo ma szczęśliwe właściwości."
		];

		foreach (TreeSubmission submission in submissions)
		{
			// Generate random number of comments per submission (0 to 4 comments)
			int commentCount = random.Next(0, 5);

			// Keep track of users who already commented to avoid duplicates
			HashSet<Guid> usersWhoCommented = new HashSet<Guid>();

			for (int i = 0; i < commentCount; i++)
			{
				// Select a random user who hasn't commented yet for this submission
				List<User> availableUsers = users.Where(u => !usersWhoCommented.Contains(u.Id)).ToList();

				// If no users left to comment, break out of the loop
				if (availableUsers.Count == 0) break;

				User selectedUser = availableUsers[random.Next(availableUsers.Count)];
				usersWhoCommented.Add(selectedUser.Id);

				// Generate random comment content
				string template = commentTemplates[random.Next(commentTemplates.Length)];
				string content = template.Contains("{0}") ?
						string.Format(template, random.Next(5, 50)) : // Random number for years/places
						template;

				// Generate random date after submission date
				DateTime commentDate;
				if (submission.ApprovalDate.HasValue)
				{
					// Comment after approval date
					TimeSpan timeSpan = DateTime.Now - submission.ApprovalDate.Value;
					TimeSpan randomTimeSpan = new TimeSpan((long)(random.NextDouble() * timeSpan.Ticks));
					commentDate = submission.ApprovalDate.Value.Add(randomTimeSpan);
				}
				else
				{
					// Comment after submission date
					TimeSpan timeSpan = DateTime.Now - submission.SubmissionDate;
					TimeSpan randomTimeSpan = new TimeSpan((long)(random.NextDouble() * timeSpan.Ticks));
					commentDate = submission.SubmissionDate.Add(randomTimeSpan);
				}

				// 15% chance for a comment to be marked as legend
				bool isLegend = random.NextDouble() < 0.15;

				comments.Add(new Comment
				{
					Id = Guid.NewGuid(),
					UserId = selectedUser.Id,
					TreeSubmissionId = submission.Id,
					Content = content,
					DatePosted = commentDate,
					IsLegend = isLegend
				});
			}
		}

		return comments.ToArray();
	}

	private CommentVote[] GetMockCommentVotes(Comment[] comments, List<User> users)
	{
		List<CommentVote> votes = new List<CommentVote>();
		Random random = new Random();

		foreach (Comment comment in comments)
		{
			// Generate random number of votes per submission (0 to 5 votes)
			int voteCount = random.Next(0, 6);

			// Keep track of users who already voted for this submission to avoid duplicates
			HashSet<Guid> usersWhoVoted = new HashSet<Guid>();

			for (int i = 0; i < voteCount; i++)
			{
				// Select a random user who hasn't voted yet for this submission
				List<User> availableUsers = users.Where(u => !usersWhoVoted.Contains(u.Id)).ToList();

				// If no users left to vote, break out of the loop
				if (availableUsers.Count == 0) break;

				User selectedUser = availableUsers[random.Next(availableUsers.Count)];
				usersWhoVoted.Add(selectedUser.Id);

				// Random vote type (70% chance for Like, 30% for Dislike)
				VoteType voteType = random.NextDouble() < 0.7 ? VoteType.Like : VoteType.Dislike;

				// Generate random date after comment date
				DateTime voteDate;

				// Vote between comment date and now
				TimeSpan timeSpan = DateTime.Now - comment.DatePosted;
				TimeSpan randomTimeSpan = new TimeSpan((long)(random.NextDouble() * timeSpan.Ticks));
				voteDate = comment.DatePosted.Add(randomTimeSpan);

				votes.Add(new CommentVote
				{
					Id = Guid.NewGuid(),
					CommentId = comment.Id,
					UserId = selectedUser.Id,
					Type = voteType,
					CreatedAt = voteDate
				});
			}
		}

		return votes.ToArray();
	}

	private Municipality[] GetMockMunicipalities()
	{
		return [
			new Municipality {
				Id = Guid.Parse("e677fa5e-876f-44f4-9542-8d90533ea4f1"),
				Name = "Gmina Warszawa",
				Address = "Plac Bankowy 3/5",
				City = "Warszawa",
				Province = "Mazowieckie",
				PostalCode = "00-950",
				Phone = "+48 22 443 01 00",
				Email = "sekretariat@um.warszawa.pl",
				Website = "https://www.warszawa.pl"
			},
			new Municipality {
				Id = Guid.Parse("e677fa5e-876f-44f4-9542-8d90533ea4f2"),
				Name = "Gmina Kraków",
				Address = "Pl. Wszystkich Świętych 3-4",
				City = "Kraków",
				Province = "Małopolskie",
				PostalCode = "31-004",
				Phone = "+48 12 616 5555",
				Email = "ws.umk@um.krakow.pl",
				Website = "https://www.krakow.pl"
			},
			new Municipality {
				Id = Guid.Parse("e677fa5e-876f-44f4-9542-8d90533ea4f3"),
				Name = "Gmina Piaseczno",
				Address = "ul. Kościuszki 5",
				City = "Piaseczno",
				Province = "Mazowieckie",
				PostalCode = "30-015",
				Phone = "+48 12 687 7844",
				Email = "sekretariat@um.piaseczno.pl",
			}
		];
	}

	private ApplicationTemplate[] GetMockTemplates(Municipality[] municipalities)
	{
		return [
				new ApplicationTemplate
				{
						Id = Guid.NewGuid(),
						MunicipalityId = municipalities[0].Id,
						Name = "Wniosek o rejestrację pomnika przyrody",
						Description = "Standardowy szablon wniosku o rejestrację drzewa jako pomnika przyrody",
						HtmlTemplate = "<!DOCTYPE html><html><head><meta charset=\"utf-8\"><title>Wniosek o rejestrację pomnika przyrody</title></head><body><h1>WNIOSEK O REJESTRACJĘ POMNIKA PRZYRODY</h1><div><h3>{{municipality_name}}</h3><p>{{municipality_address}}, {{municipality_city}} {{municipality_postal_code}}</p></div><div><h3>Dane wnioskodawcy:</h3><p>Imię i nazwisko: {{user_full_name}}</p><p>Adres: {{user_address}}, {{user_city}} {{user_postal_code}}</p><p>Telefon: {{user_phone}}</p><p>Email: {{user_email}}</p></div><div><h3>Dane drzewa:</h3><p>Gatunek: {{tree_species_polish}}</p><p>Obwód: {{tree_circumference}} cm</p><p>Wysokość: {{tree_height}} m</p><p>Wiek: {{tree_estimated_age}} lat</p><p>Stan: {{tree_condition}}</p></div><div><h3>Dodatkowe informacje:</h3><p>Uzasadnienie: {{justification}}</p><p>Przewidywany koszt opieki: {{estimated_care_cost}} zł/rok</p><p>Osoba odpowiedzialna: {{responsible_person}}</p><p>Telefon kontaktowy: {{contact_phone}}</p></div><div><p>Data: {{submission_date}}</p><p>Podpis: ................................</p></div></body></html>",
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
												MaxLength = 1000,
												ValidationMessage = "Uzasadnienie musi mieć od 50 do 1000 znaków"
										},
										HelpText = "Opisz walory przyrodnicze, historyczne lub krajobrazowe drzewa",
										Order = 1
								},
								new ApplicationField
								{
										Name = "estimated_care_cost",
										Label = "Szacowany koszt rocznej opieki (zł)",
										Type = ApplicationFieldType.Number,
										IsRequired = true,
										Placeholder = "np. 500",
										Validation = new ApplicationFieldValidation
										{
												Min = 0,
												Max = 10000,
												ValidationMessage = "Koszt musi być liczbą od 0 do 10000"
										},
										HelpText = "Przewidywany koszt opieki nad drzewem w ciągu roku",
										Order = 2
								},
								new ApplicationField
								{
										Name = "responsible_person",
										Label = "Osoba odpowiedzialna za opiekę",
										Type = ApplicationFieldType.Text,
										IsRequired = true,
										Placeholder = "Imię i nazwisko",
										Validation = new ApplicationFieldValidation
										{
												MinLength = 3,
												MaxLength = 100
										},
										Order = 3
								},
								new ApplicationField
								{
										Name = "contact_phone",
										Label = "Telefon kontaktowy osoby odpowiedzialnej",
										Type = ApplicationFieldType.Phone,
										IsRequired = true,
										Placeholder = "+48 123 456 789",
										Validation = new ApplicationFieldValidation
										{
												Pattern = @"^\+?[0-9\s\-\(\)]{9,15}$",
												ValidationMessage = "Numer telefonu musi zawierać 9-15 cyfr"
										},
										Order = 4
								},
								new ApplicationField
								{
										Name = "care_agreement",
										Label = "Zobowiązuję się do sprawowania opieki nad drzewem",
										Type = ApplicationFieldType.Checkbox,
										IsRequired = true,
										DefaultValue = "false",
										HelpText = "Wymagane potwierdzenie zobowiązania",
										Order = 5
								}
						}
				},
				new ApplicationTemplate
				{
						Id = Guid.NewGuid(),
						MunicipalityId = municipalities[1].Id,
						Name = "Uznanie obiektu przyrodniczego za pomnik przyrody WS-13",
						Description = "Standardowy szablon wniosku o rejestrację drzewa jako pomnika przyrody",
						HtmlTemplate = "<!DOCTYPE html><html><meta charset=UTF-8><style>body{font-family:Arial,sans-serif;font-size:12px}.header{text-align:left;margin:0 5px 20px 5px;font-size:12px}.title{margin:42px 5px 42px 5px}.title h1{text-align:center;font-size:22px;font-weight:300;margin:0}.title h2{text-align:center;font-size:15px;margin:0 0 12px 0}.title p{font-size:14px}table{border-collapse:collapse;margin:auto}td{border:1px solid #000;padding:4px 8px 8px 8px;vertical-align:top}.number-col{width:30px;text-align:center}.question-col{width:42%}.answer-col{width:55%}.footer{display:flex;justify-content:space-between;align-items:flex-start;margin:64px 5px 0 5px}.footer *{margin:0}.signature{display:flex;flex-direction:column;text-align:center;justify-content:center}.signature-text{font-size:10px}</style><div class=header>Załącznik do procedury WS-13</div><div class=title><h1>Wniosek</h1><h2>o uznanie obiektu przyrodniczego za pomnik przyrody</h2><p>na podstawie Art. 6 ust. 1 pkt 6, art. 40, art. 44 ustawy z dnia 16 kwietnia 2004 r. o ochronie przyrody.</div><table><tr><td class=number-col>1.<td class=question-col>Imię i nazwisko wnioskodawcy / nazwa wnioskodawcy<br>Adres / siedziba wnioskodawcy<td class=answer-col>{{user_full_name}}<br>{{user_address}}<br>{{user_city}}, {{user_postal_code}}<tr><td class=number-col>2.<td class=question-col>Nazwa i rodzaj pomnika przyrody<td class=answer-col>Nazwa polska: {{tree_species_polish}}<br>Nazwa łacińska: {{tree_species_latin}}<br>Rodzaj: drzewo<tr><td class=number-col>3.<td class=question-col>Określenie położenia geograficznego i administracyjnego pomnika przyrody (działka, obręb ewidencyjny, jednostka ewidencyjna)<td class=answer-col>Położenie geograficzne: {{geographic_location_lat}} lat, {{geographic_location_long}} long<br>Działka: {{plot}}<br>Obręb ewidencyjny: {{cadastral_district}}<br>Jednostka ewidencyjna: {{record_keeping_unit}}<tr><td class=number-col>4.<td class=question-col>Wskazanie formy własności i rodzajów gruntów<td class=answer-col>Forma własności: {{ownership_form}}<br>Rodzaj gruntów: {{land_type}}<tr><td class=number-col>5.<td class=question-col>Wskazanie mapy obrazującej lokalizację pomnika przyrody<td class=answer-col><tr><td class=number-col>6.<td class=question-col>Krótki opis pomnika przyrody<br>- dla pomników przyrody żywej gatunek, wiek, pierśnica, wysokość, rozpiętość korony, stan zdrowotny,<br>- dla pomników przyrody nieżywej typ, rodzaj, wielkość źródła, wodospadu, głazu, jaskini itp.<td class=answer-col>Wiek: {{tree_estimated_age}}<br>Pierśnica: {{tree_circumference}} cm<br>Wysokość: {{tree_height}} m<br>Rozpiętość:<br>Stan zdrowotny: {{tree_condition}}<tr><td class=number-col>7.<td class=question-col>Nazwa, autor opracowania potwierdzającego wartości przyrodnicze obiektu<td class=answer-col>Nazwa opracowania: {{study_name}}<br>Autor: {{study_author}}</table><div class=footer><p>{{municipality_city}}, dn. {{generation_date}}<div class=signature><p>..............................................<p class=signature-text><em>(podpis)</em></div></div></body></html>",
						Fields = new List<ApplicationField>
						{
								new ApplicationField
								{
										Name = "plot",
										Label = "Działka",
										Type = ApplicationFieldType.TextArea,
										IsRequired = true,
										Placeholder = "Proszę podać działkę na której znajduje się pomnik przyrody",
										Validation = new ApplicationFieldValidation
										{
												MinLength = 2,
												MaxLength = 100,
												ValidationMessage = "Tekst musi mieć od 2 do 100 znaków"
										},
										Order = 1
								},
								new ApplicationField
								{
										Name = "cadastral_district",
										Label = "Obręb ewidencyjny",
										Type = ApplicationFieldType.TextArea,
										IsRequired = true,
										Placeholder = "Proszę podać obręb ewidencyjny",
										Validation = new ApplicationFieldValidation
										{
												MinLength = 2,
												MaxLength = 100,
												ValidationMessage = "Tekst musi mieć od 2 do 100 znaków"
										},
										Order = 2
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
												MinLength = 2,
												MaxLength = 100,
												ValidationMessage = "Tekst musi mieć od 2 do 100 znaków"
										},
										Order = 3
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
												MinLength = 2,
												MaxLength = 100,
												ValidationMessage = "Tekst musi mieć od 2 do 100 znaków"
										},
										Order = 4
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
												MinLength = 2,
												MaxLength = 100,
												ValidationMessage = "Tekst musi mieć od 2 do 100 znaków"
										},
										Order = 5
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
												MinLength = 2,
												MaxLength = 100,
												ValidationMessage = "Tekst musi mieć od 2 do 100 znaków"
										},
										Order = 6
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
												MinLength = 2,
												MaxLength = 100,
												ValidationMessage = "Tekst musi mieć od 2 do 100 znaków"
										},
										Order = 7
								},
						}
				}
		 ];
	}
}