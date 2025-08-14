using System;

namespace DrzewaAPI.Models;

public class Vote
{
	public required Guid Id { get; set; }
	public required Guid TreeReportId { get; set; }
	public required Guid UserId { get; set; }
	public DateTime CreatedAt { get; set; }

	// Navigation Properties
	public TreeReport? TreeReport { get; set; }
	public User? User { get; set; }
}
