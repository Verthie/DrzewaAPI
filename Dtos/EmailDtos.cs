using System;

namespace DrzewaAPI.Dtos;

public record SendVerificationEmailDto(string Email);
public record VerifyEmailDto(string Token);
public record EmailVerificationResultDto(
		bool Success,
		string Message,
		DateTime? ExpiresAt = null
);