namespace PixHub.IdempotenceKeys;

public class PaymentsIdempotenceKey(string cpf, int amount, int pixKeyId)
{
  public string Cpf { get; } = cpf;
  public int Amount { get; } = amount;
  public int PixKeyId { get; } = pixKeyId;
}