using System;
using DrzewaAPI.Dtos.TreeSubmissions;
using DrzewaAPI.Models.Enums;
using DrzewaAPI.Models.ValueObjects;

namespace DrzewaAPI.Services;

public interface ITreeService
{
	Task<List<TreeSubmissionDto>> GetTreeSubmissionsAsync();
	Task<List<TreeSubmissionDto>> GetCurrentUserTreeSubmissionsAsync(Guid userId);
	Task<TreeSubmissionDto?> GetTreeSubmissionByIdAsync(Guid treeId);
	Task<TreeSubmissionDto?> CreateTreeSubmissionAsync(CreateTreeSubmissionDto request, Guid userId);
	Task<VotesCount?> SetVoteAsync(Guid treeId, Guid userId, VoteType? type);
}
