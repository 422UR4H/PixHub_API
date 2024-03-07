using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PixHub.Models;

public class User(string CPF, string name)
{
  [Key]
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public int Id { get; set; }
  public string CPF { get; set; } = CPF;
  public string Name { get; set; } = name;
}