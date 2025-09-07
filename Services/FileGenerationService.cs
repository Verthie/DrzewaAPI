using System;
using DrzewaAPI.Data;
using DrzewaAPI.Models.Enums;
using Microsoft.EntityFrameworkCore;
using PuppeteerSharp;
using PuppeteerSharp.Media;

namespace DrzewaAPI.Services;

public class FileGenerationService(IWebHostEnvironment _environment, ILogger<FileGenerationService> _logger) : IFileGenerationService
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

	public async Task<string> GeneratePdfAsync(string htmlContent, string fileName)
	{
		try
		{
			// Ensure the pdfs directory exists
			var pdfsDirectory = Path.Combine(_environment.WebRootPath, "pdfs");
			if (!Directory.Exists(pdfsDirectory))
			{
				Directory.CreateDirectory(pdfsDirectory);
			}

			var filePath = Path.Combine(pdfsDirectory, fileName);

			// PDF generation with PuppeteerSharp
			BrowserFetcher browserFetcher = new BrowserFetcher();
			await browserFetcher.DownloadAsync();

			using var browser = await Puppeteer.LaunchAsync(new LaunchOptions
			{
				Headless = true
			});

			using var page = await browser.NewPageAsync();
			await page.SetContentAsync(htmlContent);

			await page.PdfAsync(filePath, new PdfOptions
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

			_logger.LogInformation($"PDF wygenerowany: {filePath}");
			return $"/pdfs/{fileName}";
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Błąd podczas generowania PDF");
			throw;
		}
	}
}
