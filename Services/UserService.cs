using System;
using DrzewaAPI.Data;
using DrzewaAPI.Dtos.User;
using DrzewaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DrzewaAPI.Services;

public class UserService(ApplicationDbContext _context, ILogger<UserService> _logger) : IUserService
{

	public async Task<List<UserDetailDto>> GetAllUsersAsync()
	{
		try
		{
			List<User> users = await _context.Users.ToListAsync();

			List<UserDetailDto> userDtos = users
									.Select(u => new UserDetailDto
									{
										Id = u.Id,
										Name = u.FullName,
										Email = u.Email,
										Phone = u.Phone,
										Avatar = u.Avatar,
										RegistrationDate = u.RegistrationDate,
										SubmissionsCount = u.SubmissionsCount,
										VerificationsCount = u.VerificationsCount,
										Role = u.Role
									})
									.ToList();

			return userDtos;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Błąd podczas pobierania listy użytkowników");
			throw;
		}
	}

	public async Task<UserDetailDto?> GetUserByIdAsync(Guid userId)
	{
		try
		{
			User? user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

			ArgumentNullException.ThrowIfNull(user);

			return new UserDetailDto
			{
				Id = user.Id,
				Name = user.FullName,
				Email = user.Email,
				Phone = user.Phone,
				Avatar = user.Avatar,
				RegistrationDate = user.RegistrationDate,
				SubmissionsCount = user.SubmissionsCount,
				VerificationsCount = user.VerificationsCount,
				Role = user.Role
			};
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Błąd podczas pobierania użytkownika o ID: {UserId}", userId);
			throw;
		}
	}

	public async Task<UserDetailDto?> UpdateUserAsync(Guid userId, UpdateUserDto updateDto)
	{
		try
		{
			var user = await _context.Users
					.FirstOrDefaultAsync(u => u.Id == userId);

			if (user == null)
			{
				return null;
			}

			// Field update
			user.Phone = updateDto.Phone?.Trim();
			user.Avatar = updateDto.Avatar?.Trim();

			await _context.SaveChangesAsync();

			_logger.LogInformation("Zaktualizowano dane użytkownika: {UserId}", userId);

			return await GetUserByIdAsync(userId);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Błąd podczas aktualizacji użytkownika: {UserId}", userId);
			throw;
		}
	}

	public async Task UpdateUserStatsAsync(Guid userId)
	{
		try
		{
			var user = await _context.Users.FindAsync(userId);
			if (user == null) return;

			// TODO
			// Tutaj będzie logika liczenia statystyk gdy dodamy TreeReports i Votes

			// Przykład:
			// user.SubmissionsCount = await _context.TreeReports.CountAsync(r => r.UserId == userId);
			// user.VerificationsCount = await _context.Votes.CountAsync(v => v.UserId == userId && v.VoteType.Approved);

			await _context.SaveChangesAsync();

			_logger.LogInformation("Zaktualizowano statystyki użytkownika: {UserId}", userId);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Błąd podczas aktualizacji statystyk użytkownika: {UserId}", userId);
			throw;
		}
	}
}
