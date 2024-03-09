using System.ComponentModel.DataAnnotations;

namespace PixHub.Dtos;

public class AccountDTO(string number, string agency)
{
  [Required]
  public string Number { get; } = number;

  [Required]
  public string Agency { get; } = agency;
}