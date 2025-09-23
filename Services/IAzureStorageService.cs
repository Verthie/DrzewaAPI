namespace DrzewaAPI.Services;

public interface IAzureStorageService
{
	Task<List<string>> SaveImagesAsync(IFormFileCollection images, string folderPath);
	Task<string> SavePdfAsync(byte[] pdfContent, string fileName, string folderPath);
	void DeleteImages(List<string> imagePaths);
	Task DeletePdfAsync(string pdfPath);
	string GetFileUrl(string filePath);
	bool ValidateImage(IFormFile image);
}
