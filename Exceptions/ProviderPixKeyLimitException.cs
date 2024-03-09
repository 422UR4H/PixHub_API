namespace PixHub.Exceptions;

public class ProviderPixKeyLimitException(string message = "Pix Key limit for this provider!") : Exception(message)
{

}