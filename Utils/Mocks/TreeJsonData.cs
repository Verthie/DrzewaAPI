using System;
using DrzewaAPI.Models.ValueObjects;

namespace DrzewaAPI.Utils.Mocks;

public class TreeJsonData
{
	public required string SpeciesId { get; set; }
	public required Location Location { get; set; }
	public int Circumference { get; set; }
	public int Height { get; set; }
	public string? Condition { get; set; }
	public bool IsAlive { get; set; }
	public int EstimatedAge { get; set; }
	public string? Description { get; set; }
	public required List<string> Images { get; set; }
	public bool IsMonument { get; set; }
}
