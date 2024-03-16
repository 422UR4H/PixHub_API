using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PixHub.Models;

public class PaymentProvider(string token, string bankName, string webhook)
{
  [Key]
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public int Id { get; set; }
  public string Token { get; set; } = token;
  public string BankName { get; set; } = bankName;
  public string Webhook { get; set; } = webhook;

  public ICollection<PaymentProviderAccount>? PaymentProviderAccounts { get; set; }

  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
  public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}