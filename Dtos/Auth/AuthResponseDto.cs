using System;

namespace DrzewaAPI.Dtos.Auth;

public record AuthResponseDto
{
	public string? Token { get; init; }
	public UserDto? User { get; init; }
}
