using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection.PortableExecutable;
using DrzewaAPI.Models.Enums;
using DrzewaAPI.Models.ValueObjects;

namespace DrzewaAPI.Models;

public class TreeSpecies
{
	public Guid Id { get; set; }
	[Required]
	public required string PolishName { get; set; }
	[Required]
	public required string LatinName { get; set; }
	[Required]
	public required string Family { get; set; }
	public string? Description { get; set; }
	public List<string>? IdentificationGuide { get; set; }
	public SeasonalChanges? SeasonalChanges { get; set; }
	public Traits? Traits { get; set; }

	// Navigation Properties
	public ICollection<TreeSpeciesImage> Images { get; set; } = new List<TreeSpeciesImage>();
	public ICollection<TreeSubmission> TreeSubmissions { get; set; } = new List<TreeSubmission>();
}
