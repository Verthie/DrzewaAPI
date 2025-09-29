namespace DrzewaAPI.Services;

public interface ISpeciesService
{
	public Task<List<TreeSpeciesDto>> GetAllSpeciesAsync();
	public Task<TreeSpeciesDto> GetSpeciesByIdAsync(Guid speciesId);
	public Task<TreeSpeciesDto> CreateTreeSpeciesAsync(CreateTreeSpeciesDto req, IFormFile treeImage, IFormFile leafImage, IFormFile barkImage, IFormFile fruitImage);
	public Task DeleteSpeciesAsync(Guid speciesId);
}
