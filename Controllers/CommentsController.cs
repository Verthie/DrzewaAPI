using DrzewaAPI.Dtos.Auth;
using DrzewaAPI.Dtos.Comment;
using DrzewaAPI.Extensions;
using DrzewaAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DrzewaAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CommentsController(ICommentService _commentService, ILogger<CommentsController> _logger) : ControllerBase
{
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

	[HttpGet("trees/{id}")]
	public async Task<IActionResult> GetTreeComments(string id)
	{
		try
		{
			if (!Guid.TryParse(id, out var treeId))
			{
				return BadRequest(new ErrorResponseDto { Error = "Nieprawidłowy format ID" });
			}

			List<CommentDto> comments = await _commentService.GetTreeCommentsAsync(treeId);

			return Ok(comments);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Błąd podczas pobierania listy komentarzy dla drzewa o Id: {TreeId}", id);
			return StatusCode(500, new ErrorResponseDto { Error = "Wystąpił błąd serwera" });
		}
	}

	[Authorize]
	[HttpPost("{id}")]
	public async Task<IActionResult> CreateComment([FromBody] CreateCommentDto request, string id)
	{
		try
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			if (!Guid.TryParse(id, out var treeId))
			{
				return BadRequest(new ErrorResponseDto { Error = "Nieprawidłowy format ID" });
			}

			Guid userId = User.GetCurrentUserId();

			var result = await _commentService.CreateCommentAsync(request, userId, treeId);

			if (result == null)
			{
				return NotFound(new ErrorResponseDto { Error = "Nie udało się utworzyć komentarza" });
			}

			return CreatedAtAction(nameof(GetCommentById), new { id = result.Id }, result);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Błąd podczas tworzenia komentarza");
			return StatusCode(500, new ErrorResponseDto { Error = "Wystąpił błąd serwera" });
		}
	}
}

