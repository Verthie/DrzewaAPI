using DrzewaAPI.Data;
using DrzewaAPI.Extensions;
using DrzewaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DrzewaAPI.Services;

public class SpeciesService(ApplicationDbContext _context, ILogger<SpeciesService> _logger) : ISpeciesService
{
	public async Task<List<TreeSpeciesDto>> GetAllSpeciesAsync()
	{
		try
		{
			List<TreeSpeciesDto> species = await _context.TreeSpecies.Include(s => s.Images).Select(s => MapToTreeSpeciesDto(s)).ToListAsync();

			return species;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Błąd podczas pobierania listy gatunków");
			throw new ServiceException($"Nie można pobrać listy gatunków", "SPECIES_FETCH_ERROR");
		}
	}

	public async Task<TreeSpeciesDto> GetSpeciesByIdAsync(Guid speciesId)
	{
		try
		{
			TreeSpecies species = await _context.TreeSpecies
				.Include(s => s.Images)
				.FirstOrDefaultAsync(s => s.Id == speciesId)
				?? throw EntityNotFoundException.ForSpecies(speciesId);

			return MapToTreeSpeciesDto(species);
		}
		catch (BusinessException)
		{
			throw;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Błąd podczas pobierania gatunku o ID: {SpeciesId}", speciesId);
			throw new ServiceException($"Nie można pobrać użytkownika {speciesId}", "SPECIES_FETCH_ERROR");
		}
	}

	private static TreeSpeciesDto MapToTreeSpeciesDto(TreeSpecies s)
	{
		var images = s.Images.Select(i => new TreeSpeciesImageDto
		{
			ImageUrl = i.ImageUrl,
			Type = i.Type,
			AltText = i.AltText ?? i.Type.GenerateAltText(s.PolishName)
		}).ToList();

		return new TreeSpeciesDto
		{
			Id = s.Id,
			PolishName = s.PolishName,
			LatinName = s.LatinName,
			Family = s.Family,
			Description = s.Description,
			IdentificationGuide = s.IdentificationGuide,
			SeasonalChanges = s.SeasonalChanges,
			Images = images,
			Traits = s.Traits
		};
	}
}
