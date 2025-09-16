using DrzewaAPI.Dtos.Auth;
using DrzewaAPI.Dtos.Species;
using DrzewaAPI.Models;
using DrzewaAPI.Services;
using DrzewaAPI.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DrzewaAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SpeciesController(ISpeciesService _speciesService) : ControllerBase
{

    [HttpGet]
    public async Task<IActionResult> GetSpecies()
    {
        List<TreeSpeciesDto> species = await _speciesService.GetAllSpeciesAsync();

        return Ok(species);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetSpeciesById(string id)
    {
        Guid speciesId = ValidationHelpers.ValidateAndParseId(id);

        TreeSpeciesDto species = await _speciesService.GetSpeciesByIdAsync(speciesId);

        return Ok(species);
    }
}

