using PixHub.Data;
using PixHub.Models;
using Microsoft.EntityFrameworkCore;

namespace PixHub.Repositories;

public class PaymentProviderRepository(AppDbContext dbContext)
{
  readonly AppDbContext _dbContext = dbContext;

  public async Task<PaymentProvider?> FindByTokenAsync(string token)
  {
    return await _dbContext.PaymentProvider.FirstOrDefaultAsync(pp => pp.Token == token);
  }
}