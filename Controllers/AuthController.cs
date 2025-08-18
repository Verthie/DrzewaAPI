using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using DrzewaAPI.Dtos.Auth;
using DrzewaAPI.Services;

namespace DrzewaAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
	private readonly IAuthService _authService;
	private readonly ILogger<AuthController> _logger;

	public AuthController(IAuthService authService, ILogger<AuthController> logger)
	{
		_authService = authService;
		_logger = logger;
	}

	[HttpPost("register")]
	public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
	{
		try
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var result = await _authService.RegisterAsync(registerDto);

			if (result == null)
			{
				return BadRequest(new ErrorResponseDto { Error = "Email już istnieje" });
			}

			return StatusCode(201, result);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Błąd podczas rejestracji");
			return StatusCode(500, new ErrorResponseDto { Error = "Wystąpił błąd serwera" });
		}
	}

	[HttpPost("login")]
	public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
	{
		try
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var result = await _authService.LoginAsync(loginDto);

			if (result == null)
			{
				return Unauthorized(new ErrorResponseDto { Error = "Nieprawidłowe dane logowania" });
			}

			return Ok(result);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Błąd podczas logowania");
			return StatusCode(500, new ErrorResponseDto { Error = "Wystąpił błąd serwera" });
		}
	}
}
