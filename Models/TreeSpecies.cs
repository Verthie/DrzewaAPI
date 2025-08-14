using System;
using DrzewaAPI.Utils;

namespace DrzewaAPI.Models;

public class TreeSpecies
{
	public Guid Id { get; set; }
	public required string PolishName { get; set; }
	public required string LatinName { get; set; }
	public required SpeciesCategory Category { get; set; } //	Deciduous = 0, Coniferous = 1
	public string? Description { get; set; }
	public string? IdentificationGuide { get; set; }
	public string? SeasonalChanges { get; set; }

	// Navigation Properties
	public required ICollection<TreeSpeciesImage> Images { get; set; }
	public ICollection<TreeReport>? TreeReports { get; set; }
}
