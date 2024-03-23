using PixHub.Dtos;
using PixHub.Exceptions;
using PixHub.Models;

namespace PixHub.Services;

public class ConcilliationService(
  PaymentProviderService paymentProviderService,
  MessageService messageService)
{
  readonly MessageService _messageService = messageService;
  readonly PaymentProviderService _paymentProviderService = paymentProviderService;

  const string CONCILLIATION_QUEUE = "concilliation";

  public async Task Verify(ConcilliationDTO dto, string token)
  {
    if (dto.Date > DateTime.Today) throw new InvalidFutureDateException();

    PaymentProvider paymentProvider = await _paymentProviderService.FindByTokenAsync(token);
    PublishConcilliationDTO concilliationDTO = new(paymentProvider.Id, dto);

    _messageService.SendMessage(concilliationDTO, CONCILLIATION_QUEUE);
  }
}