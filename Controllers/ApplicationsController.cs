using System.ComponentModel.DataAnnotations;
using DrzewaAPI.Extensions;
using DrzewaAPI.Services;
using DrzewaAPI.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DrzewaAPI.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ApplicationsController(IApplicationService _applicationService) : ControllerBase
{
	[HttpGet]
	public async Task<IActionResult> GetUserApplications()
	{
		Guid userId = User.GetCurrentUserId();

		List<ApplicationDto> applications = await _applicationService.GetUserApplicationsAsync(userId);

		return Ok(applications);
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> GetApplicationById(string id)
	{
		Guid applicationId = ValidationHelpers.ValidateAndParseId(id);
		Guid userId = User.GetCurrentUserId();

		ApplicationDto application = await _applicationService.GetApplicationByIdAsync(applicationId, userId);

		return Ok(application);
	}

	[HttpPost]
	[IdempotentAction(10)] // Cache for 10 minutes
	public async Task<IActionResult> CreateApplication([FromBody] CreateApplicationDto createDto, [FromHeader(Name = "X-Idempotency-Key")][Required] string idempotencyKey)
	{
		ValidationHelpers.ValidateModelState(ModelState);
		Guid userId = User.GetCurrentUserId();

		ApplicationDto application = await _applicationService.CreateApplicationAsync(userId, createDto);

		return CreatedAtAction(nameof(GetApplicationById),
				new { id = application.Id }, application);
	}

	[HttpPut("{id}")]
	public async Task<IActionResult> UpdateApplication(string id, [FromBody] UpdateApplicationDto updateDto)
	{
		ValidationHelpers.ValidateModelState(ModelState);
		Guid applicationId = ValidationHelpers.ValidateAndParseId(id);
		Guid userId = User.GetCurrentUserId();

		ApplicationDto application = await _applicationService.UpdateApplicationAsync(applicationId, userId, updateDto);

		return Ok(application);
	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> DeleteApplication(string id)
	{
		Guid applicationId = ValidationHelpers.ValidateAndParseId(id);
		Guid userId = User.GetCurrentUserId();

		await _applicationService.DeleteApplicationAsync(applicationId, userId);

		return NoContent();
	}

	[HttpGet("{id}/form-schema")]
	public async Task<IActionResult> GetApplicationFormSchema(string id)
	{
		Guid applicationId = ValidationHelpers.ValidateAndParseId(id);
		Guid userId = User.GetCurrentUserId();

		ApplicationFormSchemaDto schema = await _applicationService.GetApplicationFormSchemaAsync(applicationId, userId);

		return Ok(schema);
	}

	[HttpPost("{id}/submit")]
	public async Task<IActionResult> SubmitApplication(string id, [FromBody] SubmitApplicationDto submitDto)
	{
		ValidationHelpers.ValidateModelState(ModelState);
		Guid applicationId = ValidationHelpers.ValidateAndParseId(id);
		Guid userId = User.GetCurrentUserId();

		ApplicationDto application = await _applicationService.SubmitApplicationAsync(applicationId, userId, submitDto);

		return Ok(application);
	}

	[HttpPost("{id}/generate-pdf")]
	public async Task<IActionResult> GeneratePdf(string id)
	{
		Guid applicationId = ValidationHelpers.ValidateAndParseId(id);
		Guid userId = User.GetCurrentUserId();

		string pdfPath = await _applicationService.GeneratePdfFromAplicationAsync(applicationId, userId);

		return Ok(new { PdfUrl = pdfPath });
	}
}