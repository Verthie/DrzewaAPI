using System.ComponentModel.DataAnnotations;
using DrzewaAPI.Models;

namespace DrzewaAPI.Dtos.Application;

public record UpdateApplicationTemplateDto
{
	public Guid MunicipalityId { get; init; }

	[MaxLength(100)]
	public required string Name { get; init; }

	[MaxLength(500)]
	public required string Description { get; init; }

	public required string HtmlTemplate { get; init; }

	public required List<ApplicationField> Fields { get; init; } = new List<ApplicationField>();

	public bool IsActive { get; init; }
}
