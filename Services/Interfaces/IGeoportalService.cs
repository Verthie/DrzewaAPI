using System;
using DrzewaAPI.Models;

namespace DrzewaAPI.Services;

public interface IGeoportalService
{
	Task<Plot?> GetPlotByLocationAsync(double latitude, double longitude);
}
