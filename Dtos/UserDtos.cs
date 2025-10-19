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
	public DateTime RegistrationDate { get; init; }
	public OrganizationDto? Organization { get; init; }
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
	public DateTime RegistrationDate { get; init; }
	public OrganizationDto? Organization { get; init; }
	public UserRole Role { get; init; }
	public UserStatisticsDto Statistics { get; init; } = new UserStatisticsDto();
}

public record UpdateUserDto
{
	[Phone(ErrorMessage = "Nieprawidłowy numer telefonu")]
	public string? Phone { get; init; }
	public string? Address { get; init; }
	public string? City { get; init; }
	public string? PostalCode { get; init; }
	public OrganizationDto? Organization { get; init; }
}

public record UpdatePasswordDto
{
	public string Token { get; set; } = string.Empty;
	public required string NewPassword { get; init; }

	[Required(ErrorMessage = "Potwierdzenie hasła jest wymagane")]
	[Compare(nameof(NewPassword), ErrorMessage = "Hasła nie są identyczne")]
	public required string ConfirmPassword { get; init; }
}

public class UserStatisticsDto
{
	public int SubmissionCount { get; init; } = 0;
	public int ApplicationCount { get; init; } = 0;
}

public class OrganizationDto
{
	public string Name { get; set; } = string.Empty;
	public string Address { get; set; } = string.Empty;
	public string PostalCode { get; set; } = string.Empty;
	public string City { get; set; } = string.Empty;
	public string Krs { get; set; } = string.Empty;
	public string Regon { get; set; } = string.Empty;
	public string Mail { get; set; } = string.Empty;
	public string Phone { get; set; } = string.Empty;
	public CorrespondenceDto Correspondence { get; set; } = new CorrespondenceDto();
}

public record CorrespondenceDto
{
	public int PoBox { get; set; } = 0;
	public string Address { get; set; } = string.Empty;
	public string PostalCode { get; set; } = string.Empty;
	public string City { get; set; } = string.Empty;
}