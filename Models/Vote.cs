using System;
using DrzewaAPI.Models.Enums;

namespace DrzewaAPI.Models;

public class Vote
{
	public Guid Id { get; set; }
	public Guid TreeSubmissionId { get; set; }
	public Guid UserId { get; set; }
	public required VoteType Type { get; set; }
	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
	public DateTime? UpdatedAt { get; set; }

	// Navigation Properties
	public TreeSubmission TreeSubmission { get; set; } = default!;
	public User User { get; set; } = default!;
}
