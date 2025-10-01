using System;

namespace DrzewaAPI.Dtos;

public record UserDataDto
{
	public Guid UserId { get; init; }
	public required string UserName { get; init; }
	public string? Avatar { get; init; }
}