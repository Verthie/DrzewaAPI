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

    [HttpPut("data")]
    public async Task<IActionResult> UpdateUser([FromForm] UpdateUserDto req, IFormFile image)
    {
        ValidationHelpers.ValidateModelState(ModelState);

        if (image == null)
        {
            return BadRequest("An image needs to be provided");
        }

        req.UserId ??= User.GetCurrentUserId().ToString();

        Guid userId = ValidationHelpers.ValidateAndParseId(req.UserId);
        Guid currentUserId = User.GetCurrentUserId();

        string? currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;
        bool isModerator = currentUserRole == UserRole.Moderator.ToString();

        UserDto updatedUser = await _userService.UpdateUserAsync(currentUserId, userId, req, image, isModerator);

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

    // TODO Add User Deletion
}
