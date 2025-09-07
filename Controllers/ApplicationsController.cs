using DrzewaAPI.Dtos.Application;
using DrzewaAPI.Dtos.Auth;
using DrzewaAPI.Extensions;
using DrzewaAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DrzewaAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ApplicationsController(IApplicationService _applicationService, ILogger<ApplicationsController> _logger) : ControllerBase
{
	[HttpGet]
	public async Task<IActionResult> GetUserApplications()
	{
		try
		{
			var userId = User.GetCurrentUserId();
			var applications = await _applicationService.GetUserApplicationsAsync(userId);
			return Ok(applications);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Błąd podczas pobierania wniosków użytkownika");
			return StatusCode(500, new ErrorResponseDto { Error = "Wystąpił błąd serwera" });
		}
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> GetApplicationById(string id)
	{
		try
		{
			if (!Guid.TryParse(id, out var applicationId))
			{
				return BadRequest(new ErrorResponseDto { Error = "Nieprawidłowy format ID" });
			}

			var userId = User.GetCurrentUserId();
			var application = await _applicationService.GetApplicationByIdAsync(applicationId, userId);

			if (application == null)
			{
				return NotFound(new ErrorResponseDto { Error = "Wniosek nie został znaleziony" });
			}

			return Ok(application);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Błąd podczas pobierania wniosku: {ApplicationId}", id);
			return StatusCode(500, new ErrorResponseDto { Error = "Wystąpił błąd serwera" });
		}
	}

	[HttpPost]
	public async Task<IActionResult> CreateApplication([FromBody] CreateApplicationDto createDto)
	{
		try
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var userId = User.GetCurrentUserId();
			var application = await _applicationService.CreateApplicationAsync(userId, createDto);

			return CreatedAtAction(nameof(GetApplicationById),
					new { id = application.Id }, application);
		}
		catch (ArgumentException ex)
		{
			return BadRequest(new ErrorResponseDto { Error = ex.Message });
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Błąd podczas tworzenia wniosku");
			return StatusCode(500, new ErrorResponseDto { Error = "Wystąpił błąd serwera" });
		}
	}

	[HttpPut("{id}")]
	public async Task<IActionResult> UpdateApplication(string id, [FromBody] UpdateApplicationDto updateDto)
	{
		try
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			if (!Guid.TryParse(id, out var applicationId))
			{
				return BadRequest(new ErrorResponseDto { Error = "Nieprawidłowy format ID" });
			}

			var userId = User.GetCurrentUserId();
			var application = await _applicationService.UpdateApplicationAsync(applicationId, userId, updateDto);

			if (application == null)
			{
				return NotFound(new ErrorResponseDto { Error = "Wniosek nie został znaleziony" });
			}

			return Ok(application);
		}
		catch (InvalidOperationException ex)
		{
			return BadRequest(new ErrorResponseDto { Error = ex.Message });
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Błąd podczas aktualizacji wniosku: {ApplicationId}", id);
			return StatusCode(500, new ErrorResponseDto { Error = "Wystąpił błąd serwera" });
		}
	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> DeleteApplication(string id)
	{
		try
		{
			if (!Guid.TryParse(id, out var applicationId))
			{
				return BadRequest(new ErrorResponseDto { Error = "Nieprawidłowy format ID" });
			}

			var userId = User.GetCurrentUserId();
			var result = await _applicationService.DeleteApplicationAsync(applicationId, userId);

			if (!result)
			{
				return NotFound(new ErrorResponseDto { Error = "Wniosek nie został znaleziony" });
			}

			return NoContent();
		}
		catch (InvalidOperationException ex)
		{
			return BadRequest(new ErrorResponseDto { Error = ex.Message });
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Błąd podczas usuwania wniosku: {ApplicationId}", id);
			return StatusCode(500, new ErrorResponseDto { Error = "Wystąpił błąd serwera" });
		}
	}

	[HttpGet("{id}/form-schema")]
	public async Task<IActionResult> GetApplicationFormSchema(string id)
	{
		try
		{
			if (!Guid.TryParse(id, out var applicationId))
			{
				return BadRequest(new ErrorResponseDto { Error = "Nieprawidłowy format ID" });
			}

			var userId = User.GetCurrentUserId();
			var schema = await _applicationService.GetApplicationFormSchemaAsync(applicationId, userId);

			if (schema == null)
			{
				return NotFound(new ErrorResponseDto { Error = "Wniosek nie został znaleziony" });
			}

			return Ok(schema);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Błąd podczas pobierania schematu formularza: {ApplicationId}", id);
			return StatusCode(500, new ErrorResponseDto { Error = "Wystąpił błąd serwera" });
		}
	}

	[HttpPost("{id}/submit")]
	public async Task<IActionResult> SubmitApplication(string id, [FromBody] SubmitApplicationDto submitDto)
	{
		try
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			if (!Guid.TryParse(id, out var applicationId))
			{
				return BadRequest(new ErrorResponseDto { Error = "Nieprawidłowy format ID" });
			}

			var userId = User.GetCurrentUserId();
			var application = await _applicationService.SubmitApplicationAsync(applicationId, userId, submitDto);

			if (application == null)
			{
				return NotFound(new ErrorResponseDto { Error = "Wniosek nie został znaleziony" });
			}

			return Ok(application);
		}
		catch (InvalidOperationException ex)
		{
			return BadRequest(new ErrorResponseDto { Error = ex.Message });
		}
		catch (ArgumentException ex)
		{
			return BadRequest(new ErrorResponseDto { Error = ex.Message });
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Błąd podczas przesyłania wniosku: {ApplicationId}", id);
			return StatusCode(500, new ErrorResponseDto { Error = "Wystąpił błąd serwera" });
		}
	}

	[HttpPost("{id}/generate-pdf")]
	public async Task<IActionResult> GeneratePdf(string id)
	{
		try
		{
			if (!Guid.TryParse(id, out var applicationId))
			{
				return BadRequest(new ErrorResponseDto { Error = "Nieprawidłowy format ID" });
			}

			var userId = User.GetCurrentUserId();
			var pdfPath = await _applicationService.GeneratePdfFromAplicationAsync(applicationId, userId);

			return Ok(new { PdfUrl = pdfPath });
		}
		catch (ArgumentException ex)
		{
			return BadRequest(new ErrorResponseDto { Error = ex.Message });
		}
		catch (InvalidOperationException ex)
		{
			return BadRequest(new ErrorResponseDto { Error = ex.Message });
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Błąd podczas generowania PDF: {ApplicationId}", id);
			return StatusCode(500, new ErrorResponseDto { Error = "Wystąpił błąd serwera" });
		}
	}
}