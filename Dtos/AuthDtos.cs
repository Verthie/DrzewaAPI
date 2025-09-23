using System;
using System.ComponentModel.DataAnnotations;

namespace DrzewaAPI.Dtos;

public record AuthResponseDto
{
	public required string AccessToken { get; init; }
	public required string RefreshToken { get; init; }
}

public record ErrorResponseDto
{
	public string Error { get; set; } = string.Empty;
	public string Code { get; set; } = string.Empty;
	public Exception? InnerException { get; set; }
	public string Details { get; set; } = string.Empty;
	public Dictionary<string, string[]> Data { get; set; } = new Dictionary<string, string[]>();
	public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}

public record LoginDto
{
	[Required(ErrorMessage = "Email jest wymagany")]

	[EmailAddress(ErrorMessage = "Nieprawidłowy format email")]
	public required string Email { get; init; }

	[Required(ErrorMessage = "Hasło jest wymagane")]
	public required string Password { get; init; }
}

public record RefreshTokenRequestDto
{
	public string RefreshToken { get; init; } = string.Empty;
}

public record RegisterDto
{
	[Required(ErrorMessage = "Imię jest wymagane")]
	[StringLength(50, MinimumLength = 2, ErrorMessage = "Imię musi mieć od 2 do 50 znaków")]
	public required string FirstName { get; init; }

	[Required(ErrorMessage = "Nazwisko jest wymagane")]
	[StringLength(50, MinimumLength = 2, ErrorMessage = "Nazwisko musi mieć od 2 do 50 znaków")]
	public required string LastName { get; init; }

	[Required(ErrorMessage = "Email jest wymagany")]
	[EmailAddress(ErrorMessage = "Nieprawidłowy format email")]
	public required string Email { get; init; }

	[Required(ErrorMessage = "Hasło jest wymagane")]
	[StringLength(100, MinimumLength = 6, ErrorMessage = "Hasło musi mieć od 6 do 100 znaków")]
	public required string Password { get; init; }
	[Required(ErrorMessage = "Potwierdzenie hasła jest wymagane")]
	[Compare(nameof(Password), ErrorMessage = "Hasła nie są identyczne")]
	public required string ConfirmPassword { get; init; }

	[Phone(ErrorMessage = "Nieprawidłowy numer telefonu")]
	[StringLength(20, MinimumLength = 9, ErrorMessage = "Numer telefonu musi zawierać co najmniej 9 cyfr")]
	public string? Phone { get; init; }
}
