using System;
using System.Text.Json;
using DotNetEnv;
using DotSpatial.Projections;
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

		_httpClient.Timeout = TimeSpan.FromSeconds(20);

		_httpClient.DefaultRequestHeaders.Clear();
		_httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36");
		_httpClient.DefaultRequestHeaders.Add("Accept", "application/json, text/plain, */*");
		_httpClient.DefaultRequestHeaders.Add("Accept-Language", "pl-PL,pl;q=0.9,en-US;q=0.8,en;q=0.7");
	}

	public double[] ConvertCoordinates(double longitude, double latitude)
	{
		try
		{
			var source = ProjectionInfo.FromEpsgCode(4326);
			var target = ProjectionInfo.FromEpsgCode(2180);

			double[] xy = { longitude, latitude };  // longitude, latitude
			double[] z = { 0 };

			Reproject.ReprojectPoints(xy, z, source, target, 0, 1);

			Console.WriteLine($"X: {xy[0]}, Y: {xy[1]}");

			_logger.LogInformation($"Przekonwertowano koordynaty na współrzędne: X = {xy[0]}, Y = {xy[1]}");

			return xy;
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
		double[] coordinates = ConvertCoordinates(longitude, latitude);

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

			string content = await response.Content.ReadAsStringAsync();

			_logger.LogDebug($"Odpowiedź z API: {content}");

			string[] segments = content.TrimEnd('\r', '\n').Split("|");

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
