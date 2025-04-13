using System.Net;
using System.Text.Json;

namespace ReportProject.Api.Middleware
{
    public class ErrorMiddleware// התשובה מופיעה בסווגר
    // לטיפול בשגיאות                          
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorMiddleware> _logger;

        public ErrorMiddleware(RequestDelegate next, ILogger<ErrorMiddleware> logger)
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
                _logger.LogError(ex, "-------------Error occurred.------------"); // לוגינג של החריגה ופרטיה
                context.Response.StatusCode = 500;
                await context.Response.WriteAsJsonAsync(new { message = "Middleware:---------------------An internal server error has occurred.------------------" });
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "--------------------application/json-----------------------";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            var response = new { message = "Middleware:---------------------An error occurred.------------------" };
            var jsonResponse = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(jsonResponse);
        }
    }
}
