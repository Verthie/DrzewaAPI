using System;
using DrzewaAPI.Data;
using DrzewaAPI.Dtos.User;
using DrzewaAPI.Models;
using DrzewaAPI.Models.Enums;
using DrzewaAPI.Models.ValueObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DrzewaAPI.Services;

public class UserService(ApplicationDbContext _context, IPasswordHasher<User> _passwordHasher, ILogger<UserService> _logger) : IUserService
{
	public async Task<List<UserDto>> GetAllUsersAsync()
	{
		try
		{
			List<UserDto> users = await _context.Users
				.Include(u => u.TreeSubmissions)
				.Include(u => u.Applications)
				.Where(u => u.Role == UserRole.User)
				.Select(u => MapToUserDto(u))
				.ToListAsync();

			return users;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Błąd podczas pobierania listy użytkowników");
			throw new ServiceException($"Nie można pobrać listy użytkowników", "USER_FETCH_ERROR");
		}
	}

	public async Task<T> GetUserByIdAsync<T>(Guid userId)
	{
		try
		{
			User user = await _context.Users
				.Include(u => u.TreeSubmissions)
				.Include(u => u.Applications)
				.FirstOrDefaultAsync(u => u.Id == userId)
				?? throw EntityNotFoundException.ForUser(userId);

			return typeof(T) == typeof(UserDto)
				? (T)(object)MapToUserDto(user)
				: (T)(object)new CurrentUserDto
				{
					Id = user.Id,
					Email = user.Email,
					Name = user.FullName,
					Phone = user.Phone,
					Address = user.Address,
					City = user.City,
					PostalCode = user.PostalCode,
					Avatar = user.Avatar,
					RegistrationDate = user.RegistrationDate,
					Role = user.Role,
					Statistics = new UserStatistics
					{
						ApplicationCount = user.Applications.Count,
						SubmissionCount = user.TreeSubmissions.Count
					}
				};
		}
		catch (BusinessException)
		{
			throw;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Błąd podczas pobierania użytkownika {UserId}", userId);
			throw new ServiceException($"Nie można pobrać użytkownika {userId}", "USER_FETCH_ERROR");
		}
	}

	public async Task<UserDto> UpdateUserAsync(Guid currentUserId, Guid userId, UpdateUserDto updateDto, bool isModerator)
	{
		try
		{
			User user = await _context.Users
					.FirstOrDefaultAsync(u => u.Id == userId)
					?? throw EntityNotFoundException.ForUser(userId);

			// Check privileges
			if (!isModerator && user.Id != currentUserId) throw EntityAccessDeniedException.ForUser(userId);

			// Field update
			user.Phone = updateDto.Phone?.Trim();
			user.Avatar = updateDto.Avatar?.Trim();
			user.Address = updateDto.Address?.Trim();
			user.City = updateDto.City?.Trim();
			user.PostalCode = updateDto.PostalCode?.Trim();

			await _context.SaveChangesAsync();

			_logger.LogInformation("Zaktualizowano dane użytkownika: {UserId}", userId);

			return await GetUserByIdAsync<UserDto>(userId);
		}
		catch (BusinessException)
		{
			throw;
		}
		catch (DbUpdateException ex)
		{
			_logger.LogError(ex, "Błąd podczas wprowadzania danych do bazy");
			throw EntityUpdateFailedException.ForUser(userId, "Błąd podczas zapisu do bazy danych");
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Nieoczekiwany błąd podczas aktualizacji danych użytkownika");
			throw EntityUpdateFailedException.ForUser(userId, "Nieoczekiwany błąd systemu");
		}
	}

	public async Task UpdatePasswordAsync(Guid currentUserId, Guid userId, UpdatePasswordDto updatePasswordDto, bool isModerator)
	{
		try
		{
			User user = await _context.Users
					.FirstOrDefaultAsync(u => u.Id == userId)
					?? throw EntityNotFoundException.ForUser(userId);

			// Check privileges
			if (!isModerator && user.Id != currentUserId) throw EntityAccessDeniedException.ForUser(userId);

			if (!isModerator)
			{
				PasswordVerificationResult passwordResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, updatePasswordDto.OldPassword);
				if (passwordResult == PasswordVerificationResult.Failed) throw AccountException.ForIncorrectPassword();
			}

			// Field update
			user.PasswordHash = _passwordHasher.HashPassword(user, updatePasswordDto.NewPassword);

			await _context.SaveChangesAsync();

			_logger.LogInformation("Zaktualizowano hasło użytkownika: {UserId}", userId);
		}
		catch (BusinessException)
		{
			throw;
		}
		catch (DbUpdateException ex)
		{
			_logger.LogError(ex, "Błąd podczas wprowadzania danych do bazy");
			throw EntityUpdateFailedException.ForUser(userId, "Błąd podczas zapisu do bazy danych");
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Nieoczekiwany błąd podczas aktualizacji hasła użytkownika");
			throw EntityUpdateFailedException.ForUser(userId, "Nieoczekiwany błąd systemu");
		}
	}

	private static UserDto MapToUserDto(User u)
	{
		return new UserDto
		{
			Id = u.Id,
			Email = u.Email,
			Name = u.FullName,
			Phone = u.Phone,
			Address = u.Address,
			City = u.City,
			PostalCode = u.PostalCode,
			Avatar = u.Avatar,
			RegistrationDate = u.RegistrationDate,
			Statistics = new UserStatistics()
			{
				ApplicationCount = u.Applications.Count,
				SubmissionCount = u.TreeSubmissions.Count
			}
		};
	}
}
