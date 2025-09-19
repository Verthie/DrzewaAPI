using System;
using DrzewaAPI.Dtos.User;

namespace DrzewaAPI.Dtos.Auth;

public record AuthResponseDto
{
	public required string AccessToken { get; init; }
	public required string RefreshToken { get; init; }
	public required UserDto User { get; init; }
}
