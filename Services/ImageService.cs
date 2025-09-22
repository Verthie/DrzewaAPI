using System;

namespace DrzewaAPI.Services;

public class ImageService(IWebHostEnvironment _environment, ILogger<ImageService> _logger) : IImageService
{
	private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
	private readonly long _maxFileSize = 5 * 1024 * 1024; // 5MB

	public async Task<List<string>> SaveImagesAsync(IFormFileCollection images, string folderPath)
	{
		List<string> savedPaths = new List<string>();

		if (images == null || !images.Any())
			return savedPaths;

		// Create directory if it doesn't exist
		string uploadsPath = Path.Combine(_environment.WebRootPath, folderPath);

		if (!Directory.Exists(uploadsPath))
		{
			Directory.CreateDirectory(uploadsPath);
		}

		foreach (IFormFile image in images)
		{
			if (!ValidateImage(image))
			{
				_logger.LogWarning($"Invalid image file: {image.FileName}");
				continue;
			}

			try
			{
				// Generate unique filename
				string fileExtension = Path.GetExtension(image.FileName).ToLowerInvariant();
				string uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
				string filePath = Path.Combine(uploadsPath, uniqueFileName);

				// Save file
				using (FileStream stream = new FileStream(filePath, FileMode.Create))
				{
					await image.CopyToAsync(stream);
				}

				// Store relative path for database
				string relativePath = Path.Combine(folderPath, uniqueFileName).Replace("\\", "/");
				savedPaths.Add(relativePath);

				_logger.LogInformation($"Image saved: {relativePath}");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"Error saving image: {image.FileName}");
				throw new InvalidOperationException($"Failed to save image: {image.FileName}");
			}
		}

		return savedPaths;
	}

	public void DeleteImages(List<string> imagePaths)
	{
		foreach (string imagePath in imagePaths)
		{
			try
			{
				string fullPath = Path.Combine(_environment.WebRootPath, imagePath.TrimStart('/'));
				if (File.Exists(fullPath))
				{
					File.Delete(fullPath);
					_logger.LogInformation($"Image deleted: {imagePath}");
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"Error deleting image: {imagePath}");
			}
		}
	}

	public bool ValidateImage(IFormFile image)
	{
		if (image == null || image.Length == 0)
			return false;

		if (image.Length > _maxFileSize)
		{
			_logger.LogWarning($"Image too large: {image.FileName} ({image.Length} bytes)");
			return false;
		}

		string extension = Path.GetExtension(image.FileName).ToLowerInvariant();
		if (!_allowedExtensions.Contains(extension))
		{
			_logger.LogWarning($"Invalid file extension: {extension}");
			return false;
		}

		return true;
	}
}

