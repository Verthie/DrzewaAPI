using System;
using DrzewaAPI.Utils;

namespace DrzewaAPI.Models;

public class CommentVote : Vote
{
	public Guid CommentId { get; set; }

	// Navigation Properties
	public Comment Comment { get; set; } = default!;
}
