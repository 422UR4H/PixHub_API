using PixHub.Data;
using PixHub.Models;
using Microsoft.EntityFrameworkCore;

namespace PixHub.Repositories;

public class UserRepository(AppDbContext dbContext)
{
  readonly AppDbContext _dbContext = dbContext;

  public async Task<User?> FindByCpfAsync(string cpf)
  {
    return await _dbContext.User
      .Include(u => u.PaymentProviderAccounts)
      .Where(u => u.PaymentProviderAccounts != null)
      .FirstOrDefaultAsync(u => u.CPF == cpf);
      // .ThenInclude(a => a.PixKeys)
  }
}