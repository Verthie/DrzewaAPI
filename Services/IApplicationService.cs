namespace DrzewaAPI.Services;

public interface IApplicationService
{
	Task<List<ApplicationDto>> GetUserApplicationsAsync(Guid userId);
	Task<ApplicationDto> GetApplicationByIdAsync(Guid id, Guid userId);
	Task<ApplicationDto> CreateApplicationAsync(Guid userId, CreateApplicationDto createDto);
	Task<ApplicationDto> UpdateApplicationAsync(Guid id, Guid userId, UpdateApplicationDto updateDto);
	Task DeleteApplicationAsync(Guid id, Guid userId, bool isModerator);
	Task<ApplicationFormSchemaDto> GetApplicationFormSchemaAsync(Guid applicationId, Guid userId);
	Task<ApplicationDto> SubmitApplicationAsync(Guid id, Guid userId, SubmitApplicationDto submitDto);
	Task<string> GeneratePdfFromAplicationAsync(Guid applicationId, Guid userId);
}
