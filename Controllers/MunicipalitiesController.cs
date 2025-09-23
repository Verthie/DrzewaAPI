using DrzewaAPI.Services;
using DrzewaAPI.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DrzewaAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MunicipalitiesController(IMunicipalityService _municipalityService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllMunicipalities()
    {
        List<MunicipalityDto> municipalities = await _municipalityService.GetAllMunicipalitiesAsync();

        return Ok(municipalities);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetMunicipalityById(string id)
    {
        Guid municipalityId = ValidationHelpers.ValidateAndParseId(id);

        MunicipalityDto municipality = await _municipalityService.GetMunicipalityByIdAsync(municipalityId);

        return Ok(municipality);
    }

    [HttpPost]
    [Authorize(Roles = "Moderator")]
    public async Task<IActionResult> CreateMunicipality([FromBody] CreateMunicipalityDto createDto)
    {
        ValidationHelpers.ValidateModelState(ModelState);

        MunicipalityDto municipality = await _municipalityService.CreateMunicipalityAsync(createDto);

        return CreatedAtAction(nameof(GetMunicipalityById), new { id = municipality.Id }, municipality);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Moderator")]
    public async Task<IActionResult> UpdateMunicipality(string id, [FromBody] UpdateMunicipalityDto updateDto)
    {
        ValidationHelpers.ValidateModelState(ModelState);

        Guid municipalityId = ValidationHelpers.ValidateAndParseId(id);

        MunicipalityDto municipality = await _municipalityService.UpdateMunicipalityAsync(municipalityId, updateDto);

        return Ok(municipality);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Moderator")]
    public async Task<IActionResult> DeleteMunicipality(string id)
    {
        Guid municipalityId = ValidationHelpers.ValidateAndParseId(id);

        await _municipalityService.DeleteMunicipalityAsync(municipalityId);

        return NoContent();
    }
}

