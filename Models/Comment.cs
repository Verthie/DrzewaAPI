using System;

namespace DrzewaAPI.Models;

public class Comment
{
	public Guid Id { get; set; }
	public Guid TreeReportId { get; set; }
	public Guid UserId { get; set; }
	public required string Content { get; set; }
	public DateTime CreatedAt { get; set; }
	public DateTime? UpdatedAt { get; set; }


	// Navigation Properties
	public User? User { get; set; }
	public TreeSubmission? TreeReport { get; set; }
}