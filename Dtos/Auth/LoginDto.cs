using System;
using System.ComponentModel.DataAnnotations;

namespace DrzewaAPI.Dtos.Auth;

public record LoginDto
{
	[Required(ErrorMessage = "Email jest wymagany")]

	[EmailAddress(ErrorMessage = "Nieprawidłowy format email")]
	public required string Email { get; init; }

	[Required(ErrorMessage = "Hasło jest wymagane")]
	public required string Password { get; init; }
}
