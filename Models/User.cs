using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DrzewaAPI.Models;

public class User
{
	public Guid Id { get; set; }
	[Required]
	[MaxLength(100)]
	public required string FirstName { get; set; }
	[Required]
	[MaxLength(100)]
	public required string LastName { get; set; }
	[Required]
	[EmailAddress]
	[MaxLength(255)]
	public required string Email { get; set; }
	[Required]
	public string PasswordHash { get; set; } = "";
	[Phone]
	[StringLength(20, MinimumLength = 9)]
	public string? Phone { get; set; }
	[Length(1, 150)]
	public string? Address { get; set; }
	[Length(1, 100)]
	public string? City { get; set; }
	[MaxLength(10)]
	public string? PostalCode { get; set; }
	public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;
	public UserRole Role { get; set; } = UserRole.User;
	public OrganizationDto? Organization { get; set; }
	public bool IsEmailVerified { get; set; } = false;
	public DateTime? EmailVerifiedAt { get; set; }

	// public DateTime? LastLoginAt { get; set; }
	// public bool IsActive { get; set; }

	// Helper Properties
	[NotMapped]
	public string FullName => $"{FirstName} {LastName}";

	// Navigation Properties
	public ICollection<TreeSubmission> TreeSubmissions { get; set; } = new List<TreeSubmission>();
	public ICollection<TreeVote> TreeVotes { get; set; } = new List<TreeVote>();
	public ICollection<Application> Applications { get; set; } = new List<Application>();
	public ICollection<EmailVerificationToken> EmailVerificationTokens { get; set; } = new List<EmailVerificationToken>();
}