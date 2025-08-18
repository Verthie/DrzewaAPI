using System;

namespace DrzewaAPI.Dtos.Auth;

public record AuthResponseDto
{
	public bool Success { get; init; }
	public string? Token { get; init; }
	public UserDto? User { get; init; }
	public string? ErrorMessage { get; init; }
	public List<string> Errors { get; init; } = new();
}
