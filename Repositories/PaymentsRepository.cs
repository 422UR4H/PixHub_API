using Microsoft.EntityFrameworkCore;
using PixHub.Data;
using PixHub.Exceptions;
using PixHub.Models;

namespace PixHub.Repositories;

public class PaymentsRepository(AppDbContext dbContext)
{
  readonly AppDbContext _dbContext = dbContext;

  public async Task<Payments> CreateAsync(Payments payment)
  {
    var entry = _dbContext.Payments.Add(payment);
    await _dbContext.SaveChangesAsync();

    return entry.Entity;
  }

  public async Task FinishPaymentAsync(string status, int id)
  {
    Payments payment = await _dbContext.Payments.FirstOrDefaultAsync(p => p.Id == id)
      ?? throw new PaymentNotFoundException();

    payment.Status = status;
    await _dbContext.SaveChangesAsync();
  }
}