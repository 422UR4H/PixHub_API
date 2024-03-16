using System.ComponentModel.DataAnnotations;

namespace PixHub.Dtos;

public class FinishPaymentsDTO(string status)
{
  [RegularExpression("^(FAILED|SUCCESS)$")]
  [Required(ErrorMessage = "Field status is mandatory")]
  public string Status { get; } = status;
}