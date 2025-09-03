using System;
using DrzewaAPI.Dtos.Comment;
using DrzewaAPI.Models.Enums;
using DrzewaAPI.Models.ValueObjects;

namespace DrzewaAPI.Services;

public interface ICommentService
{
	Task<List<CommentDto>> GetCommentsAsync();
	Task<CommentDto?> GetCommentByIdAsync(Guid commentId);
	Task<List<CommentDto>> GetTreeCommentsAsync(Guid treeId);
	Task<CommentDto?> CreateCommentAsync(CreateCommentDto request, Guid userId, Guid treeId);
	Task<VotesCount?> SetVoteAsync(Guid treeId, Guid userId, VoteType? type);
}
