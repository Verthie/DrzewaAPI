using System;
using DrzewaAPI.Data;
using DrzewaAPI.Dtos.Comment;
using DrzewaAPI.Models;
using DrzewaAPI.Models.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace DrzewaAPI.Services;

public class CommentService(ApplicationDbContext _context, ILogger<CommentService> _logger) : ICommentService
{
	public async Task<CommentDto?> GetCommentByIdAsync(Guid commentId)
	{
		try
		{
			Comment? comment = await _context.Comments
				.Include(c => c.User)
				.Include(c => c.Likes)
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
				.Include(c => c.Likes)
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
			await _context.Entry(comment).Reference(s => s.User).LoadAsync();
			await _context.Entry(comment).Collection(s => s.Likes).LoadAsync();

			return MapToCommentDto(comment);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Błąd podczas tworzenia komentarza do drzewa o Id: {TreeId}", treeId);
			throw;
		}
	}

	private static CommentDto MapToCommentDto(Comment s)
	{
		return new CommentDto
		{
			Id = s.Id,
			UserData = new UserData
			{
				UserName = s.User.FullName,
				Avatar = s.User.Avatar
			},
			Content = s.Content,
			DatePosted = s.DatePosted,
			IsLegend = s.IsLegend,
			LikesCount = s.Likes.Count
		};
	}
}
