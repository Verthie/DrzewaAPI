using System.ComponentModel.DataAnnotations;

namespace DrzewaAPI.Models;

public class ApplicationTemplate
{
	public Guid Id { get; set; }
	public Guid MunicipalityId { get; set; }

	[Required]
	[MaxLength(100)]
	public required string Name { get; set; }

	[MaxLength(500)]
	public string? Description { get; set; }

	[Required]
	public required string HtmlTemplate { get; set; }

	public required List<ApplicationField> Fields { get; set; } = new List<ApplicationField>();
	public bool IsActive { get; set; } = true;
	public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
	public DateTime? LastModifiedDate { get; set; }

	// Navigation Properties
	public Municipality Municipality { get; set; } = default!;
	public ICollection<Application> Applications { get; set; } = new List<Application>();
}
