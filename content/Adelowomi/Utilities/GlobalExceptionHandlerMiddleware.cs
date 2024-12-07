using System;
using System.Net;
using Adelowomi.Models.UtilityModels;

namespace Adelowomi.Utilities;

/// <summary>
/// Middleware to handle uncaught exceptions globally
/// </summary>
public class GlobalExceptionHandlerMiddleware : IMiddleware
{
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;
    private readonly IHostEnvironment _environment;

    public GlobalExceptionHandlerMiddleware(
        ILogger<GlobalExceptionHandlerMiddleware> logger,
        IHostEnvironment environment)
    {
        _logger = logger;
        _environment = environment;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "An unhandled exception occurred");
            await HandleExceptionAsync(context, exception);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        
        var response = exception switch
        {
            ValidationException validationEx => 
                HandleValidationException(validationEx),
            NotFoundException notFoundEx => 
                HandleNotFoundException(notFoundEx),
            UnauthorizedAccessException unauthorizedEx => 
                HandleUnauthorizedException(unauthorizedEx),
            // Add more specific exception types as needed
            _ => HandleUnknownException(exception)
        };

        context.Response.StatusCode = (int)response.StatusCode;
        await context.Response.WriteAsJsonAsync(response);
    }

    private StandardResponse<object> HandleValidationException(ValidationException exception)
    {
        return StandardResponse<object>.BadRequest(
            "Validation failed",
            exception.Errors);
    }

    private StandardResponse<object> HandleNotFoundException(NotFoundException exception)
    {
        return StandardResponse<object>.NotFound(
            exception.Message);
    }

    private StandardResponse<object> HandleUnauthorizedException(UnauthorizedAccessException exception)
    {
        return StandardResponse<object>.Unauthorized(
            exception.Message);
    }

    private StandardResponse<object> HandleUnknownException(Exception exception)
    {
        var errorMessage = _environment.IsDevelopment() 
            ? exception.Message 
            : "An internal server error occurred";

        var errorDetails = _environment.IsDevelopment() 
            ? new
            {
                exception.Message,
                exception.StackTrace,
                exception.Source,
                InnerMessage = exception.InnerException?.Message
            }
            : null;

        return StandardResponse<object>.Error(
            errorMessage,
            HttpStatusCode.InternalServerError,
            errorDetails);
    }
}