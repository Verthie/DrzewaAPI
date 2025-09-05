using System;
using DrzewaAPI.Dtos.Application;

namespace DrzewaAPI.Services;

public interface IApplicationService
{
	Task<List<ApplicationDto>> GetUserApplicationsAsync(Guid userId);
	Task<ApplicationDto?> GetApplicationByIdAsync(Guid id, Guid userId);
	Task<ApplicationDto> CreateApplicationAsync(Guid userId, CreateApplicationDto createDto);
	Task<ApplicationDto?> UpdateApplicationAsync(Guid id, Guid userId, UpdateApplicationDto updateDto);
	Task<bool> DeleteApplicationAsync(Guid id, Guid userId);
	Task<ApplicationFormSchemaDto?> GetApplicationFormSchemaAsync(Guid applicationId, Guid userId);
	Task<string> GeneratePdfFromAplicationAsync(Guid applicationId, Guid userId);
}
