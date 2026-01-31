using System.Net;
using System.Text.Json;

public class Exceptions
{
    private readonly RequestDelegate _next;
    private readonly ILogger<Exceptions> _logger;
    //delegate parameiterise the function
    public Exceptions(RequestDelegate next, ILogger<Exceptions> logger)
    {
        _next = next;
        _logger = logger; //for logging errors
    }

    public async Task InvokeAsync(HttpContext context) //This is the entry point. This runs after the middleware is registered
    {
        try
        {
            await _next(context); //passes the request to the next middleware component
        }
        catch (Exception ex) //catches any unhandled exceptions
        {
            _logger.LogError(ex, "An unhandled exception occurred.");
            await HandleExceptionAsync(context, ex);
        }
    }
    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var statusCode = exception switch //if-else -> switch expression
        {
            KeyNotFoundException => HttpStatusCode.NotFound,
            InvalidEmotionException => HttpStatusCode.BadRequest,
            _ => HttpStatusCode.InternalServerError
        };

        context.Response.StatusCode = (int)statusCode;

        var response = new
        {
            error = $"{context.Response.StatusCode} {statusCode}", //e,g 404 Not Found
            details = exception.Message
        };

        return context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}