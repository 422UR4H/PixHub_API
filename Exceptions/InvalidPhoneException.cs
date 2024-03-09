namespace PixHub.Exceptions;

public class InvalidPhoneException(string message = "Invalid phone format!") : Exception(message)
{

}