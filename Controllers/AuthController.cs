using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using DrzewaAPI.Dtos.Auth;
using DrzewaAPI.Services;
using DrzewaAPI.Utils;

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
}
