using System.ComponentModel.DataAnnotations;

namespace PixHub.Dtos;

public class UserDTO(string cpf)
{
  [Required]
  public string Cpf { get; } = cpf;
}