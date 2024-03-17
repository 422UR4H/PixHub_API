namespace PixHub.Exceptions;

public class InvalidAccountNumberException(string message = "Account Number should be just numbers!") : Exception(message)
{

}