using System.Net;

namespace MakeMeAPix.Middlewares;

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
      // SomeException _ => new ExceptionResponse(HttpStatusCode.BadRequest, ex.Message),
      _ => new ExceptionResponse(HttpStatusCode.InternalServerError, "Internal server error. Please retry later.")
    };

    context.Response.ContentType = "application/json";
    context.Response.StatusCode = (int)response.StatusCode;
    await context.Response.WriteAsJsonAsync(response);
  }
}

public record ExceptionResponse(HttpStatusCode StatusCode, string Description);