using System.ComponentModel.DataAnnotations;
using PixHub.Models;

namespace PixHub.Dtos;

public class KeyDTO(string value, string type)
{
  [Required]
  public string Value { get; } = value;

  [Required]
  // TODO: add enum validation here
  public string Type { get; } = type;

  public KeyDTO(PixKey pixKey) : this(pixKey.Value, pixKey.Type) { }

  public PixKey ToEntity(int accountId)
  {
    return new PixKey(Value, Type, accountId);
  }
}