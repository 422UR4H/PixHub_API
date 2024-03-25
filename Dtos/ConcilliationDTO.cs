using System.ComponentModel.DataAnnotations;

namespace PixHub.Dtos;

public class ConcilliationDTO(DateTime date, string file, string postback)
{
  [DataType(DataType.Date)]
  [Required(ErrorMessage = "Field date is mandatory")]
  public DateTime Date { get; } = date;

  [Required(ErrorMessage = "Field file is mandatory")]
  public string File { get; } = file;

  [Required(ErrorMessage = "Field postback is mandatory")]
  public string Postback { get; } = postback;
}
