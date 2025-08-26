using System;

namespace DrzewaAPI.Dtos.Comment;

public record CreateCommentDto
{
	public required string Content { get; set; }
	public bool IsLegend { get; set; }
}
