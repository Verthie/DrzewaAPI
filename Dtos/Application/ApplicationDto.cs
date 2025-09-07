using DrzewaAPI.Models.Enums;

namespace DrzewaAPI.Dtos.Application;

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
