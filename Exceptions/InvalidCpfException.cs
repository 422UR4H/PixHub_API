namespace PixHub.Exceptions;

public class InvalidCpfException(string message = "Invalid CPF format!") : Exception(message)
{

}