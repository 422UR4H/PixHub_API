using PixHub.Exceptions;
using PixHub.Models;
using PixHub.Repositories;

namespace PixHub.Services;

public class PaymentProviderService(PaymentProviderRepository repository)
{
  readonly PaymentProviderRepository _repository = repository;

  public async Task<PaymentProvider> FindByTokenAsync(string token)
  {
    return await _repository.FindByTokenAsync(token) ??
      throw new UnauthorizedProviderException();
  }
}