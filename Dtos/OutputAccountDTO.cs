using PixHub.Models;

namespace PixHub.Dtos;

public class OutputAccountDTO(string number, string agency, string bankName, int bankId)
{
  public string Number { get; } = number;
  public string Agency { get; } = agency;
  public string BankName { get; } = bankName;
  public int BankId { get; } = bankId;

  public OutputAccountDTO(PaymentProviderAccount account, PaymentProvider paymentProvider) :
    this(account.AccountNumber, account.Agency, paymentProvider.BankName, paymentProvider.Id)
  { }
}