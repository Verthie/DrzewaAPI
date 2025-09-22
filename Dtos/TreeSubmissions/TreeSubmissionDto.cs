using DrzewaAPI.Models.Enums;
using DrzewaAPI.Models.ValueObjects;

namespace DrzewaAPI.Dtos.TreeSubmissions;

public record TreeSubmissionDto
{
	public Guid Id { get; init; }
	public required UserData UserData { get; set; }
	public required string Species { get; init; }
	public required string SpeciesLatin { get; init; }
	public required Location Location { get; init; }
	public required int Circumference { get; init; }
	public double Height { get; set; }
	public required string Condition { get; init; }
	public bool IsAlive { get; init; } = true;
	public int EstimatedAge { get; set; }
	public string? Description { get; set; }
	public List<string> ImageUrls { get; set; } = new();
	public bool IsMonument { get; init; } = false;
	public SubmissionStatus Status { get; init; }
	public DateTime SubmissionDate { get; init; }
	public DateTime? ApprovalDate { get; set; }
	public required VotesCount Votes { get; init; }
	public required int CommentCount { get; init; }
}
