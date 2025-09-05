using System;
using DrzewaAPI.Dtos.Application;
using DrzewaAPI.Dtos.Municipality;

namespace DrzewaAPI.Services;

public interface IMunicipalityService
{
	Task<List<MunicipalityDto>> GetAllMunicipalitiesAsync();
	Task<MunicipalityDto?> GetMunicipalityByIdAsync(Guid id);
	Task<MunicipalityDto> CreateMunicipalityAsync(CreateMunicipalityDto createDto);
	Task<MunicipalityDto?> UpdateMunicipalityAsync(Guid id, UpdateMunicipalityDto updateDto);
	Task<bool> DeleteMunicipalityAsync(Guid id);
	Task<List<ApplicationTemplateDto>> GetMunicipalityTemplatesAsync(Guid municipalityId);
	Task<List<ApplicationDto>> GetMunicipalityApplicationsAsync(Guid municipalityId);
}
