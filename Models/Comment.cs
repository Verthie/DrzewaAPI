using System;

namespace DrzewaAPI.Models;

public class Comment
{
	public required Guid Id { get; set; }
	public required Guid TreeReportId { get; set; }
	public required Guid UserId { get; set; }
	public required string Content { get; set; }
	public bool IsLegend { get; set; } = false;
	public DateTime CreatedAt { get; set; }
	public DateTime? UpdatedAt { get; set; }
	public bool IsDeleted { get; set; }

	// Navigation Properties
	public User? User { get; set; }
	public TreeReport? TreeReport { get; set; }
}