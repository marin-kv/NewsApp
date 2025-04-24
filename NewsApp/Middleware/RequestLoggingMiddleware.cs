namespace NewsApp.Middleware;

public class RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<RequestLoggingMiddleware> _logger = logger;

    public async Task InvokeAsync(HttpContext context)
    {
        var request = context.Request;

        _logger.LogInformation("Incoming Request: {method} {path} from {ip}",
            request.Method,
            request.Path,
            context.Connection.RemoteIpAddress
        );

        await _next(context); // Call the next middleware
    }
}

