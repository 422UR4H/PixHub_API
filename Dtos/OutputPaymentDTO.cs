namespace PixHub.Dtos;

public class OutputPaymentDTO(Guid transactionId)
{
  public Guid TransactionId { get; } = transactionId;
}