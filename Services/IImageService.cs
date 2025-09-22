using System;

namespace DrzewaAPI.Services;

public interface IImageService
{
	Task<List<string>> SaveImagesAsync(IFormFileCollection images, string folderPath);
	void DeleteImages(List<string> imagePaths);
	bool ValidateImage(IFormFile image);
}
