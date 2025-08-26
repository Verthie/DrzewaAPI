using System;

namespace DrzewaAPI.Models.ValueObjects;

public record UserData
{
	public required string UserName { get; init; }
	public string? Avatar { get; init; }
}
