using System.Security.Claims;
using DrzewaAPI.Extensions;
using DrzewaAPI.Services;
using DrzewaAPI.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DrzewaAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
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

    [HttpPut("data")]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDto updateDto)
    {
        ValidationHelpers.ValidateModelState(ModelState);

        updateDto.UserId ??= User.GetCurrentUserId().ToString();

        Guid userId = ValidationHelpers.ValidateAndParseId(updateDto.UserId);
        Guid currentUserId = User.GetCurrentUserId();

        string? currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;
        bool isModerator = currentUserRole == UserRole.Moderator.ToString();

        UserDto updatedUser = await _userService.UpdateUserAsync(currentUserId, userId, updateDto, isModerator);

        return Ok(updatedUser);
    }

    [HttpPut("password")]
    public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordDto updatePasswordDto)
    {
        ValidationHelpers.ValidateModelState(ModelState);

        updatePasswordDto.UserId ??= User.GetCurrentUserId().ToString();

        Guid userId = ValidationHelpers.ValidateAndParseId(updatePasswordDto.UserId);
        Guid currentUserId = User.GetCurrentUserId();

        string? currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;
        bool isModerator = currentUserRole == UserRole.Moderator.ToString();

        await _userService.UpdatePasswordAsync(currentUserId, userId, updatePasswordDto, isModerator);

        return NoContent();
    }

    // TODO Add User Deletion
}
