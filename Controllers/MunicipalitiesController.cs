using DrzewaAPI.Dtos.Auth;
using DrzewaAPI.Dtos.Municipality;
using DrzewaAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DrzewaAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MunicipalitiesController(IMunicipalityService _municipalityService, ILogger<MunicipalitiesController> _logger) : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = "Moderator")]
    public async Task<IActionResult> GetAllMunicipalities()
    {
        try
        {
            var municipalities = await _municipalityService.GetAllMunicipalitiesAsync();
            return Ok(municipalities);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Błąd podczas pobierania wszystkich gmin");
            return StatusCode(500, new ErrorResponseDto { Error = "Wystąpił błąd serwera" });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetMunicipalityById(string id)
    {
        try
        {
            if (!Guid.TryParse(id, out var municipalityId))
            {
                return BadRequest(new ErrorResponseDto { Error = "Nieprawidłowy format ID" });
            }

            var municipality = await _municipalityService.GetMunicipalityByIdAsync(municipalityId);

            if (municipality == null)
            {
                return NotFound(new ErrorResponseDto { Error = "Gmina nie została znaleziona" });
            }

            return Ok(municipality);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Błąd podczas pobierania gminy: {MunicipalityId}", id);
            return StatusCode(500, new ErrorResponseDto { Error = "Wystąpił błąd serwera" });
        }
    }

    [HttpGet("{id}/templates")]
    public async Task<IActionResult> GetMunicipalityTemplates(string id)
    {
        try
        {
            if (!Guid.TryParse(id, out var municipalityId))
            {
                return BadRequest(new ErrorResponseDto { Error = "Nieprawidłowy format ID" });
            }

            var templates = await _municipalityService.GetMunicipalityTemplatesAsync(municipalityId);
            return Ok(templates);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Błąd podczas pobierania szablonów gminy: {MunicipalityId}", id);
            return StatusCode(500, new ErrorResponseDto { Error = "Wystąpił błąd serwera" });
        }
    }

    [HttpGet("{id}/applications")]
    [Authorize(Roles = "Moderator")]
    public async Task<IActionResult> GetMunicipalityApplications(string id)
    {
        try
        {
            if (!Guid.TryParse(id, out var municipalityId))
            {
                return BadRequest(new ErrorResponseDto { Error = "Nieprawidłowy format ID" });
            }

            var applications = await _municipalityService.GetMunicipalityApplicationsAsync(municipalityId);
            return Ok(applications);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Błąd podczas pobierania wniosków gminy: {MunicipalityId}", id);
            return StatusCode(500, new ErrorResponseDto { Error = "Wystąpił błąd serwera" });
        }
    }

    [HttpPost]
    [Authorize(Roles = "Moderator")]
    public async Task<IActionResult> CreateMunicipality([FromBody] CreateMunicipalityDto createDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var municipality = await _municipalityService.CreateMunicipalityAsync(createDto);

            return CreatedAtAction(nameof(GetMunicipalityById),
                new { id = municipality.Id }, municipality);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new ErrorResponseDto { Error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Błąd podczas tworzenia gminy");
            return StatusCode(500, new ErrorResponseDto { Error = "Wystąpił błąd serwera" });
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Moderator")]
    public async Task<IActionResult> UpdateMunicipality(string id, [FromBody] UpdateMunicipalityDto updateDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!Guid.TryParse(id, out var municipalityId))
            {
                return BadRequest(new ErrorResponseDto { Error = "Nieprawidłowy format ID" });
            }

            var municipality = await _municipalityService.UpdateMunicipalityAsync(municipalityId, updateDto);

            if (municipality == null)
            {
                return NotFound(new ErrorResponseDto { Error = "Gmina nie została znaleziona" });
            }

            return Ok(municipality);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new ErrorResponseDto { Error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Błąd podczas aktualizacji gminy: {MunicipalityId}", id);
            return StatusCode(500, new ErrorResponseDto { Error = "Wystąpił błąd serwera" });
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Moderator")]
    public async Task<IActionResult> DeleteMunicipality(string id)
    {
        try
        {
            if (!Guid.TryParse(id, out var municipalityId))
            {
                return BadRequest(new ErrorResponseDto { Error = "Nieprawidłowy format ID" });
            }

            var result = await _municipalityService.DeleteMunicipalityAsync(municipalityId);

            if (!result)
            {
                return NotFound(new ErrorResponseDto { Error = "Gmina nie została znaleziona" });
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Błąd podczas usuwania gminy: {MunicipalityId}", id);
            return StatusCode(500, new ErrorResponseDto { Error = "Wystąpił błąd serwera" });
        }
    }
}

