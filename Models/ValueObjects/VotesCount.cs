namespace DrzewaAPI.Models.ValueObjects;

public record VotesCount
{
	public int Approve { get; set; } = 0;
	public int Reject { get; set; } = 0;
}
