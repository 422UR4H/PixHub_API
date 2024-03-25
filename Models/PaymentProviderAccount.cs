using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PixHub.Models;

[Index(nameof(AccountNumber), nameof(Agency), IsUnique = true)]
public class PaymentProviderAccount(string accountNumber, string agency, long userId, long paymentProviderId)
{
  [Key]
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public long Id { get; set; }
  public string AccountNumber { get; set; } = accountNumber;
  public string Agency { get; set; } = agency;

  public ICollection<PixKey>? PixKeys { get; set; }
  public ICollection<Payments>? Payments { get; set; }

  public User? User { get; set; }
  public long UserId { get; set; } = userId;
  public PaymentProvider? PaymentProvider { get; set; }
  public long PaymentProviderId { get; set; } = paymentProviderId;

  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
  public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}