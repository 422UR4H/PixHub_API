using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PixHub.Models;

[Index("Token", Name = "Idx_PaymentProvider_Token", IsUnique = true)]
public class PaymentProvider(string token, string bankName, string webhook)
{
  [Key]
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public long Id { get; set; }
  public string Token { get; set; } = token;
  public string BankName { get; set; } = bankName;
  public string Webhook { get; set; } = webhook;

  public ICollection<PaymentProviderAccount>? PaymentProviderAccounts { get; set; }

  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
  public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}