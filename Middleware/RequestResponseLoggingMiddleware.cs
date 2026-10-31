using System.Diagnostics;

namespace UserManagementAPI.Middleware;

public class RequestResponseLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestResponseLoggingMiddleware> _logger;

    public RequestResponseLoggingMiddleware(
        RequestDelegate next,
        ILogger<RequestResponseLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Start tracking request duration
        var stopwatch = Stopwatch.StartNew();

        // Log incoming request information
        _logger.LogInformation(
            "Request: {Method} {Path}",
            context.Request.Method,
            context.Request.Path);

        // Pass control to the next middleware
        await _next(context);

        // Stop the timer
        stopwatch.Stop();

        // Log response information
        _logger.LogInformation(
            "Response: {StatusCode} completed in {ElapsedMilliseconds} ms",
            context.Response.StatusCode,
            stopwatch.ElapsedMilliseconds);
    }
}