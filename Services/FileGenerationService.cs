using iText.Forms;
using iText.Forms.Fields;
using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
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

	public async Task<string> GeneratePdfAsync(string htmlContent, string folderPath, SignatureDto signature)
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

			// Adding Signature Field to PDF
			byte[] pdfWithSignature = AddSignatureFieldToBytes(pdfBytes, signature);

			// Saving final PDF
			string relativePath = await _azureStorageService.SavePdfAsync(pdfWithSignature, uniqueFileName, folderPath);
			_logger.LogInformation($"PDF z polem podpisu zapisany w Azure Storage: {relativePath}");

			return relativePath;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Błąd podczas generowania PDF");
			throw;
		}
	}

	private byte[] AddSignatureFieldToBytes(byte[] inputPdfBytes, SignatureDto signature)
	{
		try
		{
			using var inputStream = new MemoryStream(inputPdfBytes);
			using var outputStream = new MemoryStream();
			using var reader = new PdfReader(inputStream);
			using var writer = new PdfWriter(outputStream);
			using var pdfDoc = new PdfDocument(reader, writer);

			// Adding Metadata
			var info = pdfDoc.GetDocumentInfo();
			info.SetTitle("Wniosek");

			var page = pdfDoc.GetFirstPage();
			var pageSize = page.GetPageSize();

			// Setting the position of the field
			float signatureWidth = signature.Width;
			float signatureHeight = signature.Height;
			float signatureX = signature.X;
			float signatureY = signature.Y;

			Rectangle signatureRect = new Rectangle(
				signatureX,
				signatureY,
				signatureWidth,
				signatureHeight
			);

			PdfAcroForm form = PdfAcroForm.GetAcroForm(pdfDoc, true);

			// Creation of signature field
			PdfSignatureFormField signatureField = new SignatureFormFieldBuilder(pdfDoc, "Podpis_Cyfrowy")
				.SetWidgetRectangle(signatureRect)
				.SetPage(1)
				.CreateSignature();

			signatureField.SetFieldName("Podpis_Cyfrowy");

			// Adding field to form
			form.AddField(signatureField);

			_logger.LogInformation("✓ Pole podpisu dodane do PDF");

			pdfDoc.Close();

			return outputStream.ToArray();
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Błąd podczas dodawania pola podpisu");
			throw;
		}
	}
}
