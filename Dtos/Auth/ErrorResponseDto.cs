using System;

namespace DrzewaAPI.Dtos.Auth;

public record ErrorResponseDto
{
	public string Error { get; set; } = string.Empty;
	public string Code { get; set; } = string.Empty;
	public Exception? InnerException { get; set; }
	public string Details { get; set; } = string.Empty;
	public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
