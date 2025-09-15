using System.Security.Claims;
using DrzewaAPI.Data;
using DrzewaAPI.Dtos.Auth;
using DrzewaAPI.Dtos.TreeSubmissions;
using DrzewaAPI.Extensions;
using DrzewaAPI.Models;
using DrzewaAPI.Models.Enums;
using DrzewaAPI.Models.ValueObjects;
using DrzewaAPI.Services;
using DrzewaAPI.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateTreeSubmission([FromBody] CreateTreeSubmissionDto request)
    {
        ValidationHelpers.ValidateModelState(ModelState);

        Guid userId = User.GetCurrentUserId();
        TreeSubmissionDto result = await _treeService.CreateTreeSubmissionAsync(request, userId);

        return CreatedAtAction(nameof(GetTreeSubmissionById), new { id = result.Id }, result);
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
    public async Task<ActionResult<VotesCount>> UpdateVote(string id, [FromBody] VoteRequestDto request)
    {
        Guid treeId = ValidationHelpers.ValidateAndParseId(id);
        Guid userId = User.GetCurrentUserId();

        if (request?.Type == null) throw new TreeVoteFailedException(treeId, "Typ głosu musi zostać podany");

        VotesCount result = await _treeService.SetVoteAsync(treeId, userId, request.Type);

        return Ok(result);
    }

    [Authorize]
    [HttpDelete("{id}/vote")]
    public async Task<ActionResult<VotesCount>> DeleteVote(string id)
    {
        Guid treeId = ValidationHelpers.ValidateAndParseId(id);
        Guid userId = User.GetCurrentUserId();

        VotesCount result = await _treeService.SetVoteAsync(treeId, userId, type: null);

        return Ok(result);
    }
}
