using System.Net;
using PixHub.Exceptions;

namespace PixHub.Middlewares;

public class GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
{
  readonly RequestDelegate _next = next;
  readonly ILogger<GlobalExceptionHandlerMiddleware> _logger = logger;

  public async Task InvokeAsync(HttpContext context)
  {
    try
    {
      await _next(context);
    }
    catch (Exception ex)
    {
      await HandleExceptionAsync(context, ex);
    }
  }

  private async Task HandleExceptionAsync(HttpContext context, Exception ex)
  {
    _logger.LogError(ex, "An unexpected error occurred.");

    //More log stuff        

    ExceptionResponse response = ex switch
    {
      UserNotFoundException _ => new ExceptionResponse(HttpStatusCode.NotFound, ex.Message),
      PixKeyNotFoundException _ => new ExceptionResponse(HttpStatusCode.NotFound, ex.Message),
      TotalPixKeyLimitException _ => new ExceptionResponse(HttpStatusCode.Forbidden, ex.Message),
      ProviderPixKeyLimitException _ => new ExceptionResponse(HttpStatusCode.Forbidden, ex.Message),
      PixKeyAlreadyExistsException _ => new ExceptionResponse(HttpStatusCode.Conflict, ex.Message),
      InvalidCpfPixKeyException _ => new ExceptionResponse(HttpStatusCode.Forbidden, ex.Message),
      InvalidCpfException _ => new ExceptionResponse(HttpStatusCode.UnprocessableEntity, ex.Message),
      InvalidEmailException _ => new ExceptionResponse(HttpStatusCode.UnprocessableEntity, ex.Message),
      InvalidPhoneException _ => new ExceptionResponse(HttpStatusCode.UnprocessableEntity, ex.Message),
      UnauthorizedProviderException _ => new ExceptionResponse(HttpStatusCode.Unauthorized, ex.Message),
      PixKeyPersistenceDatabaseException _ => new ExceptionResponse(HttpStatusCode.InternalServerError, ex.Message),
      PaymentProviderAccountNotFoundException _ => new ExceptionResponse(HttpStatusCode.NotFound, ex.Message),
      PaymentProviderAccountAlreadyExistsException _ => new ExceptionResponse(HttpStatusCode.Conflict, ex.Message),
      _ => new ExceptionResponse(HttpStatusCode.InternalServerError, "Internal server error. Please retry later.")
    };

    context.Response.ContentType = "application/json";
    context.Response.StatusCode = (int)response.StatusCode;
    await context.Response.WriteAsJsonAsync(response);
  }
}

public record ExceptionResponse(HttpStatusCode StatusCode, string Description);