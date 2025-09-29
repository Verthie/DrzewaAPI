using System;
using DrzewaAPI.Models;

namespace DrzewaAPI.Services;

public interface ITokenService
{
	Task<string> GenerateEmailVerificationTokenAsync(Guid userId);
	Task<string> GeneratePasswordResetTokenAsync(Guid userId);
	Task<EmailVerificationToken> ValidateTokenAsync(string token, EmailTokenType tokenType);
	Task MarkTokenAsUsedAsync(string token, string? ipAddress = null, string? userAgent = null);
	Task InvalidateAllUserTokensAsync(Guid userId, EmailTokenType tokenType);
	Task CleanupExpiredTokensAsync();
}
