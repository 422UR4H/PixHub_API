using PixHub.Data;
using PixHub.Models;
using Microsoft.EntityFrameworkCore;

namespace PixHub.Repositories;

public class UserRepository(AppDbContext dbContext)
{
  readonly AppDbContext _dbContext = dbContext;

  public async Task<User?> FindByCpfWithAccountsThenIncludesPixKeysAsync(string cpf)
  {
    return await _dbContext.User
      .Where(u => u.PaymentProviderAccounts != null)
      .Include(u => u.PaymentProviderAccounts)!
      .ThenInclude(a => a.PixKeys)
      .FirstOrDefaultAsync(u => u.CPF == cpf);
  }
}