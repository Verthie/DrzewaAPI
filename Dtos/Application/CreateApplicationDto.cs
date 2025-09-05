using System.ComponentModel.DataAnnotations;

namespace DrzewaAPI.Dtos.Application;

public record CreateApplicationDto
{
	[Required]
	public Guid TreeSubmissionId { get; set; }

	[Required]
	public Guid ApplicationTemplateId { get; set; }
}
