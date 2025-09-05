using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DrzewaAPI.Models.Enums;
using DrzewaAPI.Utils;

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
	[MaxLength(150)]
	public string? Address { get; set; }
	[MaxLength(50)]
	public string? City { get; set; }
	[MaxLength(50)]
	public string? PostalCode { get; set; }
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
	public ICollection<TreeVote> TreeVotes { get; set; } = new List<TreeVote>();
	public ICollection<CommentVote> CommentVotes { get; set; } = new List<CommentVote>();
	public ICollection<Comment> Comments { get; set; } = new List<Comment>();
	public ICollection<Application> Applications { get; set; } = new List<Application>();
}