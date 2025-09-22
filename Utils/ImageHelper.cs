using System;

namespace DrzewaAPI.Utils;

public static class ImageHelper
{
	public static string GetImageUrl(string imagePath, IHttpContextAccessor httpContextAccessor)
	{
		if (string.IsNullOrEmpty(imagePath))
			return string.Empty;

		HttpRequest? request = httpContextAccessor.HttpContext?.Request;
		string baseUrl = $"{request?.Scheme}://{request?.Host}";

		// Ensure path starts with /
		string normalizedPath = imagePath.StartsWith("/") ? imagePath : "/" + imagePath;

		return $"{baseUrl}{normalizedPath}";
	}
}
