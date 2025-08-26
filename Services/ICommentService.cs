using System;
using DrzewaAPI.Dtos.Comment;

namespace DrzewaAPI.Services;

public interface ICommentService
{
	Task<CommentDto?> GetCommentByIdAsync(Guid commentId);
	Task<List<CommentDto>> GetTreeCommentsAsync(Guid treeId);
	Task<CommentDto?> CreateCommentAsync(CreateCommentDto request, Guid userId, Guid treeId);
}
