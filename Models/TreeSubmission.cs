using System.ComponentModel.DataAnnotations;

namespace DrzewaAPI.Models;

public class TreeSubmission
{
	public Guid Id { get; set; }
	public Guid UserId { get; set; }
	public Guid SpeciesId { get; set; }
	public string? Name { get; set; }
	[Required]
	public required LocationDto Location { get; set; }
	[Required]
	[Range(1, 4000)]
	public required double Circumference { get; set; } // Pierśnica
	[Required]
	[Range(1, 150)]
	public required double Height { get; set; }
	public List<string>? Soil { get; set; }
	public List<string>? Health { get; set; }
	public List<string>? Environment { get; set; }
	public bool IsAlive { get; set; } = true;
	[Required]
	[Range(450, 10000)]
	public required int EstimatedAge { get; set; }
	[Required]
	[Range(1, 200)]
	public required double CrownSpread { get; set; }
	[MaxLength(1500)]
	public string? Description { get; set; }
	[MaxLength(2000)]
	public string? Legend { get; set; }
	public List<string>? Images { get; set; }
	public bool IsMonument { get; set; } = false;
	public string? TreeScreenshotUrl { get; set; }
	public SubmissionStatus Status { get; set; } = SubmissionStatus.Pending;
	public DateTime SubmissionDate { get; set; } = DateTime.UtcNow;
	public DateTime? ApprovalDate { get; set; }

	// Navigation Properties
	public User User { get; set; } = default!;
	public TreeSpecies Species { get; set; } = default!;
	public ICollection<TreeVote> TreeVotes { get; set; } = new List<TreeVote>();
	public ICollection<Application> Applications { get; set; } = new List<Application>();
}
