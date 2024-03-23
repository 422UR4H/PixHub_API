using PixHub.Exceptions;
using PixHub.Models;

namespace PixHub.Middlewares;

public static class ValidationMiddleware
{
  public static void ValidatesRequestIntegrityBy(
    PaymentProvider paymentProvider,
    PaymentProviderAccount account)
  {
    if (paymentProvider.Id != account.PaymentProviderId)
    {
      throw new UnauthorizedProviderException();
    }
  }
}