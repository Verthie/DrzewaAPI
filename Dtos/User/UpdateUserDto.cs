using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DrzewaAPI.Dtos.User;

public record UpdateUserDto
{
	[DefaultValue(null)]
	public string? UserId { get; set; } = null;
	[Phone(ErrorMessage = "Nieprawidłowy numer telefonu")]
	public string? Phone { get; init; }
	public string? Address { get; init; }
	public string? City { get; init; }
	public string? PostalCode { get; init; }

	[Url(ErrorMessage = "Nieprawidłowy format URL avatara")]
	public string? Avatar { get; init; }
}
