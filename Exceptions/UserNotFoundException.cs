namespace PixHub.Exceptions;

public class UserNotFoundException(string message = "User not found!") : Exception(message)
{

}