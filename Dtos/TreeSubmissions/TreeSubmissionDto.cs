using DrzewaAPI.Models.Enums;
using DrzewaAPI.Models.ValueObjects;

namespace DrzewaAPI.Dtos.TreeSubmissions;

public record TreeSubmissionDto
{
	public Guid Id { get; init; }
	public required string Species { get; init; }
	public required string SpeciesLatin { get; init; }
	public required Location Location { get; init; }
	public required int Circumference { get; init; }
	public double? Height { get; set; }
	public required string Condition { get; init; }
	public bool IsAlive { get; init; } = true;
	public int? EstimatedAge { get; set; }
	public string? Description { get; set; }
	public required List<string> Images { get; set; }
	public bool IsMonument { get; init; } = false;
	public SubmissionStatus Status { get; init; }
	public DateTime SubmissionDate { get; init; }
	public DateTime? ApprovalDate { get; set; }
	public required VotesCount Votes { get; init; }
}
