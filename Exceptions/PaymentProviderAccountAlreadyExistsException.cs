namespace PixHub.Exceptions;

public class PaymentProviderAccountAlreadyExistsException(string message = "Payment Provider Account already exists!") : Exception(message)
{

}