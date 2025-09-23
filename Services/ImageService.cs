using System;

namespace DrzewaAPI.Services;

public class ImageService(IAzureStorageService _azureStorageService) : IImageService
{
	public async Task<List<string>> SaveImagesAsync(IFormFileCollection images, string folderPath)
	{
		return await _azureStorageService.SaveImagesAsync(images, folderPath);
	}

	public void DeleteImages(List<string> imagePaths)
	{
		_azureStorageService.DeleteImages(imagePaths);
	}

	public bool ValidateImage(IFormFile image)
	{
		return _azureStorageService.ValidateImage(image);
	}
}

