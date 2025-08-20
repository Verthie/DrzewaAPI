using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DrzewaAPI.Models.Enums;
using DrzewaAPI.Models.ValueObjects;

namespace DrzewaAPI.Models;

public class TreeSubmission
{
	public Guid Id { get; set; }
	public Guid UserId { get; set; }
	public Guid SpeciesId { get; set; }
	[Required]
	public required Location Location { get; set; }
	[Required]
	public required int Circumference { get; set; } // Pierśnica
	public double? Height { get; set; }
	[Required]
	public required string Condition { get; set; }
	public bool IsAlive { get; set; } = true;
	public int? EstimatedAge { get; set; }
	public string? Description { get; set; }
	public bool IsMonument { get; set; } = false;
	public SubmissionStatus Status { get; set; } = SubmissionStatus.Pending;
	public DateTime SubmissionDate { get; set; } = DateTime.UtcNow;
	public DateTime? ApprovalDate { get; set; }

	// Computed Properties
	[NotMapped]
	public int ApprovalVotes => Votes?.Count(v => v.Type == VoteType.Approve) ?? 0;

	[NotMapped]
	public int RejectionVotes => Votes?.Count(v => v.Type == VoteType.Reject) ?? 0;

	// Navigation Properties
	public User User { get; set; } = null!;
	public TreeSpecies Species { get; set; } = null!;
	public ICollection<Vote> Votes { get; set; } = new List<Vote>();
	// public ICollection<TreeConditionTags>? ConditionTags { get; set; }
	// public ICollection<TreeSubmissionAttachment>? Attachments { get; set; }
	// public ICollection<Comment>? Comments { get; set; }
}
