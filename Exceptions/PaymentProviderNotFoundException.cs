namespace PixHub.Exceptions;

public class PaymentProviderNotFoundException(string message = "Payment Provider not found!") : Exception(message)
{

}