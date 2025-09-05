using System.ComponentModel.DataAnnotations;

namespace DrzewaAPI.Dtos.Application;

public record UpdateApplicationDto
{
	public Dictionary<string, object>? FormData { get; set; }
}
