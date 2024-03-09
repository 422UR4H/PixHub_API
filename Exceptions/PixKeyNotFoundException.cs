namespace PixHub.Exceptions;

public class PixKeyNotFoundException(string message = "Pix Key not found!") : Exception(message)
{

}