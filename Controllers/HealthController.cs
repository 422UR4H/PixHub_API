using MakeMeAPix.Services;
using Microsoft.AspNetCore.Mvc;

namespace MakeMeAPix.Controllers;

[ApiController]
[Route("[controller]")]
public class HealthController(HealthService service) : ControllerBase
{
  HealthService _service = service;

  [HttpGet]
  public IActionResult GetHealth()
  {
    return Ok(_service.GetHealth());
  }
}