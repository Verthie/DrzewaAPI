using System;

namespace DrzewaAPI.Services;

public interface IGeminiService
{
	Task<string> GetJustificationAsync(Guid applicationId);
	Task<string> GetTestMessageAsync();
}
