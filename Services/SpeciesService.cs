using DrzewaAPI.Data;
using DrzewaAPI.Extensions;
using DrzewaAPI.Models;
using DrzewaAPI.Utils;
using Microsoft.EntityFrameworkCore;

namespace DrzewaAPI.Services;

public class SpeciesService(
		ApplicationDbContext _context,
		ILogger<SpeciesService> _logger,
		IAzureStorageService _azureStorageService) : ISpeciesService
{

	public async Task<List<TreeSpeciesDto>> GetAllSpeciesAsync()
	{
		try
		{
			List<TreeSpecies> species = await _context.TreeSpecies.ToListAsync();

			List<TreeSpeciesDto> result = species
				.Select(MapToTreeSpeciesDto)
				.ToList();

			return result;
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

	public async Task<TreeSpeciesDto> CreateTreeSpeciesAsync(CreateTreeSpeciesDto req, IFormFile treeImage, IFormFile leafImage, IFormFile barkImage, IFormFile fruitImage)
	{
		try
		{
			TreeSpecies species = new TreeSpecies
			{
				Id = Guid.NewGuid(),
				PolishName = req.PolishName,
				LatinName = req.LatinName,
				Family = req.Family,
				Description = req.Description,
				IdentificationGuide = req.IdentificationGuide,
				SeasonalChanges = req.SeasonalChanges,
				Traits = req.Traits,
			};

			FormFileCollection images = new FormFileCollection { treeImage, leafImage, barkImage, fruitImage };

			// Handle image uploads
			if (images != null && images.Count == 4)
			{
				try
				{
					string folderPath = $"uploads/tree-species/{species.Id}";
					List<string> imagePaths = await _azureStorageService.SaveImagesAsync(images, folderPath);
					species.Images = new List<TreeSpeciesImageDto>
					{
							new(){
								ImageUrl = FileHelper.GetFileUrl(imagePaths[0], _azureStorageService),
								Type = ImageType.Tree,
							},
							new(){
								ImageUrl = FileHelper.GetFileUrl(imagePaths[1], _azureStorageService),
								Type = ImageType.Leaf,
							},
							new(){
								ImageUrl = FileHelper.GetFileUrl(imagePaths[2], _azureStorageService),
								Type = ImageType.Bark,
							},
							new(){
								ImageUrl = FileHelper.GetFileUrl(imagePaths[3], _azureStorageService),
								Type = ImageType.Fruit,
							}
					};
				}
				catch (Exception ex)
				{
					_logger.LogError(ex, "Error saving images for tree submission {Id}", species.Id);
					// TODO Notify the user that images couldn't be saved
				}
			}
			else
			{
				throw new ServiceException("All images need to be provided", "MISSING_IMAGES_ERROR");
			}

			_context.TreeSpecies.Add(species);
			await _context.SaveChangesAsync();

			return MapToTreeSpeciesDto(species);
		}
		catch (BusinessException)
		{
			throw;
		}
		catch (DbUpdateException ex)
		{
			_logger.LogError(ex, "Błąd podczas zapisu drzewa do bazy danych");
			throw EntityCreationFailedException.ForTree("Błąd podczas zapisu do bazy danych");
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Nieoczekiwany błąd podczas tworzenia gatunku");
			throw EntityCreationFailedException.ForTree("Nieoczekiwany błąd systemu");
		}
	}

	public async Task DeleteSpeciesAsync(Guid speciesId)
	{
		try
		{
			TreeSpecies species = await _context.TreeSpecies
					.FirstOrDefaultAsync(s => s.Id == speciesId)
					?? throw EntityNotFoundException.ForSpecies(speciesId);

			_context.TreeSpecies.Remove(species);
			await _context.SaveChangesAsync();
		}
		catch (BusinessException)
		{
			throw;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Nieoczekiwany błąd podczas usuwania gatunku");
			throw new ServiceException("Nieoczekiwany błąd podczas ususwania gatunku", "SPECIES_DELETE_ERROR");
		}
	}

	private TreeSpeciesDto MapToTreeSpeciesDto(TreeSpecies s)
	{
		var images = s.Images!.Select(i => new TreeSpeciesImageDto
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
