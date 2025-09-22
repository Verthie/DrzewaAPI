using System;

namespace DrzewaAPI.Utils;

public static class FileHelper
{
	public static string GetFileUrl(string filePath, IHttpContextAccessor httpContextAccessor)
	{
		if (string.IsNullOrEmpty(filePath))
			return string.Empty;

		HttpRequest? request = httpContextAccessor.HttpContext?.Request;
		string baseUrl = $"{request?.Scheme}://{request?.Host}";

		// Ensure path starts with /
		string normalizedPath = filePath.StartsWith("/") ? filePath : "/" + filePath;

		return $"{baseUrl}{normalizedPath}";
	}
}
