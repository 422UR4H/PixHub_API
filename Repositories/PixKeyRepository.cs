using PixHub.Data;
using PixHub.Models;
using Microsoft.EntityFrameworkCore;

namespace PixHub.Repositories;

public class PixKeyRepository(AppDbContext dbContext)
{
  readonly AppDbContext _dbContext = dbContext;

  public async Task<bool> ExistsPixKeyAsync(string value)
  {
    return await _dbContext.PixKey.AnyAsync(pk => pk.Value.Equals(value));
  }

  public async Task<PixKey?> CreateAsync(PixKey pixKey)
  {
    var entry = _dbContext.PixKey.Add(pixKey);
    await _dbContext.SaveChangesAsync();

    return entry.Entity;
  }

  public async Task<int> CountByUserIdAsync(int userId)
  {
    return await _dbContext.PixKey.CountAsync(pk => pk.PaymentProviderAccount!.UserId.Equals(userId));
  }

  public async Task<int> CountByAccountIdAsync(int accountId)
  {
    return await _dbContext.PixKey.CountAsync(pk => pk.PaymentProviderAccountId.Equals(accountId));
  }

  public async Task<PixKey?> FindWithAccountAndUserAsync(string type, string value)
  {
    return await _dbContext.PixKey
      .Include(pk => pk.PaymentProviderAccount)
      .ThenInclude(a => a!.User)
      .FirstOrDefaultAsync(pk => pk.Type.Equals(type) && pk.Value.Equals(value));
  }

  public async Task<PixKey?> FindWithAccountAndProviderAsync(string type, string value)
  {
    return await _dbContext.PixKey
      .Include(pk => pk.PaymentProviderAccount)
      .ThenInclude(a => a!.PaymentProvider)
      .FirstOrDefaultAsync(pk => pk.Type.Equals(type) && pk.Value.Equals(value));
  }
}