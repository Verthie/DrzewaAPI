using System;

namespace DrzewaAPI.Models;

public class Vote
{
	public Guid Id { get; set; }
	public Guid TreeReportId { get; set; }
	public Guid UserId { get; set; }
	public DateTime CreatedAt { get; set; }

	// Navigation Properties
	public required TreeReport TreeReport { get; set; }
	public required User User { get; set; }
}
