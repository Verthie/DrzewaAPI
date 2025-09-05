using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DrzewaAPI.Models.Enums;
using DrzewaAPI.Models.ValueObjects;
using DrzewaAPI.Utils;

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
	public required List<string> Images { get; set; }
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
