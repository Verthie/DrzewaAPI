using DrzewaAPI.Data;
using DrzewaAPI.Dtos.TreeSubmissions;
using DrzewaAPI.Models;
using DrzewaAPI.Models.Enums;
using DrzewaAPI.Models.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace DrzewaAPI.Services;

public class TreeService(ApplicationDbContext _context, ILogger<TreeService> _logger) : ITreeService
{
	public async Task<List<TreeSubmissionDto>> GetTreeSubmissionsAsync()
	{
		try
		{
			List<TreeSubmissionDto> submissions = await _context.TreeSubmissions
				.Include(s => s.Species)
				.Include(s => s.Votes)
				.Select(s => MapToTreeSubmissionDto(s))
				.ToListAsync();

			return submissions;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Błąd podczas pobierania listy drzew");
			throw;
		}
	}

	public async Task<TreeSubmissionDto?> GetTreeSubmissionByIdAsync(Guid treeId)
	{
		try
		{
			TreeSubmission? submission = await _context.TreeSubmissions
				.Include(s => s.Species)
				.Include(s => s.Votes)
				.FirstOrDefaultAsync(s => s.Id == treeId);

			ArgumentNullException.ThrowIfNull(submission);

			return MapToTreeSubmissionDto(submission);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Błąd podczas pobierania drzewa o ID: {TreeId}", treeId);
			throw;
		}
	}

	public async Task<TreeSubmissionDto?> CreateTreeSubmissionAsync(CreateTreeSubmissionDto req, Guid userId)
	{
		using var tx = await _context.Database.BeginTransactionAsync();

		TreeSubmission submission = new TreeSubmission
		{
			Id = Guid.NewGuid(),
			UserId = userId,
			SpeciesId = req.SpeciesId,
			Location = new Location
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
			Description = req.Description,
			Images = req.Images,
			IsMonument = req.IsMonument,
		};

		_context.TreeSubmissions.Add(submission);
		await _context.SaveChangesAsync();

		// Load navigation properties
		await _context.Entry(submission).Reference(s => s.Species).LoadAsync();
		await _context.Entry(submission).Collection(s => s.Votes).LoadAsync();

		await tx.CommitAsync();

		return MapToTreeSubmissionDto(submission);
	}

	private static TreeSubmissionDto MapToTreeSubmissionDto(TreeSubmission s)
	{
		return new TreeSubmissionDto
		{
			Id = s.Id,
			Species = s.Species.PolishName,
			SpeciesLatin = s.Species.LatinName,
			Location = s.Location,
			Circumference = s.Circumference,
			Height = s.Height,
			Condition = s.Condition,
			IsAlive = s.IsAlive,
			EstimatedAge = s.EstimatedAge,
			Description = s.Description,
			Images = s.Images,
			IsMonument = s.IsMonument,
			Status = s.Status,
			SubmissionDate = s.SubmissionDate,
			ApprovalDate = s.ApprovalDate,
			Votes = new VotesCount
			{
				Approve = s.Votes.Count(v => v.Type == VoteType.Approve),
				Reject = s.Votes.Count(v => v.Type == VoteType.Reject)
			},
		};
	}
}
