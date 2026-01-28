using System.Net;
using System.Text.Json;

public class Exceptions
{
    private readonly RequestDelegate _next;
    private readonly ILogger<Exceptions> _logger;

    public Exceptions(RequestDelegate next, ILogger<Exceptions> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred.");
            await HandleExceptionAsync(context, ex);
        }
    }
    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        // Default to 502/503 for technical failures
        var statusCode = HttpStatusCode.BadGateway;
        var errorTitle = "TMDb is down / fails";

        // If it's a 404 (KeyNotFoundException), map it to your requirements
        if (exception is KeyNotFoundException)
        {
            statusCode = HttpStatusCode.NotFound;
            errorTitle = "Movie not found";
        }else if (exception is InvalidEmotionException)
        {
            statusCode = HttpStatusCode.BadRequest; // 400
            errorTitle = "Invalid emotion names";
        }
        context.Response.StatusCode = (int)statusCode;

        // Create the exact JSON shape from your requirements
        var response = new
        {
            error = errorTitle,
            details = exception.Message // This pulls the "TMDb returned 404 for movieId..." message
        };

        return context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
    public class InvalidEmotionException : Exception
    {
        public InvalidEmotionException(string message) : base(message) { }
    }
}