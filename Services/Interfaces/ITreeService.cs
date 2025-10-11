namespace DrzewaAPI.Services;

public interface ITreeService
{
	Task<List<TreeSubmissionDto>> GetTreeSubmissionsAsync();
	Task<List<TreeSubmissionDto>> GetCurrentUserTreeSubmissionsAsync(Guid userId);
	Task<TreeSubmissionDto> GetTreeSubmissionByIdAsync(Guid treeId);
	Task<TreeSubmissionDto> CreateTreeSubmissionAsync(CreateTreeSubmissionDto request, IFormFileCollection images, Guid userId, IFormFile screenshot);
	Task<TreeSubmissionDto> UpdateTreeSubmissionAsync(Guid id, UpdateTreeSubmissionDto req, IFormFileCollection? images, Guid currentUserId, bool isModerator);
	Task DeleteTreeSubmissionAsync(Guid treeId, Guid userId, bool isModerator);
	Task ApproveTreeAsync(Guid treeId);
	Task<int> SetVoteAsync(Guid treeId, Guid userId, bool vote);
}
