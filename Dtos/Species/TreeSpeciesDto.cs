using System;
using DrzewaAPI.Models.ValueObjects;

namespace DrzewaAPI.Dtos.Species;

public class TreeSpeciesDto
{
	public Guid Id { get; set; }
	public required string PolishName { get; set; }
	public required string LatinName { get; set; }
	public required string Family { get; set; }
	public string? Description { get; set; }
	public List<string>? IdentificationGuide { get; set; }
	public SeasonalChanges? SeasonalChanges { get; set; }
	public List<TreeSpeciesImageDto>? Images { get; set; }
	public Traits? Traits
	{ get; set; }
}
