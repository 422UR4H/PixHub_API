using PixHub.Data;
using PixHub.Models;
using Microsoft.EntityFrameworkCore;

namespace PixHub.Repositories;

public class PaymentProviderAccountRepository(AppDbContext dbContext)
{
  readonly AppDbContext _dbContext = dbContext;

  public async Task<PaymentProviderAccount?> FindByAccountNumberAsync(string number)
  {
    return await _dbContext.PaymentProviderAccount.FirstOrDefaultAsync(ppa => ppa.AccountNumber == number);
  }

  public async Task<PaymentProviderAccount> CreateAsync(PaymentProviderAccount account)
  {
    var entry = _dbContext.PaymentProviderAccount.Add(account);
    await _dbContext.SaveChangesAsync();

    return entry.Entity;
  }
}