namespace PixHub.Exceptions;

public class InvalidFutureDateException(string message = "Future date is invalid!") : Exception(message)
{

}