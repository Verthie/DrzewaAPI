using System;

namespace DrzewaAPI.Models;

public class EmailVerificationToken
{
	public Guid Id { get; set; }
	public Guid UserId { get; set; }
	public string Token { get; set; } = string.Empty;
	public EmailTokenType TokenType { get; set; }
	public DateTime CreatedAt { get; set; }
	public DateTime ExpiresAt { get; set; }
	public bool IsUsed { get; set; }
	public DateTime? UsedAt { get; set; }
	public string? IpAddress { get; set; }
	public string? UserAgent { get; set; }

	// Navigation Properties
	public User User { get; set; } = default!;
}
