namespace PixHub.Exceptions;

public class UnauthorizedProviderException(string message = "Payment Provider Access Denied!") : Exception(message)
{

}