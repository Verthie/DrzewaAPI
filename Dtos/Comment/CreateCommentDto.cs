using System;

namespace DrzewaAPI.Dtos.Comment;

public record CreateCommentDto
{
	public required string Content { get; init; }
	public bool IsLegend { get; init; }
}
