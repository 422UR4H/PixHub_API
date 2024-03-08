using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PixHub.Models;

public class PaymentProviderAccount(string accountNumber, string agency, int userId, int paymentProviderId)
{
  [Key]
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public int Id { get; set; }
  public string AccountNumber { get; set; } = accountNumber;
  public string Agency { get; set; } = agency;

  public ICollection<PixKey>? PixKeys { get; set; }

  public User? User { get; set; }
  public int UserId { get; set; } = userId;
  public PaymentProvider? PaymentProvider { get; set; }
  public int PaymentProviderId { get; set; } = paymentProviderId;

  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
  public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}