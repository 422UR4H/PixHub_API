namespace PixHub.Exceptions;

public class PaymentNotFoundException(string message = "Payment not found!") : Exception(message)
{

}