using System;
using System.ComponentModel.DataAnnotations;
using DrzewaAPI.Models;

namespace DrzewaAPI.Dtos;

public record ApplicationTemplateDto
{
	public Guid Id { get; init; }
	public Guid CommuneId { get; init; }
	public string Name { get; init; } = string.Empty;
	public string? Description { get; init; }
	public bool IsActive { get; init; }
	public DateTime CreatedDate { get; init; }
	public DateTime? LastModifiedDate { get; init; }
}

public record CreateApplicationTemplateDto
{
	[Required]
	public Guid CommuneId { get; set; }

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

public record UpdateApplicationTemplateDto
{
	public Guid CommuneId { get; init; }

	[MaxLength(100)]
	public required string Name { get; init; }

	[MaxLength(500)]
	public required string Description { get; init; }

	public required string HtmlTemplate { get; init; }

	public required List<ApplicationField> Fields { get; init; } = new List<ApplicationField>();

	public bool IsActive { get; init; }
}

public record SignatureDto
{
	public float Width { get; init; }
	public float Height { get; init; }
	public float X { get; init; }
	public float Y { get; init; }
}