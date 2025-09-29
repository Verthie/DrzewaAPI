namespace DrzewaAPI.Services;

public interface IApplicationTemplateService
{
	Task<List<ApplicationTemplateDto>> GetAllTemplatesAsync();
	Task<List<ApplicationTemplateDto>> GetActiveTemplatesAsync();
	Task<ApplicationTemplateDto> GetTemplateByIdAsync(Guid id);
	Task<List<ApplicationTemplateDto>> GetTemplatesByCommuneIdAsync(Guid CommuneId);
	Task<ApplicationTemplateDto> CreateTemplateAsync(CreateApplicationTemplateDto createDto);
	Task<ApplicationTemplateDto> UpdateTemplateAsync(Guid id, UpdateApplicationTemplateDto updateDto);
	Task DeleteTemplateAsync(Guid id);
}
