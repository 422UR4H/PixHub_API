namespace PixHub.Exceptions;

public class PixKeyAlreadyExistsException(string message = "Pix Key already exists!") : Exception(message)
{

}