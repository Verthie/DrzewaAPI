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

	public async Task DeleteUserAsync(Guid currentUserId, Guid userId, bool isModerator)
	{
		using var transaction = await _context.Database.BeginTransactionAsync();
		try
		{
			User user = await _context.Users
					.FirstOrDefaultAsync(u => u.Id == userId)
					?? throw EntityNotFoundException.ForUser(userId);

			if (!isModerator && user.Id != currentUserId) throw EntityAccessDeniedException.ForUser(userId);

			_logger.LogInformation("Starting cascade deletion for user {UserId}", userId);

			// 1. Delete EmailVerificationTokens
			var emailTokens = await _context.EmailVerificationTokens
					.Where(t => t.UserId == userId)
					.ToListAsync();
			if (emailTokens.Any())
			{
				_context.EmailVerificationTokens.RemoveRange(emailTokens);
				_logger.LogDebug("Deleted {Count} email verification tokens for user {UserId}", emailTokens.Count, userId);
			}

			// 2. Delete CommentVotes
			var commentVotes = await _context.CommentVotes
					.Where(cv => cv.UserId == userId)
					.ToListAsync();
			if (commentVotes.Any())
			{
				_context.CommentVotes.RemoveRange(commentVotes);
				_logger.LogDebug("Deleted {Count} comment votes for user {UserId}", commentVotes.Count, userId);
			}

			// 3. Delete TreeVotes
			var treeVotes = await _context.TreeVotes
					.Where(tv => tv.UserId == userId)
					.ToListAsync();
			if (treeVotes.Any())
			{
				_context.TreeVotes.RemoveRange(treeVotes);
				_logger.LogDebug("Deleted {Count} tree votes for user {UserId}", treeVotes.Count, userId);
			}

			// 4. Delete Comments
			var comments = await _context.Comments
					.Where(c => c.UserId == userId)
					.ToListAsync();
			if (comments.Any())
			{
				_context.Comments.RemoveRange(comments);
				_logger.LogDebug("Deleted {Count} comments for user {UserId}", comments.Count, userId);
			}

			// 5. Delete Applications
			var applications = await _context.Applications
					.Where(a => a.UserId == userId)
					.ToListAsync();
			if (applications.Any())
			{
				_context.Applications.RemoveRange(applications);
				_logger.LogDebug("Deleted {Count} applications for user {UserId}", applications.Count, userId);
			}

			// 6. Delete TreeSubmissions (these might have complex relationships)
			var treeSubmissions = await _context.TreeSubmissions
					.Where(ts => ts.UserId == userId)
					.ToListAsync();
			if (treeSubmissions.Any())
			{
				_context.TreeSubmissions.RemoveRange(treeSubmissions);
				_logger.LogDebug("Deleted {Count} tree submissions for user {UserId}", treeSubmissions.Count, userId);
			}

			// 7. Finally, delete the user
			_context.Users.Remove(user);

			// Save all changes
			await _context.SaveChangesAsync();
			await transaction.CommitAsync();

			_logger.LogInformation("Successfully deleted user {UserId} and all associated data", userId);
		}
		catch (BusinessException)
		{
			await transaction.RollbackAsync();
			throw;
		}
		catch (Exception ex)
		{
			await transaction.RollbackAsync();
			_logger.LogError(ex, "Błąd podczas kaskadowego usuwania użytkownika {UserId}", userId);
			throw new ServiceException($"Nie można usunąć użytkownika {userId}", "USER_DELETE_ERROR");
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
