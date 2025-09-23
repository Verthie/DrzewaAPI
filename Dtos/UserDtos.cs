using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DrzewaAPI.Models.Enums;

namespace DrzewaAPI.Dtos;

public record UserDto
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
	public UserStatisticsDto Statistics { get; init; } = new UserStatisticsDto();
}

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
	public UserStatisticsDto Statistics { get; init; } = new UserStatisticsDto();
}

public record UpdateUserDto
{
	[DefaultValue(null)]
	public string? UserId { get; set; } = null;
	[Phone(ErrorMessage = "Nieprawidłowy numer telefonu")]
	public string? Phone { get; init; }
	public string? Address { get; init; }
	public string? City { get; init; }
	public string? PostalCode { get; init; }
}

public record UpdatePasswordDto
{
	[DefaultValue(null)]
	public string? UserId { get; set; } = null;
	public string OldPassword { get; init; } = string.Empty;
	public required string NewPassword { get; init; }

	[Required(ErrorMessage = "Potwierdzenie hasła jest wymagane")]
	[Compare(nameof(NewPassword), ErrorMessage = "Hasła nie są identyczne")]
	public required string ConfirmNewPassword { get; init; }
}

public class UserStatisticsDto
{
	public int SubmissionCount { get; set; } = 0;
	public int ApplicationCount { get; set; } = 0;
}