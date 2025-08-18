using System;
using DrzewaAPI.Data;
using DrzewaAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DrzewaAPI.Utils;

public class DataSeeder(ApplicationDbContext _db, IPasswordHasher<User> _hasher, ILogger<DataSeeder> _logger) : IDataSeeder
{
	public async Task SeedAsync(CancellationToken ct = default)
	{
		// await _db.Database.MigrateAsync(ct);
		if (await _db.Users.AnyAsync(ct)) return;   // idempotent guard

		List<User> users = GetMockUsers();

		_db.Users.AddRange(users);
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
			SubmissionsCount = 12,
			VerificationsCount = 45,
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
			SubmissionsCount = 8,
			VerificationsCount = 32,
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
			SubmissionsCount = 15,
			VerificationsCount = 28,
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
			SubmissionsCount = 22,
			VerificationsCount = 67,
		};
		user4.PasswordHash = _hasher.HashPassword(user4, "123456789");

		users.Add(user1);
		users.Add(user2);
		users.Add(user3);
		users.Add(user4);

		return users;
	}
}