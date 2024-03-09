using System.ComponentModel.DataAnnotations;

namespace PixHub.Dtos;

public class AccountDTO(string number, string agency)
{
  [StringLength(20)]
  [Required(ErrorMessage = "Field number is mandatory")]
  public string Number { get; } = number;

  [StringLength(64)]
  [Required(ErrorMessage = "Field agency is mandatory")]
  public string Agency { get; } = agency;
}