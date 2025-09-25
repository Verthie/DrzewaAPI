namespace DrzewaAPI.Dtos;

public record ErrorResponseDto
{
	public string Error { get; set; } = string.Empty;
	public string Code { get; set; } = string.Empty;
	public Exception? InnerException { get; set; }
	public string Details { get; set; } = string.Empty;
	public Dictionary<string, string[]> Data { get; set; } = new Dictionary<string, string[]>();
	public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
