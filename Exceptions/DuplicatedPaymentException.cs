namespace PixHub.Exceptions;

public class DuplicatedPaymentException(string message = "Duplicated payment!") : Exception(message)
{

}