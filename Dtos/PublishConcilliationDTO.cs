using System.ComponentModel.DataAnnotations;

namespace PixHub.Dtos;

public class PublishConcilliationDTO(long paymentProviderId, ConcilliationDTO dto)
{
  public long PaymentProviderId { get; } = paymentProviderId;
  public DateTime Date { get; } = dto.Date;
  public string File { get; } = dto.File;
  public string Postback { get; } = dto.Postback;
}
