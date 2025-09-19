using System;

namespace DrzewaAPI.Models;

public class RefreshToken
{
	public int Id { get; set; }
	public string Token { get; set; } = string.Empty;
	public Guid UserId { get; set; }
	public DateTime ExpiresAt { get; set; }
	public bool IsRevoked { get; set; } = false;
}
