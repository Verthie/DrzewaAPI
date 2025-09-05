using System.ComponentModel.DataAnnotations;

namespace DrzewaAPI.Dtos.Application;

public record class SubmitApplicationDto
{
	[Required]
	public required Dictionary<string, object> FormData { get; set; }
}
