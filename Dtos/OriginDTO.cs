using System.ComponentModel.DataAnnotations;

namespace PixHub.Dtos;

public class OriginDTO(UserDTO user, AccountDTO account)
{
  [Required(ErrorMessage = "Field user is mandatory")]
  public UserDTO User { get; } = user;

  [Required(ErrorMessage = "Field account is mandatory")]
  public AccountDTO Account { get; } = account;

  public string GetCpfUser()
  {
    return User.Cpf;
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