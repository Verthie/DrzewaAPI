using System;
using DrzewaAPI.Dtos.User;
using DrzewaAPI.Models;
using DrzewaAPI.Models.Enums;

namespace DrzewaAPI.Services;

public interface IUserService
{
	Task<List<UserDto>> GetAllUsersAsync();
	Task<UserDto> GetUserByIdAsync(Guid userId);
	Task<UserDto> UpdateUserAsync(Guid currentUserId, Guid userId, UpdateUserDto updateDto, bool isModerator);
	// Task<bool> DeactivateUserAsync(Guid userId);
	// Task<bool> ActivateUserAsync(Guid userId);
}
