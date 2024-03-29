using Microsoft.AspNetCore.Mvc;
using PixHub.Dtos;
using PixHub.Services;

namespace PixHub.Controllers;

[ApiController]
[Route("payments")]
public class PaymentController(PaymentsService service) : ControllerBase
{
  readonly PaymentsService _service = service;

  [HttpPost]
  public async Task<IActionResult> PostPayments([FromBody] CreatePaymentDTO dto, [FromHeader] string token)
  {
    return CreatedAtAction(null, null, await _service.CreatePayment(dto, token));
  }

  [HttpPost("finish/{id}/{transactionId}")]
  public async Task<IActionResult> FinishPayments(int id, Guid transactionId, [FromBody] FinishPaymentsDTO dto)
  {
    await _service.FinishPayment(dto, id, transactionId);
    return Ok();
  }
}