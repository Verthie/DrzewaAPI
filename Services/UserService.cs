using System;
using DrzewaAPI.Data;
using DrzewaAPI.Dtos.User;
using DrzewaAPI.Models;
using DrzewaAPI.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace DrzewaAPI.Services;

public class UserService(ApplicationDbContext _context, ILogger<UserService> _logger) : IUserService
{
	public async Task<List<UserDto>> GetAllUsersAsync()
	{
		try
		{
			List<UserDto> users = await _context.Users
				.Select(u => MapToUserDto(u))
				.ToListAsync();

			return users;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Błąd podczas pobierania listy użytkowników");
			throw;
		}
	}

	public async Task<UserDto?> GetUserByIdAsync(Guid userId)
	{
		try
		{
			User? user = await _context.Users
				.Include(u => u.TreeSubmissions)
				.FirstOrDefaultAsync(u => u.Id == userId);

			ArgumentNullException.ThrowIfNull(user);

			return MapToUserDto(user);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Błąd podczas pobierania użytkownika o ID: {UserId}", userId);
			throw;
		}
	}

	public async Task<UserDto?> UpdateUserAsync(Guid userId, UpdateUserDto updateDto)
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
			user.Address = updateDto.Address?.Trim();
			user.City = updateDto.City?.Trim();
			user.PostalCode = updateDto.PostalCode?.Trim();

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
		};
	}
}
