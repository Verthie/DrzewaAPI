using System;

namespace DrzewaAPI.Models;

public class ApplicationFieldValidation
{
	public int? MinLength { get; set; }
	public int? MaxLength { get; set; }
	public string? Pattern { get; set; }
	public double? Min { get; set; }
	public double? Max { get; set; }
	public string? ValidationMessage { get; set; }
}
