using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PixHub.Models;

public class User(string CPF, string name)
{
  [Key]
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public int Id { get; set; }
  public string CPF { get; set; } = CPF;
  public string Name { get; set; } = name;

  [JsonInclude]
  public ICollection<PaymentProviderAccount>? PaymentProviderAccounts { get; set; }

  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
  public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}