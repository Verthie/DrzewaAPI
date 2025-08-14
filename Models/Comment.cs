using System;

namespace DrzewaAPI.Models;

public class Comment
{
	public Guid Id { get; set; }
	public Guid TreeReportId { get; set; }
	public Guid UserId { get; set; }
	public required string Content { get; set; }
	public bool IsLegend { get; set; } = false;
	public DateTime CreatedAt { get; set; }
	public DateTime? UpdatedAt { get; set; }
	public bool IsDeleted { get; set; }

	// Navigation Properties
	public required TreeReport TreeReport { get; set; }
	public required User User { get; set; }
}