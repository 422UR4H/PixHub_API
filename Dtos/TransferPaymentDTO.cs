namespace PixHub.Dtos;

public class TransferPaymentDTO(
  int paymentId,
  Guid transactionId,
  CreatePaymentDTO createPaymentDTO,
  string originWebhook,
  string destinyWebhook)
{
  public int PaymentId { get; } = paymentId;
  public Guid TransactionId { get; } = transactionId;
  public OriginDTO Origin { get; } = createPaymentDTO.Origin;
  public DestinyDTO Destiny { get; } = createPaymentDTO.Destiny;
  // public WebhookDTO Webhook { get; } = new(originWebhook, destinyWebhook);
  public string OriginWebhook { get; } = originWebhook;
  public string DestinyWebhook { get; } = destinyWebhook;
  public int Amount { get; } = createPaymentDTO.Amount;
  public string? Description { get; } = createPaymentDTO.Description;
}