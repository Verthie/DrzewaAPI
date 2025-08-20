using System;
using DrzewaAPI.Models.Enums;

namespace DrzewaAPI.Models;

public class TreeSpecies
{
	public Guid Id { get; set; }
	public required string PolishName { get; set; }
	public required string LatinName { get; set; }
	public required string Family { get; set; }
	public string? Description { get; set; }
	public List<string>? IdentificationGuide { get; set; }

	// Navigation Properties
	public ICollection<TreeSpeciesImage> Images { get; set; } = new List<TreeSpeciesImage>();
	public ICollection<TreeSubmission> TreeSubmissions { get; set; } = new List<TreeSubmission>();
}
