using DrzewaAPI.Models.Enums;

namespace DrzewaAPI.Dtos.User;

public record class UserDetailDto
{
	public Guid Id { get; init; }
	public required string Email { get; init; }
	public required string FirstName { get; init; }
	public required string LastName { get; init; }
	public string? Phone { get; init; }
	public string? Avatar { get; init; }
	public DateTime RegistrationDate { get; init; }
	public int SubmissionsCount { get; init; }
	public int VerificationsCount { get; init; }
	public UserRole Role { get; init; }
}
