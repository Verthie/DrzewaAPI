using System;
using System.ComponentModel.DataAnnotations;

namespace DrzewaAPI.Dtos.Auth;

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
	public string? Phone { get; init; }
}
