using System.ComponentModel.DataAnnotations;
using DrzewaAPI.Models;

namespace DrzewaAPI.Dtos.Application;

public record CreateApplicationTemplateDto
{
	[Required]
	public Guid MunicipalityId { get; set; }

	[Required]
	[MaxLength(100)]
	public required string Name { get; set; }

	[MaxLength(500)]
	public string? Description { get; set; }

	[Required]
	public required string HtmlTemplate { get; set; }

	[Required]
	public required List<ApplicationField> Fields { get; set; } = new List<ApplicationField>();
}
