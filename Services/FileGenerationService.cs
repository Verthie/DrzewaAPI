using PuppeteerSharp;
using PuppeteerSharp.Media;

namespace DrzewaAPI.Services;

public class FileGenerationService(IAzureStorageService _azureStorageService, ILogger<FileGenerationService> _logger) : IFileGenerationService
{
	public async Task<string> GenerateHtmlFromTemplateAsync(string template, Dictionary<string, object> data)
	{
		try
		{
			// Simple template engine - replace {{key}} with values
			var html = template;

			foreach (var item in data)
			{
				var placeholder = "{{" + item.Key + "}}";
				var value = item.Value?.ToString() ?? "";
				html = html.Replace(placeholder, value);
			}

			html = html.Replace("{{generation_date}}", DateTime.Now.ToString("dd.MM.yyyy"));

			return await Task.FromResult(html);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Błąd podczas generowania HTML");
			throw;
		}
	}

	public async Task<string> GeneratePdfAsync(string htmlContent, string folderPath)
	{
		try
		{
			string uniqueFileName = $"wniosek_{Guid.NewGuid()}.pdf";

			// Configure launch options for containerized environment
			var launchOptions = new LaunchOptions
			{
				Headless = true,
				Args = new[]
				{
				"--no-sandbox",
				"--disable-setuid-sandbox",
				"--disable-dev-shm-usage", // Overcome limited resource problems
                "--disable-gpu"
			}
			};

			// Check if running in container (system Chromium available)
			var executablePath = Environment.GetEnvironmentVariable("PUPPETEER_EXECUTABLE_PATH");
			if (!string.IsNullOrEmpty(executablePath) && File.Exists(executablePath))
			{
				launchOptions.ExecutablePath = executablePath;
				_logger.LogInformation($"Using system Chromium at: {executablePath}");
			}
			else
			{
				// Local development - download Chromium
				_logger.LogInformation("Downloading Chromium for local development");
				BrowserFetcher browserFetcher = new BrowserFetcher();
				await browserFetcher.DownloadAsync();
			}

			using var browser = await Puppeteer.LaunchAsync(launchOptions);
			using var page = await browser.NewPageAsync();
			await page.SetContentAsync(htmlContent);

			byte[] pdfBytes = await page.PdfDataAsync(new PdfOptions
			{
				Format = PaperFormat.A4,
				PrintBackground = true,
				MarginOptions = new MarginOptions
				{
					Top = "20mm",
					Right = "20mm",
					Bottom = "20mm",
					Left = "20mm"
				}
			});

			string relativePath = await _azureStorageService.SavePdfAsync(pdfBytes, uniqueFileName, folderPath);
			_logger.LogInformation($"PDF wygenerowany i zapisany w Azure Storage: {relativePath}");
			return relativePath;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Błąd podczas generowania PDF");
			throw;
		}
	}
}
