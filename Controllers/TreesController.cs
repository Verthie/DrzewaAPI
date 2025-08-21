using System.Security.Claims;
using DrzewaAPI.Data;
using DrzewaAPI.Dtos.Auth;
using DrzewaAPI.Dtos.TreeSubmissions;
using DrzewaAPI.Models;
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

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTreeSubmission(string id)
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

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateTreeSubmission([FromBody] CreateTreeSubmissionDto request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Forbid();

            if (!Guid.TryParse(userIdClaim, out var userGuid))
                return Forbid();

            var result = await _treeService.CreateTreeSubmissionAsync(request, userGuid);

            if (result == null)
            {
                return NotFound(new ErrorResponseDto { Error = "Nie udało się utworzyć drzewa" });
            }

            return CreatedAtAction(nameof(GetTreeSubmission), new { id = result.Id }, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Błąd podczas tworzenia drzewa");
            return StatusCode(500, new ErrorResponseDto { Error = "Wystąpił błąd serwera" });
        }
    }

}
