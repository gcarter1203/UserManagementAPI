using System.Text.Json;

// Defines the namespace for the middleware
namespace UserManagementAPI.Middleware;

public class TokenValidationMiddleware
{
    // Reference to the next middleware in the pipeline
    private readonly RequestDelegate _next;

    // Constructor called by dependency injection
    public TokenValidationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    // Executes for every incoming HTTP request
    public async Task InvokeAsync(HttpContext context)
    {
        // Check if the Authorization header exists
        if (!context.Request.Headers.TryGetValue("Authorization", out var token))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";

            var response = new
            {
                error = "Authorization token is missing."
            };

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(response));

            return;
        }

        // Example valid token for testing
        const string validToken = "Bearer MySecretToken";

        // Validate the token
        if (token != validToken)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";

            var response = new
            {
                error = "Invalid token."
            };

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(response));

            return;
        }

        // Token is valid, continue processing the request
        await _next(context);
    }
}