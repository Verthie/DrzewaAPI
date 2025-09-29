using DrzewaAPI.Services;
using DrzewaAPI.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DrzewaAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CommunesController(ICommuneService _communeService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllCommunes()
    {
        List<CommuneDto> communes = await _communeService.GetAllCommunesAsync();

        return Ok(communes);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCommuneById(string id)
    {
        Guid communeId = ValidationHelpers.ValidateAndParseId(id);

        CommuneDto commune = await _communeService.GetCommuneByIdAsync(communeId);

        return Ok(commune);
    }

    [HttpPost]
    [Authorize(Roles = "Moderator")]
    public async Task<IActionResult> CreateCommune([FromBody] CreateCommuneDto createDto)
    {
        ValidationHelpers.ValidateModelState(ModelState);

        CommuneDto commune = await _communeService.CreateCommuneAsync(createDto);

        return CreatedAtAction(nameof(GetCommuneById), new { id = commune.Id }, commune);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Moderator")]
    public async Task<IActionResult> UpdateCommune(string id, [FromBody] UpdateCommuneDto updateDto)
    {
        ValidationHelpers.ValidateModelState(ModelState);

        Guid communeId = ValidationHelpers.ValidateAndParseId(id);

        CommuneDto commune = await _communeService.UpdateCommuneAsync(communeId, updateDto);

        return Ok(commune);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Moderator")]
    public async Task<IActionResult> DeleteCommune(string id)
    {
        Guid communeId = ValidationHelpers.ValidateAndParseId(id);

        await _communeService.DeleteCommuneAsync(communeId);

        return NoContent();
    }
}

