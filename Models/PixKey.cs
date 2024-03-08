using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PixHub.Models;

public class PixKey(string value, string type, int paymentProviderAccountId)
{
  [Key]
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public int Id { get; set; }
  public string Value { get; set; } = value;
  public string Type { get; set; } = type;

  [JsonIgnore]
  public PaymentProviderAccount? PaymentProviderAccount { get; set; }
  public int PaymentProviderAccountId { get; set; } = paymentProviderAccountId;

  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
  public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}