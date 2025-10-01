using System;

namespace DrzewaAPI.Services;

public interface INominatimService
{
	Task<string?> GetAddressByLocationAsync(double latitude, double longitude);
}
