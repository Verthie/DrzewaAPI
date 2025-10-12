using System;

namespace DrzewaAPI.Services;

public interface IGeminiService
{
	Task<string> GetJustificationAsync(Guid treeId);
	Task<string> GetTestMessageAsync();
}
