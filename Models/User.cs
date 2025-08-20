using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
	public string PasswordHash { get; set; } = "";
	[Phone]
	[MaxLength(15)]
	public string? Phone { get; set; }
	public string? Avatar { get; set; }
	public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;
	public UserRole Role { get; set; } = UserRole.User;

	// public DateTime? LastLoginAt { get; set; }
	// public bool IsActive { get; set; }

	// Computed Properties (for statistics)
	[NotMapped]
	public int SubmissionsCount => TreeSubmissions?.Count ?? 0;
	[NotMapped]
	public int VerificationsCount => Votes?.Count(v => v.Type == VoteType.Approve) ?? 0;
	[NotMapped]
	public int MonumentCount => TreeSubmissions?.Count(s => s.Status == SubmissionStatus.Monument) ?? 0;

	// Helper Properties
	public string FullName => $"{FirstName} {LastName}";

	// Navigation Properties
	public ICollection<TreeSubmission> TreeSubmissions { get; set; } = new List<TreeSubmission>();
	public ICollection<Vote> Votes { get; set; } = new List<Vote>();
}