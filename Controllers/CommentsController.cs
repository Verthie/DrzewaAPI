using DrzewaAPI.Dtos.Auth;
using DrzewaAPI.Dtos.Comment;
using DrzewaAPI.Dtos.TreeSubmissions;
using DrzewaAPI.Extensions;
using DrzewaAPI.Models.ValueObjects;
using DrzewaAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DrzewaAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CommentsController(ICommentService _commentService, ILogger<CommentsController> _logger) : ControllerBase
{
	[HttpGet]
	public async Task<IActionResult> GetComments()
	{
		try
		{
			List<CommentDto> comments = await _commentService.GetCommentsAsync();

			return Ok(comments);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Błąd podczas pobierania listy komentarzy");
			return StatusCode(500, new ErrorResponseDto { Error = "Wystąpił błąd serwera" });
		}
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> GetCommentById(string id)
	{
		try
		{
			if (!Guid.TryParse(id, out var commentId))
			{
				return BadRequest(new ErrorResponseDto { Error = "Nieprawidłowy format ID" });
			}

			CommentDto? comment = await _commentService.GetCommentByIdAsync(commentId);

			return Ok(comment);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Błąd podczas pobierania komentarza o Id: {CommentId}", id);
			return StatusCode(500, new ErrorResponseDto { Error = "Wystąpił błąd serwera" });
		}
	}

	[HttpGet("tree/{treeId}")]
	public async Task<IActionResult> GetTreeComments(string treeId)
	{
		try
		{
			if (!Guid.TryParse(treeId, out var treeGuid))
			{
				return BadRequest(new ErrorResponseDto { Error = "Nieprawidłowy format ID" });
			}

			List<CommentDto> comments = await _commentService.GetTreeCommentsAsync(treeGuid);

			return Ok(comments);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Błąd podczas pobierania listy komentarzy dla drzewa o Id: {TreeId}", treeId);
			return StatusCode(500, new ErrorResponseDto { Error = "Wystąpił błąd serwera" });
		}
	}

	[Authorize]
	[HttpPost("tree/{treeId}")]
	public async Task<IActionResult> CreateComment([FromBody] CreateCommentDto request, string treeId)
	{
		try
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			if (!Guid.TryParse(treeId, out var treeGuid))
			{
				return BadRequest(new ErrorResponseDto { Error = "Nieprawidłowy format ID" });
			}

			Guid userId = User.GetCurrentUserId();

			var result = await _commentService.CreateCommentAsync(request, userId, treeGuid);

			if (result == null)
			{
				return NotFound(new ErrorResponseDto { Error = "Nie udało się utworzyć komentarza" });
			}

			return CreatedAtAction(nameof(GetCommentById), new { id = result.Id }, result);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Błąd podczas tworzenia komentarza dla drzewa o Id: {TreeId}", treeId);
			return StatusCode(500, new ErrorResponseDto { Error = "Wystąpił błąd serwera" });
		}
	}

	[Authorize]
	[HttpPut("{id}/vote")]
	public async Task<ActionResult<VotesCount>> UpdateVote(string id, [FromBody] VoteRequestDto request)
	{
		try
		{
			if (!Guid.TryParse(id, out var commentId))
			{
				return BadRequest(new ErrorResponseDto { Error = "Nieprawidłowy format ID" });
			}

			Guid userId = User.GetCurrentUserId();

			if (request?.Type == null)
			{
				return BadRequest(new ErrorResponseDto { Error = "Typ musi zostać podany" });
			}

			var result = await _commentService.SetVoteAsync(commentId, userId, request.Type);

			if (result == null)
			{
				return NotFound(new ErrorResponseDto { Error = "Nie udało się oddać głosu" });
			}

			return Ok(result);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Błąd podczas oddawania głosu na komentarz: {CommentId}", id);
			return StatusCode(500, new ErrorResponseDto { Error = "Wystąpił błąd serwera" });
		}
	}

	[Authorize]
	[HttpDelete("{id}/vote")]
	public async Task<ActionResult<VotesCount>> DeleteVote(string id)
	{
		try
		{
			if (!Guid.TryParse(id, out var commentId))
			{
				return BadRequest(new ErrorResponseDto { Error = "Nieprawidłowy format ID" });
			}

			Guid userId = User.GetCurrentUserId();

			var result = await _commentService.SetVoteAsync(commentId, userId, type: null);

			if (result == null)
			{
				return NotFound(new ErrorResponseDto { Error = "Nie udało się usunąć głosu" });
			}

			return Ok(result);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Błąd podczas usuwania głosu z komentarza: {CommentId}", id);
			return StatusCode(500, new ErrorResponseDto { Error = "Wystąpił błąd serwera" });
		}
	}
}

