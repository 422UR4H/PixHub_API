namespace PixHub.Exceptions;

public class PixKeyPersistenceDatabaseException(string message = "Internal server error when creating pix key. Please retry later.") : Exception(message)
{

}