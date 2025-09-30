using DrzewaAPI.Data;
using DrzewaAPI.Models;
using DrzewaAPI.Utils;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace DrzewaAPI.Services;

public class TreeService(
		ApplicationDbContext _context,
		ILogger<TreeService> _logger,
		IAzureStorageService _azureStorageService,
		IGeoportalService _geoportalService) : ITreeService
{
	public async Task<List<TreeSubmissionDto>> GetTreeSubmissionsAsync()
	{
		try
		{
			List<TreeSubmission> submissions = await _context.TreeSubmissions
				.Include(s => s.Species)
				.Include(s => s.TreeVotes)
				.Include(s => s.User)
				.Include(s => s.Comments)
				.ToListAsync();

			List<TreeSubmissionDto> result = submissions
				.Select(s => MapToTreeSubmissionDto(s))
				.ToList();

			return result;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Błąd podczas pobierania listy drzew");
			throw new ServiceException($"Nie można pobrać listy drzew", "TREE_FETCH_ERROR");
		}
	}

	public async Task<List<TreeSubmissionDto>> GetCurrentUserTreeSubmissionsAsync(Guid userId)
	{
		try
		{
			List<TreeSubmission> submissions = await _context.TreeSubmissions
					.Where(s => s.UserId == userId)
					.Include(s => s.Species)
					.Include(s => s.TreeVotes)
					.Include(s => s.User)
					.Include(s => s.Comments)
					.ToListAsync();

			List<TreeSubmissionDto> result = submissions
				.Select(MapToTreeSubmissionDto)
				.ToList();

			return result;
		}
		catch (SqlException ex) when (ex.Number == -2)
		{
			_logger.LogError(ex, "Timeout podczas pobierania drzew dla użytkownika {UserId}", userId);
			throw;
		}
		catch (SqlException ex) when (ex.Number == 2)
		{
			_logger.LogError(ex, "Błąd połączenia z bazą danych podczas pobierania drzew");
			throw new DatabaseConnectionException();
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Nieoczekiwany błąd podczas pobierania drzew dla użytkownika {UserId}", userId);
			throw new ServiceException($"Nie można pobrać listy drzew dla użytkownika {userId}", "TREE_FETCH_ERROR");
		}
	}

	public async Task<TreeSubmissionDto> GetTreeSubmissionByIdAsync(Guid treeId)
	{
		try
		{
			TreeSubmission submission = await _context.TreeSubmissions
				.Include(s => s.Species)
				.Include(s => s.TreeVotes)
				.Include(s => s.User)
				.Include(s => s.Comments)
				.FirstOrDefaultAsync(s => s.Id == treeId)
				?? throw EntityNotFoundException.ForTree(treeId);

			return MapToTreeSubmissionDto(submission);
		}
		catch (BusinessException)
		{
			throw;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Błąd podczas pobierania drzewa {TreeId}", treeId);
			throw new ServiceException($"Nie można pobrać drzewa {treeId}", "TREE_FETCH_ERROR");
		}
	}

	public async Task<TreeSubmissionDto> CreateTreeSubmissionAsync(CreateTreeSubmissionDto req, IFormFileCollection images, Guid userId)
	{
		try
		{
			bool speciesExists = await _context.TreeSpecies.AnyAsync(s => s.Id == req.SpeciesId);
			if (!speciesExists) throw EntityNotFoundException.ForSpecies(req.SpeciesId);

			await ValidateTreeSubmissionData(req);

			TreeSubmission submission = new TreeSubmission
			{
				Id = Guid.NewGuid(),
				UserId = userId,
				SpeciesId = req.SpeciesId,
				Location = new LocationDto
				{
					Lat = req.Location.Lat,
					Lng = req.Location.Lng,
					Address = req.Location.Address,
				},
				Circumference = req.Circumference,
				Height = req.Height,
				Condition = req.Condition,
				IsAlive = req.IsAlive,
				EstimatedAge = req.EstimatedAge,
				CrownSpread = req.CrownSpread,
				Description = req.Description,
				Images = new List<string>(),
				IsMonument = req.IsMonument,
			};

			// Get plot data from Geoportal API
			try
			{
				_logger.LogInformation($"Pobieranie danych działki dla zgłoszenia: {submission.Id}");

				Plot? plot = await _geoportalService.GetPlotByLocationAsync(
					submission.Location.Lng,
					submission.Location.Lat
				);

				if (plot != null)
				{
					submission.Location.PlotNumber = plot.PlotNumber;
					submission.Location.District = plot.District;
					submission.Location.Province = plot.Province;
					submission.Location.County = plot.County;
					submission.Location.Commune = plot.Commune;

					_logger.LogInformation("Dane działki uzupełnione");
					_logger.LogInformation($"Numer Działki: {plot.PlotNumber}");
					_logger.LogInformation($"Obręb Ewidencyjny: {plot.District}");
					_logger.LogInformation($"Województwo: {plot.Province}");
					_logger.LogInformation($"Powiat: {plot.County}");
					_logger.LogInformation($"Gmina: {plot.Commune}");
				}
				else
				{
					_logger.LogWarning("Nie udało się pobrać danych działki - zgłoszenie zostanie utworzone bez tych danych");
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Błąd podczas pobierania danych działki - kontynuowanie bez tych danych");
			}

			_context.TreeSubmissions.Add(submission);
			await _context.SaveChangesAsync();

			// Handle image uploads
			if (images != null && images.Count > 0)
			{
				try
				{
					string folderPath = $"uploads/tree-submissions/{submission.Id}";
					List<string> imagePaths = await _azureStorageService.SaveImagesAsync(images, folderPath);
					submission.Images = imagePaths;
					await _context.SaveChangesAsync();
				}
				catch (Exception ex)
				{
					_logger.LogError(ex, "Error saving images for tree submission {Id}", submission.Id);
					// TODO Notify the user that images couldn't be saved
				}
			}

			// Load navigation properties
			await _context.Entry(submission).Reference(s => s.User).LoadAsync();
			await _context.Entry(submission).Reference(s => s.Species).LoadAsync();
			await _context.Entry(submission).Collection(s => s.TreeVotes).LoadAsync();
			await _context.Entry(submission).Collection(s => s.Comments).LoadAsync();

			return MapToTreeSubmissionDto(submission);
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
			_logger.LogError(ex, "Nieoczekiwany błąd podczas tworzenia drzewa");
			throw EntityCreationFailedException.ForTree("Nieoczekiwany błąd systemu");
		}
	}

	public async Task<TreeSubmissionDto> UpdateTreeSubmissionAsync(Guid id, UpdateTreeSubmissionDto req, IFormFileCollection? images, Guid currentUserId, bool isModerator)
	{
		try
		{
			// Find existing submission
			TreeSubmission? submission = await _context.TreeSubmissions
					.Include(s => s.User)
					.Include(s => s.Species)
					.Include(s => s.TreeVotes)
					.Include(s => s.Comments)
					.FirstOrDefaultAsync(s => s.Id == id);

			if (submission == null)
				throw EntityNotFoundException.ForTree(id);

			// Check if user owns the submission or has admin rights
			if (submission.UserId != currentUserId && !isModerator)
				throw EntityAccessDeniedException.ForTree(id, currentUserId);

			// Validate species exists if being updated
			if (req.SpeciesId.HasValue)
			{
				bool speciesExists = await _context.TreeSpecies.AnyAsync(s => s.Id == req.SpeciesId.Value);
				if (!speciesExists) throw EntityNotFoundException.ForSpecies(req.SpeciesId.Value);
				submission.SpeciesId = req.SpeciesId.Value;
			}

			// Validate updated data
			await ValidateTreeSubmissionData(CreateTreeSubmissionDtoFromUpdate(req, submission));

			// Update properties if provided
			if (req.Location != null)
			{
				submission.Location = new LocationDto
				{
					Lat = req.Location.Lat,
					Lng = req.Location.Lng,
					Address = req.Location.Address,
				};
			}

			if (req.Circumference.HasValue)
				submission.Circumference = req.Circumference.Value;

			if (req.Height.HasValue)
				submission.Height = req.Height.Value;

			if (!string.IsNullOrEmpty(req.Condition))
				submission.Condition = req.Condition;

			if (req.IsAlive.HasValue)
				submission.IsAlive = req.IsAlive.Value;

			if (req.EstimatedAge.HasValue)
				submission.EstimatedAge = req.EstimatedAge.Value;

			if (req.Description != null) // Allow setting to empty string
				submission.Description = req.Description;

			if (req.IsMonument.HasValue)
				submission.IsMonument = req.IsMonument.Value;


			try
			{
				_logger.LogInformation($"Pobieranie danych działki dla zgłoszenia: {submission.Id}");

				var plot = await _geoportalService.GetPlotByLocationAsync(
					submission.Location.Lat,
					submission.Location.Lng
				);

				if (plot != null)
				{
					submission.Location.PlotNumber = plot.PlotNumber;

					_logger.LogInformation($"Dane działki uzupełnione: {plot.PlotNumber}");
				}
				else
				{
					_logger.LogWarning("Nie udało się pobrać danych działki - zgłoszenie zostanie utworzone bez tych danych");
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Błąd podczas pobierania danych działki - kontynuowanie bez tych danych");
			}

			await _context.SaveChangesAsync();

			// Handle image uploads if provided
			if (images != null && images.Count > 0)
			{
				try
				{
					string folderPath = $"uploads/tree-submissions/{submission.Id}";

					// Delete existing images if replace mode
					if (req.ReplaceImages == true && submission.Images?.Any() == true)
					{
						await _azureStorageService.DeleteImagesAsync(submission.Images);
						submission.Images = new List<string>();
					}

					// Upload new images
					List<string> imagePaths = await _azureStorageService.SaveImagesAsync(images, folderPath);

					if (req.ReplaceImages == true)
					{
						submission.Images = imagePaths;
					}
					else
					{
						// Append to existing images
						submission.Images ??= new List<string>();
						submission.Images.AddRange(imagePaths);
					}

					await _context.SaveChangesAsync();
				}
				catch (Exception ex)
				{
					_logger.LogError(ex, "Error updating images for tree submission {Id}", submission.Id);
					// TODO: Notify the user that images couldn't be updated
				}
			}

			return MapToTreeSubmissionDto(submission);
		}
		catch (BusinessException)
		{
			throw;
		}
		catch (DbUpdateException ex)
		{
			_logger.LogError(ex, "Błąd podczas aktualizacji drzewa w bazie danych");
			throw EntityUpdateFailedException.ForTree(id, "Błąd podczas aktualizacji w bazie danych");
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Nieoczekiwany błąd podczas aktualizacji drzewa");
			throw EntityUpdateFailedException.ForTree(id, "Nieoczekiwany błąd systemu");
		}
	}

	public async Task DeleteTreeSubmissionAsync(Guid treeId, Guid userId, bool isModerator)
	{
		try
		{
			TreeSubmission submission = await _context.TreeSubmissions
				.FirstOrDefaultAsync(s => s.Id == treeId)
				?? throw EntityNotFoundException.ForTree(treeId);

			// Check privileges
			if (!isModerator && submission.UserId != userId) throw EntityAccessDeniedException.ForTree(treeId, userId);

			// Delete associated images
			if (submission.Images?.Any() == true)
			{
				_azureStorageService.DeleteImages(submission.Images);
			}

			_context.TreeSubmissions.Remove(submission);
			await _context.SaveChangesAsync();
		}
		catch (BusinessException)
		{
			throw;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Błąd podczas usuwania drzewa {TreeId}", treeId);
			throw new ServiceException($"Nie można usunąć drzewa {treeId}", "TREE_DELETE_ERROR");
		}
	}

	public async Task ApproveTreeAsync(Guid treeId)
	{
		try
		{
			TreeSubmission submission = await _context.TreeSubmissions
				.FirstOrDefaultAsync(s => s.Id == treeId)
				?? throw EntityNotFoundException.ForTree(treeId);

			if (submission.ApprovalDate != null) throw TreeException.ForExistingApproval(treeId);

			submission.ApprovalDate = DateTime.UtcNow;

			await _context.SaveChangesAsync();
		}
		catch (BusinessException)
		{
			throw;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Błąd podczas zatwierdzania drzewa {TreeId}", treeId);
			throw TreeException.ForApprovalFailure(treeId, "Nieoczekiwany błąd systemu");
		}
	}

	public async Task<VotesDto> SetVoteAsync(Guid treeId, Guid userId, VoteType? type)
	{
		try
		{
			TreeSubmission submission = await _context.TreeSubmissions
				.FirstOrDefaultAsync(s => s.Id == treeId)
				?? throw EntityNotFoundException.ForTree(treeId);

			TreeVote? existing = await _context.TreeVotes
				.SingleOrDefaultAsync(v => v.TreeSubmissionId == treeId && v.UserId == userId);

			if (type == null)
			{
				if (existing == null) throw new ServiceException($"Nie znaleziono istniejącego głosu dla użytkownika o ID {userId} na drzewo o ID {treeId}", "VOTE_NOT_FOUND");

				// remove existing vote
				_context.TreeVotes.Remove(existing);
			}
			else
			{
				if (existing == null)
				{
					// add new vote
					_context.TreeVotes.Add(new TreeVote
					{
						Id = Guid.NewGuid(),
						TreeSubmissionId = submission.Id,
						UserId = userId,
						Type = type.Value,
					});
				}
				else if (existing.Type != type.Value)
				{
					existing.Type = type.Value; // change the vote from one type to another
				}
			}

			await _context.SaveChangesAsync();

			var counts = await _context.TreeVotes
				.Where(v => v.TreeSubmissionId == treeId)
				.GroupBy(_ => 1)
				.Select(g => new VotesDto
				{
					Like = g.Count(v => v.Type == VoteType.Like),
					Dislike = g.Count(v => v.Type == VoteType.Dislike)
				})
				.FirstOrDefaultAsync() ?? new VotesDto();

			return counts;
		}
		catch (BusinessException)
		{
			throw;
		}
		catch (DbUpdateException ex)
		{
			_logger.LogError(ex, "Błąd podczas wprowadzania głosu do bazy danych");
			throw EntityVoteException.ForTree(treeId, "Błąd podczas zapisu do bazy danych");
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Nieoczekiwany błąd podczas oddawania głosu");
			throw EntityVoteException.ForTree(treeId, "Nieoczekiwany błąd systemu");
		}
	}

	private TreeSubmissionDto MapToTreeSubmissionDto(TreeSubmission s)
	{
		return new TreeSubmissionDto
		{
			Id = s.Id,
			UserData = new UserDataDto
			{
				UserId = s.User.Id,
				UserName = s.User.FullName,
				Avatar = s.User.Avatar
			},
			Species = s.Species.PolishName,
			SpeciesLatin = s.Species.LatinName,
			Location = s.Location,
			Circumference = s.Circumference,
			Height = s.Height,
			Condition = s.Condition,
			IsAlive = s.IsAlive,
			EstimatedAge = s.EstimatedAge,
			Description = s.Description,
			ImageUrls = s.Images?.Select(path =>
						FileHelper.GetFileUrl(path, _azureStorageService)).ToList() ?? new List<string>(),
			IsMonument = s.IsMonument,
			Status = s.Status,
			SubmissionDate = s.SubmissionDate,
			ApprovalDate = s.ApprovalDate,
			Votes = new VotesDto
			{
				Like = s.TreeVotes.Count(v => v.Type == VoteType.Like),
				Dislike = s.TreeVotes.Count(v => v.Type == VoteType.Dislike)
			},
			CommentCount = s.Comments.Count
		};
	}

	private CreateTreeSubmissionDto CreateTreeSubmissionDtoFromUpdate(UpdateTreeSubmissionDto updateDto, TreeSubmission existing)
	{
		return new CreateTreeSubmissionDto
		{
			SpeciesId = updateDto.SpeciesId ?? existing.SpeciesId,
			Location = updateDto.Location ?? existing.Location,
			Circumference = updateDto.Circumference ?? existing.Circumference,
			Height = updateDto.Height ?? existing.Height,
			Condition = updateDto.Condition ?? existing.Condition,
			IsAlive = updateDto.IsAlive ?? existing.IsAlive,
			EstimatedAge = updateDto.EstimatedAge ?? existing.EstimatedAge,
			CrownSpread = updateDto.CrownSpread ?? existing.CrownSpread,
			Description = updateDto.Description ?? existing.Description,
			IsMonument = updateDto.IsMonument ?? existing.IsMonument
		};
	}


	private async Task ValidateTreeSubmissionData(CreateTreeSubmissionDto request)
	{
		var errors = new Dictionary<string, List<string>>();

		// Coordinates validation
		if (request.Location.Lat < -90 || request.Location.Lat > 90)
		{
			errors.GetValueOrDefault("Latitude", new List<string>()).Add("Szerokość geograficzna musi być między -90 a 90");
		}

		if (request.Location.Lng < -180 || request.Location.Lng > 180)
		{
			errors.GetValueOrDefault("Longitude", new List<string>()).Add("Długość geograficzna musi być między -180 a 180");
		}

		// Check if there are any existing tree submissions in 10m radius
		var nearbyTree = await _context.TreeSubmissions
				.AnyAsync(t => Math.Abs(t.Location.Lat - request.Location.Lat) < 0.0001
										&& Math.Abs(t.Location.Lng - request.Location.Lng) < 0.0001);

		if (nearbyTree)
		{
			errors.GetValueOrDefault("Location", new List<string>()).Add("W tym miejscu już istnieje zgłoszenie drzewa");
		}

		if (errors.Any())
		{
			var errorDict = errors.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ToArray());
			throw new ValidationException(errorDict);
		}
	}
}
