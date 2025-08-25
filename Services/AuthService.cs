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
	private readonly IUserService _userService;

	public AuthService(
			ApplicationDbContext context,
			IPasswordHasher<User> passwordHasher,
			IOptions<JwtSettings> jwtSettings,
			ILogger<AuthService> logger,
			IUserService userService)
	{
		_context = context;
		_passwordHasher = passwordHasher;
		_jwtSettings = jwtSettings.Value;
		_logger = logger;
		_userService = userService;
	}

	public async Task<AuthResponseDto?> RegisterAsync(RegisterDto registerDto)
	{
		try
		{
			// Check if the user exists
			var existingUser = await _context.Users
					.FirstOrDefaultAsync(u => u.Email.ToLower() == registerDto.Email.ToLower());

			if (existingUser != null) return null;

			// Create user
			var user = new User
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
		catch (Exception ex)
		{
			_logger.LogError(ex, "Błąd podczas rejestracji użytkownika: {Email}", registerDto.Email);
			throw;
		}
	}

	public async Task<AuthResponseDto?> LoginAsync(LoginDto loginDto)
	{
		try
		{
			// Find user
			var user = await _context.Users
					.FirstOrDefaultAsync(u => u.Email.ToLower() == loginDto.Email.ToLower());

			ArgumentNullException.ThrowIfNull(user);

			// Verify password
			var passwordResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginDto.Password);

			if (passwordResult == PasswordVerificationResult.Failed)
			{
				return null;
			}

			// Get the user again with the updated statistics
			user = await _context.Users.FindAsync(user.Id);

			ArgumentNullException.ThrowIfNull(user);

			// Updating last login time 
			// user.LastLoginAt = DateTime.UtcNow;

			await _context.SaveChangesAsync();

			// Generate JWT Token
			var token = GenerateJwtToken(user);

			_logger.LogInformation("Użytkownik zalogowany: {Email}", user.Email);

			return new AuthResponseDto
			{
				User = MapToUserDto(user),
				Token = token
			};
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Błąd podczas logowania użytkownika: {Email}", loginDto.Email);
			throw;
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
			SubmissionsCount = user.TreeSubmissions.Count,
			VerificationsCount = user.Votes.Count(v => v.Type == VoteType.Approve)
		};
	}
}