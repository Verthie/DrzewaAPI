namespace DrzewaAPI.Services;

public interface ISpeciesService
{
	public Task<List<TreeSpeciesDto>> GetAllSpeciesAsync();
	public Task<TreeSpeciesDto> GetSpeciesByIdAsync(Guid speciesId);
}
