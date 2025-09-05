using DrzewaAPI.Models;

namespace DrzewaAPI.Dtos.Application;

public record ApplicationFormSchemaDto
{
	public Guid ApplicationId { get; set; }
	public Guid ApplicationTemplateId { get; set; }
	public string TemplateName { get; set; } = string.Empty;
	public List<ApplicationField> RequiredFields { get; set; } = new List<ApplicationField>();
	public Dictionary<string, object> PrefilledData { get; set; } = new Dictionary<string, object>();
}
