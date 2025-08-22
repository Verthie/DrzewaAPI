using System.Security.Claims;
using DrzewaAPI.Dtos.Auth;
using DrzewaAPI.Dtos.User;
using DrzewaAPI.Models.Enums;
using DrzewaAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DrzewaAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController(IUserService _userService, ILogger<UsersController> _logger) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        try
        {
            List<UserDto> users = await _userService.GetAllUsersAsync();

            return Ok(users);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Błąd podczas pobierania listy użytkowników");
            return StatusCode(500, new ErrorResponseDto { Error = "Wystąpił błąd serwera" });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(string id)
    {
        try
        {
            if (!Guid.TryParse(id, out var userId))
            {
                return BadRequest(new ErrorResponseDto { Error = "Nieprawidłowy format ID" });
            }

            UserDto? user = await _userService.GetUserByIdAsync(userId);

            if (user == null)
            {
                return NotFound(new ErrorResponseDto { Error = "Użytkownik nie został znaleziony" });
            }

            return Ok(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Błąd podczas pobierania użytkownika: {UserId}", id);
            return StatusCode(500, new ErrorResponseDto { Error = "Wystąpił błąd serwera" });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(string id, [FromBody] UpdateUserDto updateDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!Guid.TryParse(id, out Guid userId))
            {
                return BadRequest(new ErrorResponseDto { Error = "Nieprawidłowy format ID" });
            }

            // Check if user can edit this profile
            string? currentUserIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            string? currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (!Guid.TryParse(currentUserIdClaim, out Guid currentUserId))
            {
                return Unauthorized(new ErrorResponseDto { Error = "Nieprawidłowy token" });
            }

            // User can edit only his own profile, unless he is a moderator
            if (currentUserId != userId && currentUserRole != UserRole.Moderator.ToString())
            {
                return Forbid();
            }

            UserDto? updatedUser = await _userService.UpdateUserAsync(userId, updateDto);

            if (updatedUser == null)
            {
                return NotFound(new ErrorResponseDto { Error = "Użytkownik nie został znaleziony" });
            }

            return Ok(updatedUser);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Błąd podczas aktualizacji użytkownika: {UserId}", id);
            return StatusCode(500, new ErrorResponseDto { Error = "Wystąpił błąd serwera" });
        }
    }
}
