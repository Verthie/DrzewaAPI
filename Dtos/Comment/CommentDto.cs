using System;
using DrzewaAPI.Models.ValueObjects;

namespace DrzewaAPI.Dtos.Comment;

public record CommentDto
{
	public Guid Id { get; set; }
	public string? TreePolishName { get; set; }
	public required UserData UserData { get; set; }
	public required string Content { get; set; }
	public DateTime DatePosted { get; set; }
	public bool IsLegend { get; set; }
	public required VotesCount Votes { get; set; }
}
