using System;
using DrzewaAPI.Models.Enums;

namespace DrzewaAPI.Models;

public class ApplicationField
{
	public string Name { get; set; } = string.Empty;
	public string Label { get; set; } = string.Empty;
	public ApplicationFieldType Type { get; set; }
	public bool IsRequired { get; set; }
	public string? DefaultValue { get; set; }
	public string? Placeholder { get; set; }
	public List<string>? Options { get; set; } // For select/radio fields
	public ApplicationFieldValidation? Validation { get; set; }
	public string? HelpText { get; set; }
	public int Order { get; set; }
}
