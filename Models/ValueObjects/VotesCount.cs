namespace DrzewaAPI.Models.ValueObjects;

public record VotesCount
{
	public required int Approve { get; set; }
	public required int Reject { get; set; }
}
