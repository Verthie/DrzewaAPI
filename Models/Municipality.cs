using System.ComponentModel.DataAnnotations;

namespace DrzewaAPI.Models;

public class Municipality
{
	public Guid Id { get; set; }

	[Required]
	[MaxLength(100)]
	public required string Name { get; set; }

	[Required]
	[MaxLength(200)]
	public required string Address { get; set; }

	[Required]
	[MaxLength(100)]
	public required string City { get; set; }

	[Required]
	[MaxLength(100)]
	public required string Province { get; set; } // Województwo

	[Required]
	[MaxLength(10)]
	public required string PostalCode { get; set; }

	[Required]
	[Phone]
	[MaxLength(20)]
	public required string Phone { get; set; }

	[Required]
	[EmailAddress]
	[MaxLength(100)]
	public required string Email { get; set; }

	[MaxLength(100)]
	public string? Website { get; set; }

	public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
	public DateTime? LastModifiedDate { get; set; }

	// Navigation Properties
	public ICollection<ApplicationTemplate> ApplicationTemplates { get; set; } = new List<ApplicationTemplate>();
	public ICollection<Application> Applications { get; set; } = new List<Application>();
}
