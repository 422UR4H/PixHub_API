using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PixHub.Models;

public class PaymentProviderAccount(string accountNumber, string agency)
{
  [Key]
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public int Id { get; set; }
  public string AccountNumber { get; set; } = accountNumber;
  public string Agency { get; set; } = agency;
}