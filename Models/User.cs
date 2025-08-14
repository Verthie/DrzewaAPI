using System;
using Microsoft.AspNetCore.Identity;

namespace DrzewaAPI.Models;

public class User
{
	public required Guid Id { get; set; }
	public required string FirstName { get; set; }
	public required string LastName { get; set; }
	public required string Email { get; set; }
	public required string PasswordHash { get; set; }
	public string? Phone { get; set; }
	public DateTime CreatedAt { get; set; }
	public DateTime? LastLoginAt { get; set; }
	public bool IsActive { get; set; }

	// Navigation Properties
	public ICollection<TreeReport>? TreeReports { get; set; }
	public ICollection<Comment>? Comments { get; set; }
	public ICollection<Vote>? Votes { get; set; }
	public ICollection<Application>? Applications { get; set; }
}