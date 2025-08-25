using System;
using DrzewaAPI.Data;
using DrzewaAPI.Dtos.Species;
using DrzewaAPI.Models;
using DrzewaAPI.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DrzewaAPI.Services;

public class SpeciesService(ApplicationDbContext _dbContext, ILogger<SpeciesService> _logger) : ISpeciesService
{
	public async Task<List<TreeSpeciesDto>> GetAllSpeciesAsync()
	{
		try
		{
			List<TreeSpeciesDto> species = await _dbContext.TreeSpecies.Include(s => s.Images).Select(s => MapToTreeSpeciesDto(s)).ToListAsync();

			return species;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Błąd podczas pobierania listy gatunków");
			throw;
		}
	}

	public async Task<TreeSpeciesDto?> GetSpeciesByIdAsync(Guid speciesId)
	{
		try
		{
			TreeSpecies? species = await _dbContext.TreeSpecies.Include(s => s.Images).FirstOrDefaultAsync(s => s.Id == speciesId);

			if (species == null) return null;

			return MapToTreeSpeciesDto(species);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Błąd podczas pobierania gatunku o ID: {SpeciesId}", speciesId);
			throw;
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
