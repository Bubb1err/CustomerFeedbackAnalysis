using FeedbackAnalysis.API.Models;
using FeedbackAnalysis.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using System.Text.Json;

namespace FeedbackAnalysis.API.Middlewares
{
    internal class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (ExecutingException ex)
            {
                _logger.LogError(ex, "An exception occurred: {Message}", ex.Message);

                await HandleExceptionAsync(httpContext, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An exception occurred: {Message}", ex.Message);

                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
        {
            httpContext.Response.ContentType = "application/json";

            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            string response = JsonSerializer.Serialize(new ErrorResponse(HttpStatusCode.InternalServerError, "Error occurred"));

            await httpContext.Response.WriteAsync(response);
        }

        private static async Task HandleExceptionAsync(HttpContext httpContext, ExecutingException exception)
        {
            httpContext.Response.ContentType = "application/json";

            httpContext.Response.StatusCode = (int)exception.StatusCode;

            string response = JsonSerializer.Serialize(new ErrorResponse(exception.StatusCode, exception.ErrorMessage));

            await httpContext.Response.WriteAsync(response);
        }
    }

    internal static class ExceptionHandlingMiddlewareExtensions
    {
        internal static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder builder)
            => builder.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}
