using System;
using DrzewaAPI.Dtos.Application;

namespace DrzewaAPI.Services;

public interface IApplicationTemplateService
{
	Task<List<ApplicationTemplateDto>> GetAllTemplatesAsync();
	Task<List<ApplicationTemplateDto>> GetActiveTemplatesAsync();
	Task<ApplicationTemplateDto?> GetTemplateByIdAsync(Guid id);
	Task<List<ShortApplicationTemplateDto>> GetTemplatesByMunicipalityIdAsync(Guid municipalityId);
	Task<ApplicationTemplateDto> CreateTemplateAsync(CreateApplicationTemplateDto createDto);
	Task<ApplicationTemplateDto?> UpdateTemplateAsync(Guid id, UpdateApplicationTemplateDto updateDto);
	Task<bool> DeleteTemplateAsync(Guid id);
}
