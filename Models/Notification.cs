using System;
using DrzewaAPI.Models.Enums;

namespace DrzewaAPI.Models;

public class Notification
{
	public Guid Id { get; set; }
	public Guid UserId { get; set; }
	public NotificationType Type { get; set; }
	public required string Title { get; set; }
	public required string Message { get; set; }
	public string? DataJson { get; set; }
	public bool IsRead { get; set; }
	public DateTime CreatedAt { get; set; }

	// Navigation Properties
	public User? User { get; set; }
}
