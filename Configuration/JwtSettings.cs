using System;

namespace DrzewaAPI.Configuration;

public class JwtSettings
{
	public const string SectionName = "JwtSettings";

	public string SecretKey { get; init; } = string.Empty;
	public string Issuer { get; init; } = string.Empty;
	public string Audience { get; init; } = string.Empty;
	public int ExpirationMinutes { get; init; } = 60;
}