using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;

namespace DrzewaAPI.Services;

public class AzureStorageService : IAzureStorageService
{
	private readonly BlobServiceClient _blobServiceClient;
	private readonly ILogger<AzureStorageService> _logger;
	private readonly string _containerName;
	private readonly string _baseUrl;

	private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
	private readonly long _maxFileSize = 5 * 1024 * 1024; // 5MB

	public AzureStorageService(BlobServiceClient blobServiceClient, ILogger<AzureStorageService> logger, IConfiguration configuration)
	{
		_blobServiceClient = blobServiceClient;
		_logger = logger;
		_containerName = configuration["AzureStorage:ContainerName"] ?? "uploads";
		_baseUrl = configuration["AzureStorage:BaseUrl"] ?? "";
	}

	public async Task<List<string>> SaveImagesAsync(IFormFileCollection images, string folderPath)
	{
		List<string> savedPaths = new List<string>();

		if (images == null || !images.Any())
			return savedPaths;

		BlobContainerClient containerClient = GetContainerClient();

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
				string blobName = $"{folderPath}/{uniqueFileName}";

				BlobClient blobClient = containerClient.GetBlobClient(blobName);

				// Set content type
				BlobUploadOptions uploadOptions = new BlobUploadOptions
				{
					HttpHeaders = new BlobHttpHeaders
					{
						ContentType = GetContentType(fileExtension)
					}
				};

				// Upload file
				using (Stream stream = image.OpenReadStream())
				{
					await blobClient.UploadAsync(stream, uploadOptions);
				}

				// Store relative path for database
				string relativePath = $"{folderPath}/{uniqueFileName}";
				savedPaths.Add(relativePath);

				_logger.LogInformation($"Image saved to Azure Storage: {relativePath}");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"Error saving image to Azure Storage: {image.FileName}");
				throw new InvalidOperationException($"Failed to save image: {image.FileName}");
			}
		}

		return savedPaths;
	}

	public async Task<string> SavePdfAsync(byte[] pdfContent, string fileName, string folderPath)
	{
		try
		{
			BlobContainerClient containerClient = GetContainerClient();
			string blobName = $"{folderPath}/{fileName}";

			BlobClient blobClient = containerClient.GetBlobClient(blobName);

			// Set content type
			BlobUploadOptions uploadOptions = new BlobUploadOptions
			{
				HttpHeaders = new BlobHttpHeaders
				{
					ContentType = "application/pdf"
				}
			};

			// Upload file
			using (MemoryStream stream = new MemoryStream(pdfContent))
			{
				await blobClient.UploadAsync(stream, uploadOptions);
			}

			string relativePath = $"{folderPath}/{fileName}";
			_logger.LogInformation($"PDF saved to Azure Storage: {relativePath}");

			return relativePath;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, $"Error saving PDF to Azure Storage: {fileName}");
			throw new InvalidOperationException($"Failed to save PDF: {fileName}");
		}
	}

	public void DeleteImages(List<string> imagePaths)
	{
		foreach (string imagePath in imagePaths)
		{
			try
			{
				DeleteBlobAsync(imagePath).Wait();
				_logger.LogInformation($"Image deleted from Azure Storage: {imagePath}");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"Error deleting image from Azure Storage: {imagePath}");
			}
		}
	}

	public async Task DeletePdfAsync(string pdfPath)
	{
		try
		{
			await DeleteBlobAsync(pdfPath);
			_logger.LogInformation($"PDF deleted from Azure Storage: {pdfPath}");
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, $"Error deleting PDF from Azure Storage: {pdfPath}");
		}
	}

	public string GetFileUrl(string filePath)
	{
		if (string.IsNullOrEmpty(filePath))
			return string.Empty;

		// If baseUrl is configured, use it, otherwise construct from blob client
		if (!string.IsNullOrEmpty(_baseUrl))
		{
			return $"{_baseUrl.TrimEnd('/')}/{_containerName}/{filePath}";
		}

		// Fallback to constructing URL from blob client
		BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
		BlobClient blobClient = containerClient.GetBlobClient(filePath);
		return blobClient.Uri.ToString();
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

	private BlobContainerClient GetContainerClient()
	{
		BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
		// Container already exists, no need to create it
		return containerClient;
	}

	private async Task DeleteBlobAsync(string blobPath)
	{
		BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
		BlobClient blobClient = containerClient.GetBlobClient(blobPath);
		await blobClient.DeleteIfExistsAsync();
	}

	private static string GetContentType(string fileExtension)
	{
		return fileExtension.ToLowerInvariant() switch
		{
			".jpg" or ".jpeg" => "image/jpeg",
			".png" => "image/png",
			".gif" => "image/gif",
			".webp" => "image/webp",
			_ => "application/octet-stream"
		};
	}
}
