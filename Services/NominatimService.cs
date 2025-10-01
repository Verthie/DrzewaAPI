using System;
using System.Text.Json;
using Nominatim.API.Geocoders;
using Nominatim.API.Models;

namespace DrzewaAPI.Services;

public class NominatimService : INominatimService
{
	private readonly ILogger<NominatimService> _logger;
	private readonly HttpClient _httpClient;
	private static DateTime _lastRequestTime = DateTime.MinValue;
	private static readonly SemaphoreSlim _rateLimitSemaphore = new SemaphoreSlim(1, 1);

	public NominatimService(ILogger<NominatimService> logger, HttpClient httpClient)
	{
		_logger = logger;
		_httpClient = httpClient;
		_httpClient.DefaultRequestHeaders.Add("User-Agent", "DrzewaAPI/1.0"); // Required by Nominatim
	}

	public async Task<string?> GetAddressByLocationAsync(double latitude, double longitude)
	{
		try
		{
			// Enforce Nominatim rate limit (1 request per second)
			await _rateLimitSemaphore.WaitAsync();
			try
			{
				var timeSinceLastRequest = DateTime.UtcNow - _lastRequestTime;
				if (timeSinceLastRequest.TotalMilliseconds < 1000)
				{
					var delay = 1000 - (int)timeSinceLastRequest.TotalMilliseconds;
					_logger.LogDebug($"Rate limiting: czekanie {delay}ms");
					await Task.Delay(delay);
				}
				_lastRequestTime = DateTime.UtcNow;
			}
			finally
			{
				_rateLimitSemaphore.Release();
			}

			Uri url = new Uri($"https://nominatim.openstreetmap.org/reverse?format=json&lat={latitude}&lon={longitude}&addressdetails=1&accept-language=pl");

			_logger.LogInformation($"Pobieranie adresu dla współrzędnych: {latitude}, {longitude}");

			var response = await _httpClient.GetAsync(url);

			if (!response.IsSuccessStatusCode)
			{
				_logger.LogWarning($"Nominatim API zwróciło status: {response.StatusCode}");
				return null;
			}

			string content = await response.Content.ReadAsStringAsync();

			_logger.LogDebug($"Odpowiedź z Nominatim: {content}");

			var jsonDoc = JsonDocument.Parse(content);
			var addressElement = jsonDoc.RootElement.GetProperty("address");

			string? result = BuildAddressString(addressElement);

			if (result != null) return result;

			_logger.LogWarning("Nominatim nie zwrócił adresu dla podanych współrzędnych");
			return null;
		}
		catch (HttpRequestException ex)
		{
			_logger.LogError(ex, "Błąd podczas komunikacji z API Nominatim");
			return null;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Nieoczekiwany błąd podczas pobierania adresu");
			return null;
		}
	}

	// Build custom address format from components
	private static string? BuildAddressString(JsonElement address)
	{
		address.TryGetProperty("road", out var road);
		address.TryGetProperty("postcode", out var postalCode);
		address.TryGetProperty("city", out var city);

		return $"{road}, {postalCode} {city}";
	}
}