using System;

namespace DrzewaAPI.Models;

public class SpeciesAdditionRequest
{
	public required Guid Id { get; set; }
	public required Guid UserId { get; set; }
	public required string PolishName { get; set; }
	public required string LatinName { get; set; }
	public string? Description { get; set; }
	public string? IdentificationGuide { get; set; }
	public string? SeasonalChanges { get; set; }
	public DateTime CreatedAt { get; set; }

	// Navigation Properties
	public User? User { get; set; }
}
