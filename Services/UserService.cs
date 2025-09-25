using DrzewaAPI.Data;
using DrzewaAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DrzewaAPI.Services;

public class UserService(ApplicationDbContext _context, IAzureStorageService _azureStorageService, IPasswordHasher<User> _passwordHasher, ILogger<UserService> _logger) : IUserService
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
					Statistics = new UserStatisticsDto
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

	public async Task<UserDto> UpdateUserAsync(Guid currentUserId, Guid userId, UpdateUserDto updateDto, IFormFile image, bool isModerator)
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
			user.Address = updateDto.Address?.Trim();
			user.City = updateDto.City?.Trim();
			user.PostalCode = updateDto.PostalCode?.Trim();


			// Handle image upload
			if (image != null)
			{
				try
				{
					string folderPath = $"uploads/user-avatars/{user.Id}";
					List<string> imagePaths = await _azureStorageService.SaveImagesAsync(new FormFileCollection() { image }, folderPath);
					user.Avatar = imagePaths[0];
					await _context.SaveChangesAsync();
				}
				catch (Exception ex)
				{
					_logger.LogError(ex, "Error saving images for user {Id}", user.Id);
					// TODO Notify the user that images couldn't be saved
				}
			}

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

	public async Task UpdatePasswordAsync(Guid userId, string newPassword)
	{
		try
		{
			User user = await _context.Users
					.FirstOrDefaultAsync(u => u.Id == userId)
					?? throw EntityNotFoundException.ForUser(userId);

			// Field update
			user.PasswordHash = _passwordHasher.HashPassword(user, newPassword);

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
			Statistics = new UserStatisticsDto()
			{
				ApplicationCount = u.Applications.Count,
				SubmissionCount = u.TreeSubmissions.Count
			}
		};
	}
}
