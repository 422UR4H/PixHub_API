namespace PixHub.Exceptions;

public class TotalPixKeyLimitException(string message = "Total of Pix Key limit!") : Exception(message)
{

}