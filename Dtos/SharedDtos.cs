using System;

namespace DrzewaAPI.Dtos;

public record UserDataDto
{
	public Guid UserId { get; set; }
	public required string UserName { get; init; }
	public string? Avatar { get; init; }
}

public record VotesDto
{
	public int Like { get; set; } = 0;
	public int Dislike { get; set; } = 0;
}
