namespace DrzewaAPI.Services;

public interface IUserService
{
	Task<List<UserDto>> GetAllUsersAsync();
	Task<T> GetUserByIdAsync<T>(Guid userId);
	Task<UserDto> UpdateUserAsync(Guid currentUserId, Guid userId, UpdateUserDto updateDto, bool isModerator);
	Task UpdatePasswordAsync(Guid currentUserId, Guid userId, UpdatePasswordDto updatePasswordDto, bool isModerator);
	// Task<bool> DeactivateUserAsync(Guid userId);
	// Task<bool> ActivateUserAsync(Guid userId);
}
