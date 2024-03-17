using System.ComponentModel.DataAnnotations;

namespace PixHub.Dtos;

public class DestinyDTO(KeyDTO key)
{
  [Required(ErrorMessage = "Field destiny is mandatory")]
  public KeyDTO Key { get; } = key;

  public string GetKeyValue()
  {
    return Key.Value;
  }

  public string GetKeyType()
  {
    return Key.Type;
  }
}