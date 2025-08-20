using DrzewaAPI.Data;
using DrzewaAPI.Dtos.TreeSubmissions;
using DrzewaAPI.Models;
using DrzewaAPI.Models.Enums;
using DrzewaAPI.Models.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace DrzewaAPI.Services;

public class TreeService(ApplicationDbContext _dbContext, ILogger<TreeService> _logger) : ITreeService
{
	public async Task<List<TreeSubmissionDto>> GetTreeSubmissionsAsync()
	{
		try
		{
			List<TreeSubmissionDto> submissions = await _dbContext.TreeSubmissions
				.Include(s => s.Species)
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

	private static TreeSubmissionDto MapToTreeSubmissionDto(TreeSubmission submission)
	{
		return new TreeSubmissionDto
		{
			Id = submission.Id,
			Species = submission.Species.PolishName ?? "Unknown species",
			SpeciesLatin = submission.Species.LatinName ?? "Unknown species",
			Location = submission.Location,
			Circumference = submission.Circumference,
			Height = submission.Height,
			Condition = submission.Condition,
			IsAlive = submission.IsAlive,
			EstimatedAge = submission.EstimatedAge,
			Description = submission.Description,
			Images = submission.Images,
			IsMonument = submission.IsMonument,
			Status = submission.Status,
			SubmissionDate = submission.SubmissionDate,
			ApprovalDate = submission.ApprovalDate,
			Votes = new VotesCount
			{
				Approve = submission.ApprovalVotes,
				Reject = submission.RejectionVotes
			},
		};
	}
}
