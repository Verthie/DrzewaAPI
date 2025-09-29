using System;

namespace DrzewaAPI.Models;

public class Plot
{
	public string PlotNumber { get; set; } = string.Empty;
	public string District { get; set; } = string.Empty;
	public string RecordKeepingUnit { get; set; } = string.Empty;
	public string Province { get; set; } = string.Empty;
	public string County { get; set; } = string.Empty;
	public string Commune { get; set; } = string.Empty;
}
