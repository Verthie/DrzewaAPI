using System.ComponentModel.DataAnnotations;

namespace DrzewaAPI.Dtos.Application;

public record CreateApplicationDto
{
	[Required]
	public Guid TreeSubmissionId { get; init; }

	[Required]
	public Guid ApplicationTemplateId { get; init; }
	[Required]
	public required string Title { get; init; }
}
