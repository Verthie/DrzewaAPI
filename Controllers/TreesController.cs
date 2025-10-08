using System.Security.Claims;
using DrzewaAPI.Extensions;
using DrzewaAPI.Models;
using DrzewaAPI.Services;
using DrzewaAPI.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace DrzewaAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TreesController(ITreeService _treeService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetTreeSubmissions()
    {
        List<TreeSubmissionDto> trees = await _treeService.GetTreeSubmissionsAsync();

        return Ok(trees);
    }

    [Authorize]
    [HttpGet("user")]
    public async Task<IActionResult> GetCurrentUserTreeSubmissions()
    {
        Guid userId = User.GetCurrentUserId();

        List<TreeSubmissionDto> trees = await _treeService.GetCurrentUserTreeSubmissionsAsync(userId);

        return Ok(trees);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTreeSubmissionById(string id)
    {
        Guid treeId = ValidationHelpers.ValidateAndParseId(id);

        TreeSubmissionDto tree = await _treeService.GetTreeSubmissionByIdAsync(treeId);

        return Ok(tree);
    }

    // TODO Add screenshot to submissions functionality
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateTreeSubmission([FromForm] CreateTreeSubmissionDto request, IFormFileCollection images)
    {
        ValidationHelpers.ValidateModelState(ModelState);

        if (images == null || images.Count == 0)
        {
            return BadRequest("At least one image needs to be provided");
        }

        // Validate images
        if (images.Count > 6) // Limit to 6 images
        {
            return BadRequest("Maximum 6 images allowed");
        }

        Guid userId = User.GetCurrentUserId();
        TreeSubmissionDto result = await _treeService.CreateTreeSubmissionAsync(request, images, userId);

        return CreatedAtAction(nameof(GetTreeSubmissionById), new { id = result.Id }, result);
    }

    // TODO Update screenshot in submissions functionality 
    [Authorize]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateTreeSubmission(Guid id, [FromForm] UpdateTreeSubmissionDto request, IFormFileCollection? images)
    {
        ValidationHelpers.ValidateModelState(ModelState);

        // Validate images if provided
        if (images?.Count > 6) // Limit to 6 images
        {
            return BadRequest("Maximum 6 images allowed");
        }

        string? currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;
        bool isModerator = currentUserRole == UserRole.Moderator.ToString();

        Guid userId = User.GetCurrentUserId();

        TreeSubmissionDto result = await _treeService.UpdateTreeSubmissionAsync(id, request, images, userId, isModerator);
        return Ok(result);
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTreeSubmission(string id)
    {
        Guid treeId = ValidationHelpers.ValidateAndParseId(id);
        Guid userId = User.GetCurrentUserId();

        string? currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;
        bool isModerator = currentUserRole == UserRole.Moderator.ToString();

        await _treeService.DeleteTreeSubmissionAsync(treeId, userId, isModerator);
        return NoContent();
    }

    [Authorize(Roles = "Moderator")]
    [HttpPut("{id}/approve")]
    public async Task<IActionResult> ApproveTree(string id)
    {
        Guid treeId = ValidationHelpers.ValidateAndParseId(id);
        await _treeService.ApproveTreeAsync(treeId);
        return NoContent();
    }

    [Authorize]
    [HttpPut("{id}/vote")]
    public async Task<ActionResult<int>> UpdateVote(string id)
    {
        Guid treeId = ValidationHelpers.ValidateAndParseId(id);
        Guid userId = User.GetCurrentUserId();

        int voteCount = await _treeService.SetVoteAsync(treeId, userId, vote: true);

        return Ok(voteCount);
    }

    [Authorize]
    [HttpDelete("{id}/vote")]
    public async Task<ActionResult<int>> DeleteVote(string id)
    {
        Guid treeId = ValidationHelpers.ValidateAndParseId(id);
        Guid userId = User.GetCurrentUserId();

        int voteCount = await _treeService.SetVoteAsync(treeId, userId, vote: false);

        return Ok(voteCount);
    }

}
