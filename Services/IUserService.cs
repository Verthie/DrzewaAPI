using System;
using DrzewaAPI.Dtos.User;
using DrzewaAPI.Models;

namespace DrzewaAPI.Services;

public interface IUserService
{
	Task<List<UserDetailDto>> GetAllUsersAsync();
	Task<UserDetailDto?> GetUserByIdAsync(Guid userId);
	Task<UserDetailDto?> UpdateUserAsync(Guid userId, UpdateUserDto updateDto);
	Task UpdateUserStatsAsync(Guid userId);
	// Task<bool> DeactivateUserAsync(Guid userId);
	// Task<bool> ActivateUserAsync(Guid userId);
}
