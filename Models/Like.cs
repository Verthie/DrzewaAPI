using System;

namespace DrzewaAPI.Models;

public class Like
{
	public Guid Id { get; set; }
	public Guid UserId { get; set; }
	public Guid CommentId { get; set; }

	// Navigation Properties
	public User User { get; set; } = default!;
	public Comment Comment { get; set; } = default!;
}
