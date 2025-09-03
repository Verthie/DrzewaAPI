using System;
using DrzewaAPI.Models;
using DrzewaAPI.Models.Enums;

namespace DrzewaAPI.Utils;

public class Vote
{
	public Guid Id { get; set; }
	public Guid UserId { get; set; }
	public required VoteType Type { get; set; }
	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
	public DateTime? UpdatedAt { get; set; }

	// Navigation Properties
	public User User { get; set; } = default!;
}
