using PixHub.Services;
using Microsoft.AspNetCore.Mvc;

namespace PixHub.Controllers;

[ApiController]
[Route("health")]
public class HealthController(HealthService service) : ControllerBase
{
  readonly HealthService _service = service;

  [HttpGet]
  public IActionResult GetHealth()
  {
    return Ok(_service.GetHealth());
  }
}