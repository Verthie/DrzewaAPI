namespace DrzewaAPI.Models.ValueObjects;

public record SeasonalChanges
{
	public required string Spring { get; set; }
	public required string Summer { get; set; }
	public required string Autumn { get; set; }
	public required string Winter { get; set; }
}
