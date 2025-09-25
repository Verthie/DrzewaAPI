using DrzewaAPI.Extensions;
using DrzewaAPI.Services;
using DrzewaAPI.Utils;
using Microsoft.AspNetCore.Authorization;
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

    [Authorize(Roles = "Moderator")]
    [HttpPost]
    public async Task<IActionResult> CreateTreeSubmission([FromForm] CreateTreeSpeciesDto request, IFormFile treeImage, IFormFile leafImage, IFormFile barkImage, IFormFile fruitImage)
    {
        ValidationHelpers.ValidateModelState(ModelState);

        if (treeImage == null || leafImage == null || barkImage == null || fruitImage == null)
        {
            return BadRequest("Images for all categories need to be provided");
        }

        Guid userId = User.GetCurrentUserId();
        TreeSpeciesDto result = await _speciesService.CreateTreeSpeciesAsync(request, treeImage, leafImage, barkImage, fruitImage);

        return CreatedAtAction(nameof(GetSpeciesById), new { id = result.Id }, result);
    }
}

