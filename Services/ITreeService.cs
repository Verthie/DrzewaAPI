namespace DrzewaAPI.Services;

public interface ITreeService
{
	Task<List<TreeSubmissionDto>> GetTreeSubmissionsAsync();
	Task<List<TreeSubmissionDto>> GetCurrentUserTreeSubmissionsAsync(Guid userId);
	Task<TreeSubmissionDto> GetTreeSubmissionByIdAsync(Guid treeId);
	Task<TreeSubmissionDto> CreateTreeSubmissionAsync(CreateTreeSubmissionDto request, IFormFileCollection images, Guid userId);
	Task DeleteTreeSubmissionAsync(Guid treeId, Guid userId, bool isModerator);
	Task ApproveTreeAsync(Guid treeId);
	Task<VotesDto> SetVoteAsync(Guid treeId, Guid userId, VoteType? type);
}
