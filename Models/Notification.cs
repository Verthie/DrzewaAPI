using System;
using DrzewaAPI.Utils;

namespace DrzewaAPI.Models;

public class Notification
{
	public required Guid Id { get; set; }
	public required Guid UserId { get; set; }
	public NotificationType Type { get; set; }
	public required string Title { get; set; }
	public required string Message { get; set; }
	public string? DataJson { get; set; }
	public bool IsRead { get; set; }
	public DateTime CreatedAt { get; set; }

	// Navigation Properties
	public User? User { get; set; }
}
