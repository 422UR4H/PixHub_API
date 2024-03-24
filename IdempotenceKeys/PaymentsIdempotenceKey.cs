namespace PixHub.IdempotenceKeys;

public class PaymentsIdempotenceKey(string cpf, int amount, long pixKeyId)
{
  public string Cpf { get; } = cpf;
  public int Amount { get; } = amount;
  public long PixKeyId { get; } = pixKeyId;
}