namespace DrzewaAPI.Services;

public interface ICommuneService
{
	Task<List<CommuneDto>> GetAllCommunesAsync();
	Task<CommuneDto> GetCommuneByIdAsync(Guid id);
	Task<CommuneDto> CreateCommuneAsync(CreateCommuneDto createDto);
	Task<CommuneDto> UpdateCommuneAsync(Guid id, UpdateCommuneDto updateDto);
	Task DeleteCommuneAsync(Guid id);
}
