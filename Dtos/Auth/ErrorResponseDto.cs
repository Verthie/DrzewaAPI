using System;

namespace DrzewaAPI.Dtos.Auth;

public record ErrorResponseDto
{
	public required string Error { get; set; }
}
