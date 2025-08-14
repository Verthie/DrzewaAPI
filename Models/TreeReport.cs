using System;
using DrzewaAPI.Utils;

namespace DrzewaAPI.Models;

public class TreeReport
{
	public Guid Id { get; set; }
	public Guid UserId { get; set; }
	public double Latitude { get; set; }
	public double Longitude { get; set; }
	public string? LocationDescription { get; set; }
	public Guid SpeciesId { get; set; }
	public int Circumference { get; set; } // Pierśnica
	public bool IsAlive { get; set; }
	public int? EstimatedAge { get; set; }
	public string? Description { get; set; }
	public string? Legend { get; set; }
	public bool IsNatureMonument { get; set; } = false;
	public ReportStatus Status { get; set; } = ReportStatus.Pending;
	public DateTime CreatedAt { get; set; }
	// public DateTime? UpdatedAt { get; set; }
	public bool IsVerified { get; set; } = false;
	public int VotesCount { get; set; } = 0;
	public int CommentsCount { get; set; } = 0;

	// Navigation Properties
	public User? User { get; set; }
	public TreeSpecies? Species { get; set; }
	public TreeConditionTags? ConditionTags { get; set; }
	public Application? Application { get; set; }
	public required ICollection<TreeReportAttachment> Attachments { get; set; }
	public ICollection<Comment>? Comments { get; set; }
	public ICollection<Vote>? Votes { get; set; }
}
