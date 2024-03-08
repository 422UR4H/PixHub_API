using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PixHub.Models;

public class PaymentProvider(string token, string bankName)
{
  [Key]
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public int Id { get; set; }
  public string Token { get; set; } = token;
  public string BankName { get; set; } = bankName;

  public ICollection<PaymentProviderAccount>? PaymentProviderAccounts { get; set; }
}