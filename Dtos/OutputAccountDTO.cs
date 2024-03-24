using PixHub.Models;

namespace PixHub.Dtos;

public class OutputAccountDTO(string number, string agency, string bankName, long bankId)
{
  public string Number { get; } = number;
  public string Agency { get; } = agency;
  public string BankName { get; } = bankName;
  public long BankId { get; } = bankId;

  public OutputAccountDTO(PaymentProviderAccount account, PaymentProvider paymentProvider) :
    this(account.AccountNumber, account.Agency, paymentProvider.BankName, paymentProvider.Id)
  { }
}