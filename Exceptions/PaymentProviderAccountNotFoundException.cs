namespace PixHub.Exceptions;

public class PaymentProviderAccountNotFoundException(string message = "Payment Provider Account not found!") : Exception(message)
{

}