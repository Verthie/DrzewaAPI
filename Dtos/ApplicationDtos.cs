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

public record CreateApplicationDto
{
	[Required]
	public Guid TreeSubmissionId { get; init; }

	[Required]
	public Guid ApplicationTemplateId { get; init; }
	[Required]
	public required string Title { get; init; }
}

public record class SubmitApplicationDto
{
	[Required]
	public required Dictionary<string, object> FormData { get; init; }
}

public record UpdateApplicationDto
{
	public Dictionary<string, object>? FormData { get; init; }
}

public record ApplicationFileUrlsDto
{
	public string? PdfPath { get; init; }
	public List<string>? Images { get; init; }
	public string? TreeScreenshotUrl { get; init; }
}