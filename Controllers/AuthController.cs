using System.Security.Claims;
using DrzewaAPI.Dtos;
using DrzewaAPI.Dtos.Auth;
using DrzewaAPI.Services;
using DrzewaAPI.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DrzewaAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService _authService) : ControllerBase
{

	[HttpPost("register")]
	public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
	{
		ValidationHelpers.ValidateModelState(ModelState);

		AuthResponseDto result = await _authService.RegisterAsync(registerDto);

		return StatusCode(201, result);
	}

	[HttpPost("login")]
	public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
	{
		ValidationHelpers.ValidateModelState(ModelState);

		AuthResponseDto result = await _authService.LoginAsync(loginDto);

		return Ok(result);
	}

	[HttpPost("refresh-token")]
	public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto request)
	{
		ValidationHelpers.ValidateModelState(ModelState);

		if (string.IsNullOrEmpty(request.RefreshToken))
			return BadRequest("Refresh token is required.");

		AuthResponseDto result = await _authService.RefreshTokenAsync(request.RefreshToken);

		return Ok(result);
	}
}
