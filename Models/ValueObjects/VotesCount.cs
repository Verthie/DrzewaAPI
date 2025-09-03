namespace DrzewaAPI.Models.ValueObjects;

public record VotesCount
{
	public int Like { get; set; } = 0;
	public int Dislike { get; set; } = 0;
}
