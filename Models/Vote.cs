using System;

namespace DrzewaAPI.Models;

public class Vote
{
	public Guid Id { get; set; }
	public Guid TreeReportId { get; set; }
	public Guid UserId { get; set; }
	public DateTime CreatedAt { get; set; }

	// Navigation Properties
	public TreeReport? TreeReport { get; set; }
	public User? User { get; set; }
}
