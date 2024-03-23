using Microsoft.AspNetCore.Mvc;
using PixHub.Dtos;
using PixHub.Services;

namespace PixHub.Controllers;

[ApiController]
[Route("concilliation")]
public class ConcilliationController(ConcilliationService service) : ControllerBase
{
  readonly ConcilliationService _service = service;

  [HttpPost]
  public async Task<IActionResult> PostConcilliation([FromBody] ConcilliationDTO dto, [FromHeader] string token)
  {
    await _service.Verify(dto, token);
    return Ok();
  }
}