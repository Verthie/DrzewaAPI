using System.ComponentModel.DataAnnotations;

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
	public SeasonalChangesDto? SeasonalChanges { get; set; }
	public TraitsDto? Traits { get; set; }

	// Navigation Properties
	public ICollection<TreeSpeciesImage> Images { get; set; } = new List<TreeSpeciesImage>();
	public ICollection<TreeSubmission> TreeSubmissions { get; set; } = new List<TreeSubmission>();
}
