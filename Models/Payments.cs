using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PixHub.Models;

public class Payments(int paymentProviderAccountId, int pixKeyId, int amount, string? description, string status = "PROCESSING")
{
  [Key]
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public int Id { get; set; }
  public Guid TransactionId { get; set; } = Guid.NewGuid();
  public string Status { get; set; } = status;
  public int Amount { get; set; } = amount;
  public string? Description { get; set; } = description;

  [JsonIgnore]
  public PaymentProviderAccount? PaymentProviderAccount { get; set; }
  public int PaymentProviderAccountId { get; set; } = paymentProviderAccountId;
  
  [JsonIgnore]
  public PixKey? PixKey { get; set; }
  public int PixKeyId { get; set; } = pixKeyId;

  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
  public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}