using PixHub.Services;
using Microsoft.AspNetCore.Mvc;
using PixHub.Dtos;

namespace PixHub.Controllers;

[ApiController]
[Route("keys")]
public class PixKeyController(PixKeyService service) : ControllerBase
{
  readonly PixKeyService _service = service;

  [HttpPost]
  public async Task<IActionResult> PostPixKey([FromBody] CreatePixKeyDTO dto, [FromHeader] string authorization)
  {
    return CreatedAtAction(null, null, await _service.CreatePixKey(dto, authorization));
  }

  [HttpGet("{type}/{value}")]
  public async Task<IActionResult> GetPixKey(string type, string value, [FromHeader] string authorization)
  {
    return Ok(await _service.FindPixKey(type, value, authorization));
  }
}