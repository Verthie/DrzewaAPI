using DrzewaAPI.Services;
using DrzewaAPI.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DrzewaAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GeminiController(IGeminiService _geminiService) : ControllerBase
{
    [HttpGet("tree/{id}/justification")]
    public async Task<IActionResult> GetJustification(string id)
    {
        Guid treeId = ValidationHelpers.ValidateAndParseId(id);

        string response = await _geminiService.GetJustificationAsync(treeId);

        return Ok(response);
    }

    [HttpGet("test")]
    public async Task<IActionResult> GetTestMessage()
    {
        string response = await _geminiService.GetTestMessageAsync();

        return Ok(response);
    }
}
