namespace DrzewaAPI.Services;

public interface IMunicipalityService
{
	Task<List<MunicipalityDto>> GetAllMunicipalitiesAsync();
	Task<MunicipalityDto> GetMunicipalityByIdAsync(Guid id);
	Task<MunicipalityDto> CreateMunicipalityAsync(CreateMunicipalityDto createDto);
	Task<MunicipalityDto> UpdateMunicipalityAsync(Guid id, UpdateMunicipalityDto updateDto);
	Task DeleteMunicipalityAsync(Guid id);
}
