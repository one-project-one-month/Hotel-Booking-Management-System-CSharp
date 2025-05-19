using HotelManagementSystem.Service.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace HotelManagementSystem.Helpers
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;
        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var (statusCode, message) = GetExceptionDetails(exception);
            _logger.LogError(exception, message);
            httpContext.Response.StatusCode = (int)statusCode;
            await httpContext.Response.WriteAsJsonAsync(message, cancellationToken);

            return true;
        }
        private (HttpStatusCode statusCode, string message) GetExceptionDetails(Exception exception)
        {
            return exception switch
            {
                UserNotFoundException => (HttpStatusCode.Unauthorized, exception.Message),
                RoleDoesNotExistException => (HttpStatusCode.BadRequest, exception.Message),
                OTPNotFoudException => (HttpStatusCode.Unauthorized, exception.Message),
                UserDoesNotExitException => (HttpStatusCode.Unauthorized, exception.Message),

                _ => (HttpStatusCode.InternalServerError, $"An unexpected error occurred: {exception.Message}")
            };
        }
    }
}
