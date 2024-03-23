using System.ComponentModel.DataAnnotations;

namespace PixHub.Dtos;

public class PublishConcilliationDTO(int paymentProviderId, ConcilliationDTO dto)
{
  public int PaymentProviderId { get; } = paymentProviderId;
  public DateTime Date { get; } = dto.Date;
  public string File { get; } = dto.File;
  public string Postback { get; } = dto.Postback;
}
