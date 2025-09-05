using System.Security.Claims;
using DrzewaAPI.Data;
using DrzewaAPI.Dtos.Auth;
using DrzewaAPI.Dtos.TreeSubmissions;
using DrzewaAPI.Extensions;
using DrzewaAPI.Models;
using DrzewaAPI.Models.ValueObjects;
using DrzewaAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DrzewaAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TreesController(ITreeService _treeService, ILogger<TreesController> _logger) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetTreeSubmissions()
    {
        try
        {
            List<TreeSubmissionDto> trees = await _treeService.GetTreeSubmissionsAsync();

            return Ok(trees);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Błąd podczas pobierania listy drzew");
            return StatusCode(500, new ErrorResponseDto { Error = "Wystąpił błąd serwera" });
        }
    }

    [HttpGet("user")]
    public async Task<IActionResult> GetCurrentUserTreeSubmissions()
    {
        try
        {
            Guid userId = User.GetCurrentUserId();

            List<TreeSubmissionDto> trees = await _treeService.GetCurrentUserTreeSubmissionsAsync(userId);

            return Ok(trees);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Błąd podczas pobierania listy drzew");
            return StatusCode(500, new ErrorResponseDto { Error = "Wystąpił błąd serwera" });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTreeSubmissionById(string id)
    {
        try
        {
            if (!Guid.TryParse(id, out var treeId))
            {
                return BadRequest(new ErrorResponseDto { Error = "Nieprawidłowy format ID" });
            }

            TreeSubmissionDto? tree = await _treeService.GetTreeSubmissionByIdAsync(treeId);

            if (tree == null)
            {
                return NotFound(new ErrorResponseDto { Error = "Drzewo nie zostało znalezione" });
            }

            return Ok(tree);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Błąd podczas pobierania drzewa: {TreeId}", id);
            return StatusCode(500, new ErrorResponseDto { Error = "Wystąpił błąd serwera" });
        }
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateTreeSubmission([FromBody] CreateTreeSubmissionDto request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Guid userId = User.GetCurrentUserId();

            var result = await _treeService.CreateTreeSubmissionAsync(request, userId);

            if (result == null)
            {
                return NotFound(new ErrorResponseDto { Error = "Nie udało się utworzyć drzewa" });
            }

            return CreatedAtAction(nameof(GetTreeSubmissionById), new { id = result.Id }, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Błąd podczas tworzenia drzewa");
            return StatusCode(500, new ErrorResponseDto { Error = "Wystąpił błąd serwera" });
        }
    }

    // :TODO Approve Tree Functionality for Moderator

    [Authorize]
    [HttpPut("{id}/vote")]
    public async Task<ActionResult<VotesCount>> UpdateVote(string id, [FromBody] VoteRequestDto request)
    {
        try
        {
            if (!Guid.TryParse(id, out var treeId))
            {
                return BadRequest(new ErrorResponseDto { Error = "Nieprawidłowy format ID" });
            }

            Guid userId = User.GetCurrentUserId();

            if (request?.Type == null)
            {
                return BadRequest(new ErrorResponseDto { Error = "Typ musi zostać podany" });
            }

            var result = await _treeService.SetVoteAsync(treeId, userId, request.Type);

            if (result == null)
            {
                return NotFound(new ErrorResponseDto { Error = "Nie udało się oddać głosu" });
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Błąd podczas oddawania głosu na drzewo: {TreeId}", id);
            return StatusCode(500, new ErrorResponseDto { Error = "Wystąpił błąd serwera" });
        }
    }

    [Authorize]
    [HttpDelete("{id}/vote")]
    public async Task<ActionResult<VotesCount>> DeleteVote(string id)
    {
        try
        {
            if (!Guid.TryParse(id, out var treeId))
            {
                return BadRequest(new ErrorResponseDto { Error = "Nieprawidłowy format ID" });
            }

            Guid userId = User.GetCurrentUserId();

            var result = await _treeService.SetVoteAsync(treeId, userId, type: null);

            if (result == null)
            {
                return NotFound(new ErrorResponseDto { Error = "Nie udało się usunąć głosu" });
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Błąd podczas usuwania głosu z drzewa: {TreeId}", id);
            return StatusCode(500, new ErrorResponseDto { Error = "Wystąpił błąd serwera" });
        }
    }
}
