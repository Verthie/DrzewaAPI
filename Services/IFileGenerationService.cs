using System;

namespace DrzewaAPI.Services;

public interface IFileGenerationService
{
	Task<string> GenerateHtmlFromTemplateAsync(string template, Dictionary<string, object> data);
	Task<string> GeneratePdfAsync(string htmlContent, string folderPath);
}
