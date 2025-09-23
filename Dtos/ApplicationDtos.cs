using System;
using System.ComponentModel.DataAnnotations;
using DrzewaAPI.Models;

namespace DrzewaAPI.Dtos;

public record ApplicationDto
{
	public Guid Id { get; init; }
	public Guid ApplicationTemplateId { get; init; }
	public Dictionary<string, object> FormData { get; init; } = new();
	public ApplicationStatus Status { get; init; }
	public DateTime CreatedDate { get; init; }
	public DateTime? SubmittedDate { get; init; }
	public DateTime? ProcessedDate { get; init; }
}

public record ApplicationFormSchemaDto
{
	public Guid ApplicationId { get; set; }
	public Guid ApplicationTemplateId { get; set; }
	public string TemplateName { get; set; } = string.Empty;
	public List<ApplicationField> RequiredFields { get; set; } = new List<ApplicationField>();
	public Dictionary<string, object> PrefilledData { get; set; } = new Dictionary<string, object>();
}

public record ApplicationTemplateDto
{
	public Guid Id { get; init; }
	public Guid MunicipalityId { get; init; }
	public string Name { get; init; } = string.Empty;
	public string? Description { get; init; }
	public bool IsActive { get; init; }
	public DateTime CreatedDate { get; init; }
	public DateTime? LastModifiedDate { get; init; }
}

public record CreateApplicationDto
{
	[Required]
	public Guid TreeSubmissionId { get; init; }

	[Required]
	public Guid ApplicationTemplateId { get; init; }
	[Required]
	public required string Title { get; init; }
}

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

public record class SubmitApplicationDto
{
	[Required]
	public required Dictionary<string, object> FormData { get; set; }
}

public record UpdateApplicationDto
{
	public Dictionary<string, object>? FormData { get; set; }
}

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
