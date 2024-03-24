using PixHub.Models;

namespace PixHub.Dtos;

public class OutputPixKeyDTO(KeyDTO keyDTO, OutputUserDTO user, OutputAccountDTO account)
{
  public KeyDTO Key { get; } = keyDTO;
  public OutputUserDTO User { get; } = user;
  public OutputAccountDTO Account { get; } = account;

  public OutputPixKeyDTO(
    PixKey pixKey,
    User user,
    PaymentProviderAccount account,
    PaymentProvider paymentProvider) : this(
      new KeyDTO(pixKey),
      new OutputUserDTO(user),
      new OutputAccountDTO(account, paymentProvider))
  { }
}