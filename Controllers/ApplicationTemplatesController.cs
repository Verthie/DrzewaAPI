using DrzewaAPI.Services;
using DrzewaAPI.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DrzewaAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ApplicationTemplatesController(IApplicationTemplateService _templateService) : ControllerBase
{
	[HttpGet]
	public async Task<IActionResult> GetAllTemplates()
	{
		List<ApplicationTemplateDto> templates = await _templateService.GetAllTemplatesAsync();

		return Ok(templates);
	}

	[HttpGet("active")]
	public async Task<IActionResult> GetActiveTemplates()
	{
		List<ApplicationTemplateDto> templates = await _templateService.GetActiveTemplatesAsync();

		return Ok(templates);
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> GetTemplateById(string id)
	{
		Guid templateId = ValidationHelpers.ValidateAndParseId(id);

		ApplicationTemplateDto template = await _templateService.GetTemplateByIdAsync(templateId);

		return Ok(template);
	}

	[HttpGet("municipality/{municipalityId}")]
	public async Task<IActionResult> GetTemplatesByMunicipalityId(string municipalityId)
	{
		Guid municipalityGuid = ValidationHelpers.ValidateAndParseId(municipalityId);

		List<ApplicationTemplateDto> templates = await _templateService.GetTemplatesByMunicipalityIdAsync(municipalityGuid);

		return Ok(templates);
	}

	[HttpPost]
	[Authorize(Roles = "Moderator")]
	public async Task<IActionResult> CreateTemplate([FromBody] CreateApplicationTemplateDto createDto)
	{
		ValidationHelpers.ValidateModelState(ModelState);

		ApplicationTemplateDto template = await _templateService.CreateTemplateAsync(createDto);

		return CreatedAtAction(nameof(GetTemplateById),
				new { id = template.Id }, template);
	}

	[HttpPut("{id}")]
	[Authorize(Roles = "Moderator")]
	public async Task<IActionResult> UpdateTemplate(string id, [FromBody] UpdateApplicationTemplateDto updateDto)
	{
		ValidationHelpers.ValidateModelState(ModelState);
		Guid templateId = ValidationHelpers.ValidateAndParseId(id);

		ApplicationTemplateDto template = await _templateService.UpdateTemplateAsync(templateId, updateDto);

		return Ok(template);
	}

	[HttpDelete("{id}")]
	[Authorize(Roles = "Moderator")]
	public async Task<IActionResult> DeleteTemplate(string id)
	{
		Guid templateId = ValidationHelpers.ValidateAndParseId(id);

		await _templateService.DeleteTemplateAsync(templateId);

		return NoContent();
	}
}