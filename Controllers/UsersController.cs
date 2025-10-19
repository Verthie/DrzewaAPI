using System.Security.Claims;
using DrzewaAPI.Extensions;
using DrzewaAPI.Services;
using DrzewaAPI.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DrzewaAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UsersController(IUserService _userService) : ControllerBase
{
    [Authorize(Roles = "Moderator")]
    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        List<UserDto> users = await _userService.GetAllUsersAsync();

        return Ok(users);
    }

    [Authorize(Roles = "Moderator")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(string id)
    {
        Guid userId = ValidationHelpers.ValidateAndParseId(id);

        UserDto user = await _userService.GetUserByIdAsync<UserDto>(userId);

        return Ok(user);
    }

    [HttpGet("current")]
    public async Task<IActionResult> GetCurrentUser()
    {
        Guid userId = User.GetCurrentUserId();

        CurrentUserDto user = await _userService.GetUserByIdAsync<CurrentUserDto>(userId);

        return Ok(user);
    }

    [HttpPut("data/{userId?}")]
    public async Task<IActionResult> UpdateUser([FromForm] UpdateUserDto req, string? userId)
    {
        // ValidationHelpers.ValidateModelState(ModelState);

        userId ??= User.GetCurrentUserId().ToString();

        Guid userGuid = ValidationHelpers.ValidateAndParseId(userId);
        Guid currentUserId = User.GetCurrentUserId();

        string? currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;
        bool isModerator = currentUserRole == UserRole.Moderator.ToString();

        UserDto updatedUser = await _userService.UpdateUserAsync(currentUserId, userGuid, req, isModerator);

        return Ok(updatedUser);
    }

    /// <summary>
    /// Allows user to update password when he is currently logged in a session
    /// </summary>
    [HttpPut("password")]
    public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordDto req)
    {
        ValidationHelpers.ValidateModelState(ModelState);

        Guid userId = User.GetCurrentUserId();

        await _userService.UpdatePasswordAsync(userId, req.ConfirmPassword);

        return NoContent();
    }

    [Authorize]
    [HttpDelete("{userId?}")]
    public async Task<IActionResult> DeleteUser(string? userId)
    {
        Guid currentUserId = User.GetCurrentUserId();
        userId ??= User.GetCurrentUserId().ToString();

        Guid userGuid = ValidationHelpers.ValidateAndParseId(userId);

        string? currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;
        bool isModerator = currentUserRole == UserRole.Moderator.ToString();

        await _userService.DeleteUserAsync(currentUserId, userGuid, isModerator);

        return NoContent();
    }
}
