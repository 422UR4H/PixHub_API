using PixHub.Data;
using PixHub.Models;
using Microsoft.EntityFrameworkCore;

namespace PixHub.Repositories;

public class PaymentProviderAccountRepository(AppDbContext dbContext)
{
  readonly AppDbContext _dbContext = dbContext;

  public async Task<bool> ExistsAccountNumberAsync(string number)
  {
    return await _dbContext.PaymentProviderAccount.AnyAsync(a => a.AccountNumber == number);
  }

  public async Task<PaymentProviderAccount> CreateAsync(PaymentProviderAccount account)
  {
    var entry = _dbContext.PaymentProviderAccount.Add(account);
    await _dbContext.SaveChangesAsync();

    return entry.Entity;
  }

  internal async Task<PaymentProviderAccount?> FindByUserCpfAsync(string cpf, string agency, string number)
  {
    return await _dbContext.PaymentProviderAccount
      .FirstOrDefaultAsync(a => a.User!.CPF == cpf && a.Agency == agency && a.AccountNumber == number);
  }
}