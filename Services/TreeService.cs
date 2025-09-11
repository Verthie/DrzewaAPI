using DrzewaAPI.Data;
using DrzewaAPI.Dtos.TreeSubmissions;
using DrzewaAPI.Models;
using DrzewaAPI.Models.Enums;
using DrzewaAPI.Models.ValueObjects;
using DrzewaAPI.Utils;
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
				.Include(s => s.TreeVotes)
				.Include(s => s.User)
				.Include(s => s.Comments)
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

	public async Task<List<TreeSubmissionDto>> GetCurrentUserTreeSubmissionsAsync(Guid userId)
	{
		try
		{
			List<TreeSubmissionDto> submissions = await _context.TreeSubmissions
				.Where(s => s.UserId == userId)
				.Include(s => s.Species)
				.Include(s => s.TreeVotes)
				.Include(s => s.User)
				.Include(s => s.Comments)
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
				.Include(s => s.TreeVotes)
				.Include(s => s.User)
				.Include(s => s.Comments)
				.FirstOrDefaultAsync(s => s.Id == treeId);

			if (submission == null) return null;

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
		try
		{
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
			await _context.Entry(submission).Reference(s => s.User).LoadAsync();
			await _context.Entry(submission).Reference(s => s.Species).LoadAsync();
			await _context.Entry(submission).Collection(s => s.TreeVotes).LoadAsync();
			await _context.Entry(submission).Collection(s => s.Comments).LoadAsync();

			return MapToTreeSubmissionDto(submission);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Błąd podczas tworzenia drzewa");
			throw;
		}
	}

	public async Task<VotesCount?> SetVoteAsync(Guid treeId, Guid userId, VoteType? type)
	{
		try
		{
			TreeSubmission? submission = await _context.TreeSubmissions.FirstOrDefaultAsync(s => s.Id == treeId);

			if (submission == null) return null;

			TreeVote? existing = await _context.TreeVotes
				.SingleOrDefaultAsync(v => v.TreeSubmissionId == treeId && v.UserId == userId);

			if (type == null)
			{
				// remove existing vote
				if (existing != null) _context.TreeVotes.Remove(existing);
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
				.Select(g => new VotesCount
				{
					Like = g.Count(v => v.Type == VoteType.Like),
					Dislike = g.Count(v => v.Type == VoteType.Dislike)
				})
				.FirstOrDefaultAsync() ?? new VotesCount();

			return counts;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Błąd podczas nadawania głosu");
			throw;
		}
	}

	private static TreeSubmissionDto MapToTreeSubmissionDto(TreeSubmission s)
	{
		return new TreeSubmissionDto
		{
			Id = s.Id,
			UserData = new UserData
			{
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
			Images = s.Images,
			IsMonument = s.IsMonument,
			Status = s.Status,
			SubmissionDate = s.SubmissionDate,
			ApprovalDate = s.ApprovalDate,
			Votes = new VotesCount
			{
				Like = s.TreeVotes.Count(v => v.Type == VoteType.Like),
				Dislike = s.TreeVotes.Count(v => v.Type == VoteType.Dislike)
			},
			CommentCount = s.Comments.Count
		};
	}

}
