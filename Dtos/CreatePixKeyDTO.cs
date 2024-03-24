using System.ComponentModel.DataAnnotations;
using PixHub.Models;

namespace PixHub.Dtos;

public class CreatePixKeyDTO(KeyDTO key, UserDTO user, AccountDTO account)
{
  [Required(ErrorMessage = "Field key is mandatory")]
  public KeyDTO Key { get; } = key;

  [Required(ErrorMessage = "Field user is mandatory")]
  public UserDTO User { get; } = user;

  [Required(ErrorMessage = "Field account is mandatory")]
  public AccountDTO Account { get; } = account;

  public PixKey ToEntity(long accountId)
  {
    return Key.ToEntity(accountId);
  }

  public string GetCpfUser()
  {
    return User.Cpf;
  }

  public string GetKeyValue()
  {
    return Key.Value;
  }

  public string GetKeyType()
  {
    return Key.Type;
  }

  public string GetAgency()
  {
    return Account.Agency;
  }

  public string GetAccountNumber()
  {
    return Account.Number;
  }
}