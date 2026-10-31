using System.Text.Json;

// Defines the namespace for the middleware
namespace UserManagementAPI.Middleware;

public class ExceptionHandlingMiddleware
{
    // Reference to the next middleware in the request pipeline
    private readonly RequestDelegate _next;

    // Logger used to record exception details
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    // Constructor called by dependency injection
    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    // This method is executed for every HTTP request
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            // Pass the request to the next middleware component
            // or controller action in the pipeline
            await _next(context);
        }
        catch (Exception ex)
        {
            // Log details about the unhandled exception
            _logger.LogError(ex,
                "An unhandled exception occurred.");

            // Return HTTP 500 Internal Server Error
            context.Response.StatusCode =
                StatusCodes.Status500InternalServerError;

            // Set the response type to JSON
            context.Response.ContentType =
                "application/json";

            // Create a consistent error response object
            var response = new
            {
                error = "Internal server error."
            };

            // Convert the object to JSON
            var jsonResponse =
                JsonSerializer.Serialize(response);

            // Write the JSON response to the client
            await context.Response.WriteAsync(jsonResponse);
        }
    }
}