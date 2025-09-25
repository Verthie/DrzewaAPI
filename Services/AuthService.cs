using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using DrzewaAPI.Configuration;
using DrzewaAPI.Data;
using DrzewaAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace DrzewaAPI.Services;

public class AuthService : IAuthService
{
	private readonly ApplicationDbContext _context;
	private readonly IPasswordHasher<User> _passwordHasher;
	private readonly JwtSettings _jwtSettings;
	private readonly IEmailService _emailService;
	private readonly ITokenService _tokenService;
	private readonly ILogger<AuthService> _logger;

	public AuthService(
			ApplicationDbContext context,
			IPasswordHasher<User> passwordHasher,
			IOptions<JwtSettings> jwtSettings,
			IEmailService emailService,
			ITokenService tokenService,
			ILogger<AuthService> logger
			)
	{
		_context = context;
		_passwordHasher = passwordHasher;
		_jwtSettings = jwtSettings.Value;
		_emailService = emailService;
		_tokenService = tokenService;
		_logger = logger;
	}

	public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
	{
		try
		{
			// Check if the user exists
			bool userExists = await _context.Users.AnyAsync(u => u.Email.ToLower() == registerDto.Email.ToLower());
			if (userExists) throw new ServiceException("Konto o podanym mailu już istnieje", "ACCOUNT_ALREADY_EXISTS");

			// Create user
			User user = new User
			{
				Id = Guid.NewGuid(),
				FirstName = registerDto.FirstName.Trim(),
				LastName = registerDto.LastName.Trim(),
				Email = registerDto.Email.ToLower().Trim(),
				Phone = registerDto.Phone?.Trim(),
				RegistrationDate = DateTime.UtcNow,
				Role = UserRole.User,
			};

			// Create password hash
			user.PasswordHash = _passwordHasher.HashPassword(user, registerDto.Password);

			_context.Users.Add(user);
			await _context.SaveChangesAsync();

			var verificationToken = await _tokenService.GenerateEmailVerificationTokenAsync(user.Id);
			await _emailService.SendVerificationEmailAsync(user.Email, $"{user.FirstName} {user.LastName}", verificationToken);

			AuthResponseDto response = new AuthResponseDto
			{
				Message = "Konto zostało utworzone. Sprawdź email w celu weryfikacji konta.",
				RequiresEmailVerification = true
			};

			return response;
		}
		catch (BusinessException)
		{
			throw;
		}
		catch (DbUpdateException ex)
		{
			_logger.LogError(ex, "Błąd bazy danych podczas rejestracji użytkownika");
			throw EntityCreationFailedException.ForUser("Błąd podczas zapisu do bazy danych");
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Nieoczekiwany błąd podczas rejestracji użytkownika");
			throw EntityCreationFailedException.ForUser("Nieoczekiwany błąd systemu");
		}
	}

	public async Task<EmailVerificationResultDto> VerifyEmailAsync(Guid userId)
	{
		try
		{
			User user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId) ?? throw EntityNotFoundException.ForUser(userId);

			if (user.IsEmailVerified)
			{
				return new EmailVerificationResultDto(false, "Konto już zostało zweryfikowane");
			}

			user.IsEmailVerified = true;
			user.EmailVerifiedAt = DateTime.UtcNow;

			await _context.SaveChangesAsync();

			return new EmailVerificationResultDto(true, "Konto zostało pomyślnie zweryfikowane");
		}
		catch (BusinessException)
		{
			throw;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Nieoczekiwany błąd podczas weryfikacji maila użytkownika");
			throw EntityCreationFailedException.ForUser("Nieoczekiwany błąd systemu");
		}
	}

	public async Task<EmailVerificationResultDto> ResendVerificationEmailAsync(string email)
	{
		try
		{
			User user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email)
					?? throw new ServiceException("Konto o podanym mailu nie zostało znalezione", "EMAIL_NOT_FOUND");

			if (user.IsEmailVerified)
			{
				return new EmailVerificationResultDto(false, "Konto już zostało zweryfikowane");
			}

			var verificationToken = await _tokenService.GenerateEmailVerificationTokenAsync(user.Id);
			await _emailService.SendVerificationEmailAsync(email, user.FullName, verificationToken);

			return new EmailVerificationResultDto(true, "Email weryfikacyjny został wysłany", DateTime.UtcNow.AddHours(24));
		}
		catch (BusinessException)
		{
			throw;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Nieoczekiwany błąd podczas weryfikacji maila użytkownika");
			throw EntityCreationFailedException.ForUser("Nieoczekiwany błąd systemu");
		}
	}

	public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
	{
		try
		{
			// Find user
			User user = await _context.Users
					.FirstOrDefaultAsync(u => u.Email.ToLower() == loginDto.Email.ToLower())
					?? throw EntityNotFoundException.ForAccount(loginDto.Email);

			// Verify password
			PasswordVerificationResult passwordResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginDto.Password);
			if (passwordResult == PasswordVerificationResult.Failed) throw AccountException.ForIncorrectPassword();

			// Updating last login time
			// user.LastLoginAt = DateTime.UtcNow;

			// await _context.SaveChangesAsync();
			string accessToken = GenerateJwtToken(user);
			string newRefreshToken = await GenerateRefreshToken(user);

			AuthResponseDto response = new AuthResponseDto
			{
				Message = "Pomyślnie zalogowano",
				AccessToken = accessToken,
				RefreshToken = newRefreshToken,
			};

			_logger.LogInformation("Użytkownik zalogowany: {Email}", user.Email);

			return response;
		}
		catch (BusinessException)
		{
			throw;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Nieoczekiwany błąd podczas logowania użytkownika: {Email}", loginDto.Email);
			throw EntityCreationFailedException.ForUser("Nieoczekiwany błąd systemu");
		}
	}

	public async Task SendPasswordResetEmailAsync(string email)
	{
		try
		{
			User user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email)
				?? throw new ServiceException("Konto o podanym mailu nie zostało znalezione", "EMAIL_NOT_FOUND");

			string resetToken = await _tokenService.GeneratePasswordResetTokenAsync(user.Id);
			await _emailService.SendPasswordResetEmailAsync(email, user.FullName, resetToken);
		}
		catch (BusinessException)
		{
			throw;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Nieoczekiwany błąd podczas wysyłania maila do zmiany hasła użytkownika");
			throw EntityCreationFailedException.ForUser("Nieoczekiwany błąd systemu");
		}
	}

	public async Task<AuthResponseDto> RefreshTokenAsync(string refreshToken)
	{
		try
		{
			RefreshToken token = await _context.RefreshTokens
					.FirstOrDefaultAsync(rt => rt.Token == refreshToken && !rt.IsRevoked) ?? throw new ServiceException("Token odświeżania nie istnieje lub jest nieprawidłowy", "TOKEN_FETCH_ERROR");

			if (token.ExpiresAt < DateTime.UtcNow) throw new ServiceException("Token odświeżania wygasł", "EXPIRED_TOKEN");

			User user = await _context.Users.FirstOrDefaultAsync(u => u.Id == token.UserId) ?? throw EntityNotFoundException.ForUser(token.UserId);

			// Revoke old token
			token.IsRevoked = true;
			_context.RefreshTokens.Update(token);

			// Issue new tokens
			string accessToken = GenerateJwtToken(user);
			string newRefreshToken = await GenerateRefreshToken(user);

			AuthResponseDto response = new AuthResponseDto
			{
				Message = "Odświeżono tokeny",
				AccessToken = accessToken,
				RefreshToken = newRefreshToken,
			};

			await _context.SaveChangesAsync();

			return response;
		}
		catch (BusinessException)
		{
			throw;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Nieoczekiwany błąd podczas odświeżania tokenów");
			throw EntityCreationFailedException.ForUser("Nieoczekiwany błąd systemu");
		}
	}

	private string GenerateJwtToken(User user)
	{
		JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
		SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));

		List<Claim> claims = new List<Claim>
		{
				new(ClaimTypes.NameIdentifier, user.Id.ToString()),
				new(ClaimTypes.Name, user.FullName),
				new(ClaimTypes.Email, user.Email),
				new(ClaimTypes.Role, user.Role.ToString()),

				new("userId", user.Id.ToString()),
				new("email", user.Email),
				new("name", user.FullName)
		};

		SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
		{
			Subject = new ClaimsIdentity(claims),
			Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes),
			SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature),
			Issuer = _jwtSettings.Issuer,
			Audience = _jwtSettings.Audience
		};

		SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
		return tokenHandler.WriteToken(token);
	}

	private async Task<string> GenerateRefreshToken(User user)
	{
		byte[] randomBytes = new byte[64];
		using RandomNumberGenerator rng = RandomNumberGenerator.Create();
		rng.GetBytes(randomBytes);

		string refreshToken = Convert.ToBase64String(randomBytes);

		RefreshToken dbToken = new RefreshToken
		{
			Token = refreshToken,
			UserId = user.Id,
			ExpiresAt = DateTime.UtcNow.AddDays(7)
		};

		_context.RefreshTokens.Add(dbToken);
		await _context.SaveChangesAsync();

		return refreshToken;
	}
}