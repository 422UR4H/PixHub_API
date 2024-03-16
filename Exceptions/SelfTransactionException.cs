namespace PixHub.Exceptions;

public class SelfTransactionException(string message = "Self transaction is not possible!") : Exception(message)
{

}