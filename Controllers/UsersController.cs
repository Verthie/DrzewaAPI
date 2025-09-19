using System.Security.Claims;
using DrzewaAPI.Dtos.Auth;
using DrzewaAPI.Dtos.User;
using DrzewaAPI.Extensions;
using DrzewaAPI.Models.Enums;
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

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(string id, [FromBody] UpdateUserDto updateDto)
    {
        ValidationHelpers.ValidateModelState(ModelState);

        Guid userId = ValidationHelpers.ValidateAndParseId(id);
        Guid currentUserId = User.GetCurrentUserId();

        string? currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;
        bool isModerator = currentUserRole == UserRole.Moderator.ToString();

        UserDto updatedUser = await _userService.UpdateUserAsync(currentUserId, userId, updateDto, isModerator);

        return Ok(updatedUser);
    }

    // TODO Add User Deletion
    // TODO Password Change
}
