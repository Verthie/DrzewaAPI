using System;
using DrzewaAPI.Dtos.TreeSubmissions;

namespace DrzewaAPI.Services;

public interface ITreeService
{
	Task<List<TreeSubmissionDto>> GetTreeSubmissionsAsync();
	Task<TreeSubmissionDto?> GetTreeSubmissionByIdAsync(Guid treeId);
	Task<TreeSubmissionDto?> CreateTreeSubmissionAsync(CreateTreeSubmissionDto request, Guid userId);
}
