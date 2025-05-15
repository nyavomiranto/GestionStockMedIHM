using GestionStockMedIHM.Domain.DTOs.Responses;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;


namespace GestionStockMedIHM.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
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
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Resource not found");
                await HandleExceptionAsync(context, ex, StatusCodes.Status404NotFound);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validation error");
                await HandleValidationExceptionAsync(context, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Server error");
                await HandleExceptionAsync(context, ex, StatusCodes.Status500InternalServerError);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception, int statusCode)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var response = ApiResponse<object>.ErrorResponse(
                statusCode == StatusCodes.Status500InternalServerError
                    ? "An unexpected error occurred"
                    : exception.Message,
                new List<string> { exception.Message });

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }

        private static async Task HandleValidationExceptionAsync(HttpContext context, ValidationException exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            var errors = new List<string> { exception.Message };

            var response = ApiResponse<object>.ErrorResponse("Validation failed", errors);
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}