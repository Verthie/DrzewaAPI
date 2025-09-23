using System.ComponentModel.DataAnnotations;

namespace DrzewaAPI.Models;

public class TreeSubmission
{
	public Guid Id { get; set; }
	public Guid UserId { get; set; }
	public Guid SpeciesId { get; set; }
	[Required]
	public required LocationDto Location { get; set; }
	[Required]
	public required int Circumference { get; set; } // Pierśnica
	[Required]
	public required double Height { get; set; }
	[Required]
	public required string Condition { get; set; }
	public bool IsAlive { get; set; } = true;
	[Required]
	public required int EstimatedAge { get; set; }
	public string? Description { get; set; }
	public List<string>? Images { get; set; }
	public bool IsMonument { get; set; } = false;
	public SubmissionStatus Status { get; set; } = SubmissionStatus.Pending;
	public DateTime SubmissionDate { get; set; } = DateTime.UtcNow;
	public DateTime? ApprovalDate { get; set; }

	// Navigation Properties
	public User User { get; set; } = default!;
	public TreeSpecies Species { get; set; } = default!;
	public ICollection<TreeVote> TreeVotes { get; set; } = new List<TreeVote>();
	public ICollection<Comment> Comments { get; set; } = new List<Comment>();
	public ICollection<Application> Applications { get; set; } = new List<Application>();
}
