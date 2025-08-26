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

	// Helper Properties
	[NotMapped]
	public string FullName => $"{FirstName} {LastName}";

	// Navigation Properties
	public ICollection<TreeSubmission> TreeSubmissions { get; set; } = new List<TreeSubmission>();
	public ICollection<Vote> Votes { get; set; } = new List<Vote>();
	public ICollection<Comment> Comments { get; set; } = new List<Comment>();
	public ICollection<Like> Likes { get; set; } = new List<Like>();
}