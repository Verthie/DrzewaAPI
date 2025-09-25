using System;
using DrzewaAPI.Services;

namespace DrzewaAPI.Utils;

public static class FileHelper
{
	public static string GetFileUrl(string filePath, IHttpContextAccessor httpContextAccessor)
	{
		if (string.IsNullOrEmpty(filePath))
			return string.Empty;

		// If it's an Azure Storage URL (starts with https://), return as is
		if (filePath.StartsWith("https://"))
			return filePath;

		// Fallback to local file serving for backward compatibility
		HttpRequest? request = httpContextAccessor.HttpContext?.Request;
		string baseUrl = $"{request?.Scheme}://{request?.Host}";

		// Ensure path starts with /
		string normalizedPath = filePath.StartsWith("/") ? filePath : "/" + filePath;

		return $"{baseUrl}{normalizedPath}";
	}

	public static string GetFileUrl(string filePath, IAzureStorageService azureStorageService)
	{
		if (string.IsNullOrEmpty(filePath))
			return string.Empty;

		return azureStorageService.GetFileUrl(filePath);
	}
}
