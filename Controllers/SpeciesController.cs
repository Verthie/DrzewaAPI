using DrzewaAPI.Dtos.Auth;
using DrzewaAPI.Dtos.Species;
using DrzewaAPI.Models;
using DrzewaAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DrzewaAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SpeciesController(ISpeciesService _speciesService, ILogger<SpeciesController> _logger) : ControllerBase
{

    [HttpGet]
    public async Task<IActionResult> GetSpecies()
    {
        try
        {
            List<TreeSpeciesDto> species = await _speciesService.GetAllSpeciesAsync();

            return Ok(species);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Błąd podczas pobierania listy gatunków");
            return StatusCode(500, new ErrorResponseDto { Error = "Wystąpił błąd serwera" });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetSpeciesById(string id)
    {
        try
        {
            if (!Guid.TryParse(id, out var speciesId))
            {
                return BadRequest(new ErrorResponseDto { Error = "Nieprawidłowy format ID" });
            }

            TreeSpeciesDto? species = await _speciesService.GetSpeciesByIdAsync(speciesId);

            if (species == null)
            {
                return NotFound(new ErrorResponseDto { Error = "Gatunek nie został znaleziony" });
            }

            return Ok(species);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Błąd podczas pobierania gatunku: {SpeciesId}", id);
            return StatusCode(500, new ErrorResponseDto { Error = "Wystąpił błąd serwera" });
        }
    }
}

