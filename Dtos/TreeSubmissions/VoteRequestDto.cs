using DrzewaAPI.Models.Enums;

namespace DrzewaAPI.Dtos.TreeSubmissions;

public record VoteRequestDto
{
	public VoteType Type { get; set; }
}
