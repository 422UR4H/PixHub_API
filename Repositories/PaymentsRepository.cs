using Microsoft.EntityFrameworkCore;
using PixHub.Data;
using PixHub.Exceptions;
using PixHub.IdempotenceKeys;
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

  public async Task FinishPaymentAsync(string status, long id, Guid transactionId)
  {
    Payments payment = await _dbContext.Payments
      .FirstOrDefaultAsync(p => p.Id == id && p.TransactionId == transactionId) ??
      throw new PaymentNotFoundException();

    payment.Status = status;
    await _dbContext.SaveChangesAsync();
  }

  public async Task<Payments?> FindRecentPaymentByIdempotenceKey(
    PaymentsIdempotenceKey key,
    int seconds)
  {
    DateTime secondsAgo = DateTime.UtcNow.AddSeconds(-seconds);

    // TODO: refactor this expression
    return await _dbContext.Payments.Where(p =>
      p.Amount.Equals(key.Amount) &&
      p.PixKeyId.Equals(key.PixKeyId) &&
      p.PaymentProviderAccount!.User!.CPF.Equals(key.Cpf) &&
      p.CreatedAt >= secondsAgo
    ).FirstOrDefaultAsync();
  }
}