using System.ComponentModel.DataAnnotations;
using PixHub.Models;

namespace PixHub.Dtos;

public class CreatePixKeyDTO(KeyDTO key, UserDTO user, AccountDTO account)
{
  [Required]
  public KeyDTO Key { get; } = key;

  [Required]
  public UserDTO User { get; } = user;

  [Required]
  public AccountDTO Account { get; } = account;

  public PixKey ToEntity(int accountId)
  {
    return Key.ToEntity(accountId);
  }
}