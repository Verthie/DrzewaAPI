using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using DrzewaAPI.Configuration;
using DrzewaAPI.Data;
using DrzewaAPI.Dtos.Auth;
using DrzewaAPI.Dtos.User;
using DrzewaAPI.Models;
using DrzewaAPI.Models.Enums;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace DrzewaAPI.Services;

public class AuthService : IAuthService
{
	private readonly ApplicationDbContext _context;
	private readonly IPasswordHasher<User> _passwordHasher;
	private readonly JwtSettings _jwtSettings;
	private readonly ILogger<AuthService> _logger;

	public AuthService(
			ApplicationDbContext context,
			IPasswordHasher<User> passwordHasher,
			IOptions<JwtSettings> jwtSettings,
			ILogger<AuthService> logger)
	{
		_context = context;
		_passwordHasher = passwordHasher;
		_jwtSettings = jwtSettings.Value;
		_logger = logger;
	}

	public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
	{
		try
		{
			// Check if the user exists
			User? existingUser = await _context.Users
					.FirstOrDefaultAsync(u => u.Email.ToLower() == registerDto.Email.ToLower())
					?? throw EntityNotFoundException.ForAccount(registerDto.Email);

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

			// Generate JWT Token
			var token = GenerateJwtToken(user);

			_logger.LogInformation("Nowy użytkownik zarejestrowany: {Email}", user.Email);

			return new AuthResponseDto
			{
				User = MapToUserDto(user),
				Token = token
			};
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

	public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
	{
		try
		{
			// Find user
			User user = await _context.Users
					.FirstOrDefaultAsync(u => u.Email.ToLower() == loginDto.Email.ToLower())
					?? throw EntityNotFoundException.ForAccount(loginDto.Email);

			// Verify password
			var passwordResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginDto.Password);
			if (passwordResult == PasswordVerificationResult.Failed) throw AccountException.ForIncorrectPassword();

			// Updating last login time
			// user.LastLoginAt = DateTime.UtcNow;

			// await _context.SaveChangesAsync();

			// Generate JWT Token
			var token = GenerateJwtToken(user);

			_logger.LogInformation("Użytkownik zalogowany: {Email}", user.Email);

			return new AuthResponseDto
			{
				User = MapToUserDto(user),
				Token = token
			};
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

	private string GenerateJwtToken(User user)
	{
		var tokenHandler = new JwtSecurityTokenHandler();
		var key = Encoding.UTF8.GetBytes(_jwtSettings.SecretKey);

		var claims = new List<Claim>
		{
				new(ClaimTypes.NameIdentifier, user.Id.ToString()),
				new(ClaimTypes.Name, user.FullName),
				new(ClaimTypes.Email, user.Email),
				new(ClaimTypes.Role, user.Role.ToString()),

				new("userId", user.Id.ToString()),
				new("email", user.Email),
				new("name", user.FullName)
		};

		var tokenDescriptor = new SecurityTokenDescriptor
		{
			Subject = new ClaimsIdentity(claims),
			Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes),
			SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
			Issuer = _jwtSettings.Issuer,
			Audience = _jwtSettings.Audience
		};

		var token = tokenHandler.CreateToken(tokenDescriptor);
		return tokenHandler.WriteToken(token);
	}

	private static UserDto MapToUserDto(User user)
	{
		return new UserDto
		{
			Id = user.Id,
			Email = user.Email,
			Name = user.FullName,
			Avatar = user.Avatar,
			RegistrationDate = user.RegistrationDate,
		};
	}
}