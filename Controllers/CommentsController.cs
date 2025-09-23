using System.Security.Claims;
using DrzewaAPI.Extensions;
using DrzewaAPI.Services;
using DrzewaAPI.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DrzewaAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CommentsController(ICommentService _commentService) : ControllerBase
{
	[HttpGet]
	public async Task<IActionResult> GetComments()
	{
		List<CommentDto> comments = await _commentService.GetCommentsAsync();

		return Ok(comments);
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> GetCommentById(string id)
	{
		Guid commentId = ValidationHelpers.ValidateAndParseId(id);

		CommentDto comment = await _commentService.GetCommentByIdAsync(commentId);

		return Ok(comment);
	}

	[HttpGet("tree/{treeId}")]
	public async Task<IActionResult> GetTreeComments(string treeId)
	{
		Guid treeGuid = ValidationHelpers.ValidateAndParseId(treeId);

		List<CommentDto> comments = await _commentService.GetTreeCommentsAsync(treeGuid);

		return Ok(comments);
	}

	[Authorize]
	[HttpPost("tree/{treeId}")]
	public async Task<IActionResult> CreateComment([FromBody] CreateCommentDto request, string treeId)
	{
		ValidationHelpers.ValidateModelState(ModelState);

		Guid treeGuid = ValidationHelpers.ValidateAndParseId(treeId);
		Guid userId = User.GetCurrentUserId();

		CommentDto result = await _commentService.CreateCommentAsync(request, userId, treeGuid);

		return CreatedAtAction(nameof(GetCommentById), new { id = result.Id }, result);
	}

	[Authorize]
	[HttpPut("{id}/vote")]
	public async Task<ActionResult<VotesDto>> UpdateVote(string id, [FromBody] VoteRequestDto request)
	{
		Guid commentId = ValidationHelpers.ValidateAndParseId(id);
		Guid userId = User.GetCurrentUserId();

		VotesDto result = await _commentService.SetVoteAsync(commentId, userId, request.Type);

		return Ok(result);
	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> DeleteTreeSubmission(string id)
	{
		Guid commentId = ValidationHelpers.ValidateAndParseId(id);
		Guid userId = User.GetCurrentUserId();

		string? currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;
		bool isModerator = currentUserRole == UserRole.Moderator.ToString();

		await _commentService.DeleteCommentAsync(commentId, userId, isModerator);

		return NoContent();
	}

	[Authorize]
	[HttpDelete("{id}/vote")]
	public async Task<ActionResult<VotesDto>> DeleteVote(string id)
	{
		Guid commentId = ValidationHelpers.ValidateAndParseId(id);
		Guid userId = User.GetCurrentUserId();

		VotesDto result = await _commentService.SetVoteAsync(commentId, userId, type: null);

		return Ok(result);
	}
}

