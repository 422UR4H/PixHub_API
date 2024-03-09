namespace PixHub.Exceptions;

public class InvalidEmailException(string message = "Invalid e-mail format!") : Exception(message)
{

}