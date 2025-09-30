using System;
using System.Text.Json;
using DotNetEnv;
using DrzewaAPI.Models;

namespace DrzewaAPI.Services;

public class GeoportalService : IGeoportalService
{
	private readonly HttpClient _httpClient;
	private readonly ILogger<GeoportalService> _logger;

	public GeoportalService(HttpClient httpClient, ILogger<GeoportalService> logger)
	{
		_httpClient = httpClient;
		_logger = logger;

		_httpClient.Timeout = TimeSpan.FromSeconds(10);

		_httpClient.DefaultRequestHeaders.Clear();
		_httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36");
		_httpClient.DefaultRequestHeaders.Add("Accept", "application/json, text/plain, */*");
		_httpClient.DefaultRequestHeaders.Add("Accept-Language", "pl-PL,pl;q=0.9,en-US;q=0.8,en;q=0.7");
	}

	public async Task<double[]> ConvertCoordinates(double longitude, double latitude)
	{
		try
		{
			double x = 20;
			double y = 20;

			Env.Load();

			string? key = Environment.GetEnvironmentVariable("MAPTILER_KEY");

			Uri url = new Uri($"https://api.maptiler.com/coordinates/transform/{longitude},{latitude}.json?key={key}&s_srs=4326&t_srs=2180");

			var response = await _httpClient.GetAsync(url);

			if (!response.IsSuccessStatusCode)
			{
				_logger.LogWarning($"API Geoportal zwróciło status: {response.StatusCode}");
			}

			var json = await response.Content.ReadAsStringAsync();

			if (string.IsNullOrWhiteSpace(json) || json == "[]")
			{
				_logger.LogInformation("Nie udało się uzyskać konwersji dla podanych koordynatów");
			}

			using var doc = JsonDocument.Parse(json);

			x = doc.RootElement
			  .GetProperty("results")[0]
			  .GetProperty("x")
			  .GetDouble();

			y = doc.RootElement
			  .GetProperty("results")[0]
			  .GetProperty("y")
			  .GetDouble();

			_logger.LogInformation($"Przekonwertowano koordynaty na współrzędne: X = {x}, Y = {y}");

			return [x, y];
		}
		catch (HttpRequestException ex)
		{
			_logger.LogError(ex, "Błąd podczas komunikacji z API Maptiler");
			throw;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Nieoczekiwany błąd podczas pobierania danych działki");
			throw;
		}
	}

	public async Task<Plot?> GetPlotByLocationAsync(double longitude, double latitude)
	{
		double[] coordinates = await ConvertCoordinates(longitude, latitude);

		double x = coordinates[0];
		double y = coordinates[1];

		try
		{
			// API ULDK takes coordinates in format lon,lat for WGS84
			Uri url = new Uri($"https://uldk.gugik.gov.pl/?request=GetParcelByXY&xy={x},{y}&result=id,numer,powiat,wojewodztwo,gmina,region");

			_logger.LogInformation($"Pobieranie danych działki dla współrzędnych: {x}, {y}");

			var response = await _httpClient.GetAsync(url);

			if (!response.IsSuccessStatusCode)
			{
				_logger.LogWarning($"API Geoportal zwróciło status: {response.StatusCode}");
				return null;
			}

			var content = await response.Content.ReadAsStringAsync();

			_logger.LogDebug($"Odpowiedź z API: {content}");

			string[] segments = content.Split("|");

			Plot plot = new Plot
			{
				PlotNumber = segments[1],
				District = segments[5],
				Province = segments[3],
				County = segments[2],
				Commune = segments[4],
			};

			return plot;
		}
		catch (HttpRequestException ex)
		{
			_logger.LogError(ex, "Błąd podczas komunikacji z API Geoportal");
			return null;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Nieoczekiwany błąd podczas pobierania danych działki");
			return null;
		}
	}
}
