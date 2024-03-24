using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace PixHub.Models;

[Index(nameof(Value), nameof(Type), IsUnique = true)]
public class PixKey(string value, string type, long paymentProviderAccountId)
{
  [Key]
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public long Id { get; set; }
  public string Value { get; set; } = value;
  public string Type { get; set; } = type;

  public ICollection<Payments>? Payments { get; set; }

  [JsonIgnore]
  public PaymentProviderAccount? PaymentProviderAccount { get; set; }
  public long PaymentProviderAccountId { get; set; } = paymentProviderAccountId;

  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
  public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}