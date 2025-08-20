using System;
using DrzewaAPI.Dtos.TreeSubmissions;

namespace DrzewaAPI.Services;

public interface ITreeService
{
	Task<List<TreeSubmissionDto>> GetTreeSubmissionsAsync();
}
