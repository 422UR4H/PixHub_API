using System.ComponentModel.DataAnnotations;

namespace PixHub.Dtos;

public class UserDTO(string cpf)
{
  [Length(11, 11)]
  [Required(ErrorMessage = "Field cpf is mandatory")]
  public string Cpf { get; } = cpf;
}