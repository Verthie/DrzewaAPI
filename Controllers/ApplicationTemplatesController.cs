using System;
using DrzewaAPI.Dtos.Application;
using DrzewaAPI.Dtos.Auth;
using DrzewaAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DrzewaAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ApplicationTemplatesController(IApplicationTemplateService _templateService, ILogger<ApplicationTemplatesController> _logger) : ControllerBase
{
	[HttpGet]
	public async Task<IActionResult> GetAllTemplates()
	{
		try
		{
			var templates = await _templateService.GetAllTemplatesAsync();
			return Ok(templates);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Błąd podczas pobierania szablonów");
			return StatusCode(500, new ErrorResponseDto { Error = "Wystąpił błąd serwera" });
		}
	}

	[HttpGet("active")]
	public async Task<IActionResult> GetActiveTemplates()
	{
		try
		{
			var templates = await _templateService.GetActiveTemplatesAsync();
			return Ok(templates);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Błąd podczas pobierania szablonów");
			return StatusCode(500, new ErrorResponseDto { Error = "Wystąpił błąd serwera" });
		}
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> GetTemplateById(string id)
	{
		try
		{
			if (!Guid.TryParse(id, out var templateId))
			{
				return BadRequest(new ErrorResponseDto { Error = "Nieprawidłowy format ID" });
			}

			var template = await _templateService.GetTemplateByIdAsync(templateId);

			if (template == null)
			{
				return NotFound(new ErrorResponseDto { Error = "Szablon nie został znaleziony" });
			}

			return Ok(template);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Błąd podczas pobierania szablonu: {TemplateId}", id);
			return StatusCode(500, new ErrorResponseDto { Error = "Wystąpił błąd serwera" });
		}
	}

	[HttpGet("municipality/{municipalityId}")]
	public async Task<IActionResult> GetTemplatesByMunicipalityId(string municipalityId)
	{
		try
		{
			if (!Guid.TryParse(municipalityId, out var guid))
			{
				return BadRequest(new ErrorResponseDto { Error = "Nieprawidłowy format ID" });
			}

			var templates = await _templateService.GetTemplatesByMunicipalityIdAsync(guid);
			return Ok(templates);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Błąd podczas pobierania szablonów gminy: {MunicipalityId}", municipalityId);
			return StatusCode(500, new ErrorResponseDto { Error = "Wystąpił błąd serwera" });
		}
	}

	[HttpPost]
	[Authorize(Roles = "Moderator")]
	public async Task<IActionResult> CreateTemplate([FromBody] CreateApplicationTemplateDto createDto)
	{
		try
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var template = await _templateService.CreateTemplateAsync(createDto);

			return CreatedAtAction(nameof(GetTemplateById),
					new { id = template.Id }, template);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Błąd podczas tworzenia szablonu");
			return StatusCode(500, new ErrorResponseDto { Error = "Wystąpił błąd serwera" });
		}
	}

	[HttpPut("{id}")]
	[Authorize(Roles = "Moderator")]
	public async Task<IActionResult> UpdateTemplate(string id, [FromBody] UpdateApplicationTemplateDto updateDto)
	{
		try
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			if (!Guid.TryParse(id, out var guid))
			{
				return BadRequest(new ErrorResponseDto { Error = "Nieprawidłowy format ID" });
			}

			var template = await _templateService.UpdateTemplateAsync(guid, updateDto);

			if (template == null)
			{
				return NotFound(new ErrorResponseDto { Error = "Szablon nie został znaleziony" });
			}

			return Ok(template);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Błąd podczas aktualizacji szablonu: {TemplateId}", id);
			return StatusCode(500, new ErrorResponseDto { Error = "Wystąpił błąd serwera" });
		}
	}

	[HttpDelete("{id}")]
	[Authorize(Roles = "Moderator")]
	public async Task<IActionResult> DeleteTemplate(string id)
	{
		try
		{
			if (!Guid.TryParse(id, out var guid))
			{
				return BadRequest(new ErrorResponseDto { Error = "Nieprawidłowy format ID" });
			}

			var result = await _templateService.DeleteTemplateAsync(guid);

			if (!result)
			{
				return NotFound(new ErrorResponseDto { Error = "Szablon nie został znaleziony" });
			}

			return NoContent();
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Błąd podczas usuwania szablonu: {TemplateId}", id);
			return StatusCode(500, new ErrorResponseDto { Error = "Wystąpił błąd serwera" });
		}
	}
}