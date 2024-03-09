namespace PixHub.Exceptions;

public class InvalidCpfPixKeyException(string message = "CPF type Pix Key must be the same as the user's CPF!") : Exception(message)
{

}