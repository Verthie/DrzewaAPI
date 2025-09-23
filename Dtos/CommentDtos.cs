using System;

namespace DrzewaAPI.Dtos;

public record CommentDto
{
	public Guid Id { get; set; }
	public string? TreePolishName { get; set; }
	public required UserDataDto UserData { get; set; }
	public required string Content { get; set; }
	public DateTime DatePosted { get; set; }
	public bool IsLegend { get; set; }
	public required VotesDto Votes { get; set; }
}

public record CreateCommentDto
{
	public required string Content { get; init; }
	public bool IsLegend { get; init; }
}
