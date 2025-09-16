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
			_logger.LogError(ex, "Błąd podczas pobierania listy komentarzy");
			throw new ServiceException($"Nie można pobrać listy komentarzy", "COMMENT_FETCH_ERROR");
		}
	}

	public async Task<CommentDto> GetCommentByIdAsync(Guid commentId)
	{
		try
		{
			Comment comment = await _context.Comments
				.Include(c => c.User)
				.Include(c => c.CommentVotes)
				.FirstOrDefaultAsync(c => c.Id == commentId)
				?? throw EntityNotFoundException.ForComment(commentId);

			return MapToCommentDto(comment);
		}
		catch (BusinessException)
		{
			throw;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Błąd podczas pobierania drzewa {CommentId}", commentId);
			throw new ServiceException($"Nie można pobrać drzewa {commentId}", "COMMENT_FETCH_ERROR");
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
			_logger.LogError(ex, "Błąd podczas pobierania listy komentarzy");
			throw new ServiceException($"Nie można pobrać listy komentarzy", "COMMENT_FETCH_ERROR");
		}
	}


	public async Task<CommentDto> CreateCommentAsync(CreateCommentDto request, Guid userId, Guid treeId)
	{
		try
		{
			bool submissionExists = await _context.TreeSubmissions.AnyAsync(s => s.Id == treeId);
			if (!submissionExists) throw EntityNotFoundException.ForTree(treeId);

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
		catch (BusinessException)
		{
			throw;
		}
		catch (DbUpdateException ex)
		{
			_logger.LogError(ex, "Błąd bazy danych podczas tworzenia drzewa");
			throw EntityCreationFailedException.ForComment("Błąd podczas zapisu do bazy danych");
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Nieoczekiwany błąd podczas tworzenia komentarza");
			throw EntityCreationFailedException.ForComment("Nieoczekiwany błąd systemu");
		}
	}

	public async Task DeleteCommentAsync(Guid commentId, Guid userId, bool isModerator)
	{
		try
		{
			Comment comment = await _context.Comments
				.FirstOrDefaultAsync(c => c.Id == commentId)
				?? throw EntityNotFoundException.ForComment(commentId);

			// Check privileges
			if (!isModerator && comment.UserId != userId) throw EntityAccessDeniedException.ForComment(commentId, userId);

			_context.Comments.Remove(comment);
			await _context.SaveChangesAsync();
		}
		catch (BusinessException)
		{
			throw;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Błąd podczas usuwania komentarza {CommentId}", commentId);
			throw new ServiceException($"Nie można usunąć komentarza {commentId}", "COMMENT_DELETE_ERROR");
		}
	}

	public async Task<VotesCount> SetVoteAsync(Guid commentId, Guid userId, VoteType? type)
	{
		try
		{
			Comment comment = await _context.Comments
				.FirstOrDefaultAsync(c => c.Id == commentId)
				?? throw EntityNotFoundException.ForComment(commentId);

			CommentVote? existing = await _context.CommentVotes
				.SingleOrDefaultAsync(v => v.CommentId == commentId && v.UserId == userId);

			if (type == null)
			{
				if (existing == null) throw new ServiceException("Nie znaleziono istniejącego głosu na komentarz", "VOTE_NOT_FOUND");

				// remove existing vote
				_context.CommentVotes.Remove(existing);
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
		catch (BusinessException)
		{
			throw;
		}
		catch (DbUpdateException ex)
		{
			_logger.LogError(ex, "Błąd podczas wprowadzania głosu do bazy danych");
			throw EntityVoteException.ForComment(commentId, "Błąd podczas zapisu do bazy danych");
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Nieoczekiwany błąd podczas oddawania głosu");
			throw EntityVoteException.ForComment(commentId, "Nieoczekiwany błąd systemu");
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
