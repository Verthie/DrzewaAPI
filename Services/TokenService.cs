using System;
using System.Security.Cryptography;
using DrzewaAPI.Data;
using DrzewaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DrzewaAPI.Services;

public class TokenService : ITokenService
{
	private readonly ApplicationDbContext _context;
	private readonly ILogger<TokenService> _logger;

	public TokenService(ApplicationDbContext context, ILogger<TokenService> logger)
	{
		_context = context;
		_logger = logger;
	}

	public async Task<string> GenerateEmailVerificationTokenAsync(Guid userId)
	{
		try
		{
			string token = GenerateSecureToken();

			EmailVerificationToken verificationToken = new EmailVerificationToken
			{
				Id = Guid.NewGuid(),
				UserId = userId,
				Token = token,
				TokenType = EmailTokenType.EmailVerification,
				CreatedAt = DateTime.UtcNow,
				ExpiresAt = DateTime.UtcNow.AddHours(24), // 24 hours
				IsUsed = false
			};

			_context.EmailVerificationTokens.Add(verificationToken);
			await _context.SaveChangesAsync();

			return token;
		}
		catch (BusinessException)
		{
			throw;
		}
		catch (DbUpdateException ex)
		{
			_logger.LogError(ex, "Błąd podczas zapisu do bazy danych");
			throw EntityCreationFailedException.ForEmailToken("Błąd podczas zapisu do bazy danych");
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Błąd podczas generowania tokena do resetu hasła");
			throw EntityCreationFailedException.ForEmailToken($"Nieoczekiwany błąd podczas generowania tokena dla użytkownika: {userId}");
		}
	}

	public async Task<string> GeneratePasswordResetTokenAsync(Guid userId)
	{
		try
		{
			// Invalidate existing password reset tokens
			await InvalidateAllUserTokensAsync(userId, EmailTokenType.PasswordReset);

			string token = GenerateSecureToken();

			EmailVerificationToken resetToken = new EmailVerificationToken
			{
				Id = Guid.NewGuid(),
				UserId = userId,
				Token = token,
				TokenType = EmailTokenType.PasswordReset,
				CreatedAt = DateTime.UtcNow,
				ExpiresAt = DateTime.UtcNow.AddHours(1), // 1 hour
				IsUsed = false
			};

			_context.EmailVerificationTokens.Add(resetToken);
			await _context.SaveChangesAsync();

			return token;
		}
		catch (BusinessException)
		{
			throw;
		}
		catch (DbUpdateException ex)
		{
			_logger.LogError(ex, "Błąd podczas zapisu do bazy danych");
			throw EntityCreationFailedException.ForEmailToken("Błąd podczas zapisu do bazy danych");
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Błąd podczas generowania tokena do resetu hasła");
			throw EntityCreationFailedException.ForEmailToken($"Nieoczekiwany błąd podczas generowania tokena dla użytkownika: {userId}");
		}
	}

	public async Task<EmailVerificationToken> ValidateTokenAsync(string token, EmailTokenType tokenType)
	{
		try
		{
			EmailVerificationToken verificationToken = await _context.EmailVerificationTokens
					.Include(t => t.User)
					.FirstOrDefaultAsync(t =>
							t.Token == token &&
							t.TokenType == tokenType &&
							!t.IsUsed &&
							t.ExpiresAt > DateTime.UtcNow)
							?? throw new ServiceException($"Podany token jest nieprawidłowy lub wygasł: {token}", "TOKEN_FETCH_ERROR");

			return verificationToken;
		}
		catch (BusinessException)
		{
			throw;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Błąd podczas pobierania tokena");
			throw new ServiceException($"Nie można pobrać tokena", "TOKEN_FETCH_ERROR");
		}
	}

	public async Task MarkTokenAsUsedAsync(string token, string? ipAddress = null, string? userAgent = null)
	{
		try
		{
			EmailVerificationToken verificationToken = await _context.EmailVerificationTokens
				.FirstOrDefaultAsync(t => t.Token == token)
				?? throw new EntityNotFoundException($"Nie udało się znaleźć podanego tokena: {token}", "TOKEN_FETCH_ERROR");

			if (verificationToken != null)
			{
				verificationToken.IsUsed = true;
				verificationToken.UsedAt = DateTime.UtcNow;
				verificationToken.IpAddress = ipAddress;
				verificationToken.UserAgent = userAgent;

				await _context.SaveChangesAsync();
			}
		}
		catch (BusinessException)
		{
			throw;
		}
		catch (DbUpdateException ex)
		{
			_logger.LogError(ex, "Błąd podczas zapisu do bazy danych");
			throw new ServiceException("Błąd podczas zapisu do bazy danych", "DATABASE_UPDATE_ERROR");
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Błąd podczas oznaczania tokena jako wykorzystany");
			throw new ServiceException($"Nieoczekiwany błąd podczas aktualizacji tokena", "TOKEN_UPDATE_ERROR");
		}
	}

	public async Task InvalidateAllUserTokensAsync(Guid userId, EmailTokenType tokenType)
	{
		try
		{
			List<EmailVerificationToken> tokens = await _context.EmailVerificationTokens
				.Where(t => t.UserId == userId && t.TokenType == tokenType && !t.IsUsed)
				.ToListAsync();

			foreach (EmailVerificationToken token in tokens)
			{
				token.IsUsed = true;
				token.UsedAt = DateTime.UtcNow;
			}

			await _context.SaveChangesAsync();
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, $"Błąd podczas aktualizacji tokenów użytkownika");
			throw new ServiceException($"Nieoczekiwany błąd podczas aktualizacji tokenów użytkownika", "TOKEN_UPDATE_ERROR");
		}
	}

	public async Task CleanupExpiredTokensAsync()
	{
		try
		{
			List<EmailVerificationToken> expiredTokens = await _context.EmailVerificationTokens
				.Where(t => t.ExpiresAt < DateTime.UtcNow)
				.ToListAsync();

			_context.EmailVerificationTokens.RemoveRange(expiredTokens);
			await _context.SaveChangesAsync();

			_logger.LogInformation($"Cleaned up {expiredTokens.Count} expired tokens");
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Błąd podczas aktualizacji tokenów użytkownika");
			throw new ServiceException($"Nieoczekiwany błąd podczas aktualizacji tokenów użytkownika", "TOKEN_UPDATE_ERROR");
		}
	}

	private string GenerateSecureToken()
	{
		byte[] randomBytes = new byte[32];
		using RandomNumberGenerator rng = RandomNumberGenerator.Create();
		rng.GetBytes(randomBytes);
		return Convert.ToBase64String(randomBytes).Replace("+", "-").Replace("/", "_").Replace("=", "");
	}
}
