using System;
using DrzewaAPI.Data;
using DrzewaAPI.Dtos.Comment;
using DrzewaAPI.Models;
using DrzewaAPI.Models.Enums;
using DrzewaAPI.Models.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace DrzewaAPI.Services;

public class CommentService(ApplicationDbContext _context, ILogger<CommentService> _logger) : ICommentService
{

	public async Task<List<CommentDto>> GetCommentsAsync()
	{
		try
		{
			List<CommentDto> comments = await _context.Comments
				.Include(c => c.User)
				.Include(c => c.CommentVotes)
				.Include(c => c.TreeSubmission)
					.ThenInclude(s => s.Species)
				.Select(c => MapToCommentDto(c))
				.ToListAsync();

			return comments;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Błąd podczas pobierania listy drzew");
			throw;
		}
	}

	public async Task<CommentDto?> GetCommentByIdAsync(Guid commentId)
	{
		try
		{
			Comment? comment = await _context.Comments
				.Include(c => c.User)
				.Include(c => c.CommentVotes)
				.FirstOrDefaultAsync(c => c.Id == commentId);

			if (comment == null) return null;

			return MapToCommentDto(comment);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Błąd podczas komentarza o Id: {CommentId}", commentId);
			throw;
		}
	}

	public async Task<List<CommentDto>> GetTreeCommentsAsync(Guid treeId)
	{
		try
		{
			List<CommentDto> comments = await _context.Comments
				.Include(c => c.User)
				.Include(c => c.CommentVotes)
				.Where(c => c.TreeSubmissionId == treeId)
				.Select(c => MapToCommentDto(c))
				.ToListAsync();

			return comments;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Błąd podczas pobierania listy komentarzy dla drzewa o Id: {TreeId}", treeId);
			throw;
		}
	}


	public async Task<CommentDto?> CreateCommentAsync(CreateCommentDto request, Guid userId, Guid treeId)
	{
		try
		{
			Comment comment = new Comment
			{
				Id = Guid.NewGuid(),
				UserId = userId,
				TreeSubmissionId = treeId,
				Content = request.Content,
				IsLegend = request.IsLegend,
			};

			_context.Comments.Add(comment);
			await _context.SaveChangesAsync();

			// Load navigation properties
			await _context.Entry(comment).Reference(c => c.User).LoadAsync();
			await _context.Entry(comment).Collection(c => c.CommentVotes).LoadAsync();

			return MapToCommentDto(comment);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Błąd podczas tworzenia komentarza do drzewa o Id: {TreeId}", treeId);
			throw;
		}
	}

	public async Task<bool> DeleteCommentAsync(Guid commentId, Guid userId, bool isModerator)
	{
		try
		{
			Comment? comment = isModerator
					? await _context.Comments.FirstOrDefaultAsync(a => a.Id == commentId)
					: await _context.Comments.FirstOrDefaultAsync(a => a.Id == commentId && a.UserId == userId);

			if (comment == null)
				return false;

			_context.Comments.Remove(comment);
			await _context.SaveChangesAsync();

			return true;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Błąd podczas usuwania komentarza");
			throw;
		}
	}

	public async Task<VotesCount?> SetVoteAsync(Guid commentId, Guid userId, VoteType? type)
	{
		try
		{
			Comment? comment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == commentId);

			if (comment == null) return null;

			CommentVote? existing = await _context.CommentVotes
				.SingleOrDefaultAsync(v => v.CommentId == commentId && v.UserId == userId);

			if (type == null)
			{
				// remove existing vote
				if (existing != null) _context.CommentVotes.Remove(existing);
			}
			else
			{
				if (existing == null)
				{
					// add new vote
					_context.CommentVotes.Add(new CommentVote
					{
						Id = Guid.NewGuid(),
						CommentId = comment.Id,
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

			var counts = await _context.CommentVotes
				.Where(v => v.CommentId == commentId)
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

	private static CommentDto MapToCommentDto(Comment c)
	{
		return new CommentDto
		{
			Id = c.Id,
			TreePolishName = c.TreeSubmission?.Species.PolishName,
			UserData = new UserData
			{
				UserName = c.User.FullName,
				Avatar = c.User.Avatar
			},
			Content = c.Content,
			DatePosted = c.DatePosted,
			IsLegend = c.IsLegend,
			Votes = new VotesCount
			{
				Like = c.CommentVotes.Count(v => v.Type == VoteType.Like),
				Dislike = c.CommentVotes.Count(v => v.Type == VoteType.Dislike)
			},
		};
	}
}
