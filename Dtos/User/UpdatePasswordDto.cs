using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DrzewaAPI.Dtos.User;

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
