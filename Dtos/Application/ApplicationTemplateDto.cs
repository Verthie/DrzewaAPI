using DrzewaAPI.Models;

namespace DrzewaAPI.Dtos.Application;

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
