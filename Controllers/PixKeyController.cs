using PixHub.Services;
using Microsoft.AspNetCore.Mvc;
using PixHub.Dtos;
using PixHub.Models;

namespace PixHub.Controllers;

[ApiController]
[Route("keys")]
public class PixKeyController(PixKeyService service) : ControllerBase
{
  readonly PixKeyService _service = service;

  [HttpPost]
  public async Task<IActionResult> PostPixKey([FromBody] CreatePixKeyDTO dto, [FromHeader] string token)
  {
    return CreatedAtAction(null, null, await _service.CreatePixKey(dto, token));
  }
}