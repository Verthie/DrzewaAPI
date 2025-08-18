using System;
using System.ComponentModel.DataAnnotations;
using DrzewaAPI.Models.Enums;

namespace DrzewaAPI.Models;

public class User
{
	public Guid Id { get; set; }
	[Required]
	[MaxLength(50)]
	public required string FirstName { get; set; }
	[Required]
	[MaxLength(50)]
	public required string LastName { get; set; }
	[Required]
	[EmailAddress]
	[MaxLength(100)]
	public required string Email { get; set; }
	[Required]
	public required string PasswordHash { get; set; }
	[Phone]
	[MaxLength(15)]
	public string? Phone { get; set; }
	public string? Avatar { get; set; }
	public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;
	public UserRole Role { get; set; } = UserRole.User;

	// public DateTime? LastLoginAt { get; set; }
	// public bool IsActive { get; set; }

	// Computed Properties (for statistics)
	public int SubmissionsCount { get; set; } = 0;
	public int VerificationsCount { get; set; } = 0;

	// Navigation Properties
	public ICollection<TreeReport> TreeReports { get; set; } = new List<TreeReport>();
	public ICollection<Comment> Comments { get; set; } = new List<Comment>();
	public ICollection<Vote> Votes { get; set; } = new List<Vote>();
	public ICollection<Application> Applications { get; set; } = new List<Application>();

	// Helper Properties
	public string FullName => $"{FirstName} {LastName}";
}