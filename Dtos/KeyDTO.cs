using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using PixHub.Models;

namespace PixHub.Dtos;

[method: JsonConstructor]
public class KeyDTO(string value, string type)
{
  [StringLength(32)]
  [Required(ErrorMessage = "Field value is mandatory")]
  public string Value { get; } = value;

  [RegularExpression("^(CPF|Email|Phone|Random)$")]
  [Required(ErrorMessage = "Field type is mandatory")]
  public string Type { get; } = type;

  public KeyDTO(PixKey pixKey) : this(pixKey.Value, pixKey.Type) { }

  public PixKey ToEntity(long accountId)
  {
    return new PixKey(Value, Type, accountId);
  }
}