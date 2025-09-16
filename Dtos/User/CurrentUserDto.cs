using DrzewaAPI.Models.Enums;

namespace DrzewaAPI.Dtos.User;

public record CurrentUserDto
{
	public Guid Id { get; init; }
	public required string Email { get; init; }
	public required string Name { get; init; }
	public string? Phone { get; set; }
	public string? Address { get; set; }
	public string? City { get; set; }
	public string? PostalCode { get; set; }
	public string? Avatar { get; set; }
	public DateTime RegistrationDate { get; init; }
	public UserRole Role { get; init; }
}
