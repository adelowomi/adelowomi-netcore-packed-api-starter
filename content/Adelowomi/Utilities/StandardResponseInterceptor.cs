using System;
using System.Net;
using Adelowomi.Models.UtilityModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Adelowomi.Utilities;

/// <summary>
/// Ensures all API responses follow the StandardResponse format
/// </summary>
public class StandardResponseInterceptor : IAsyncActionFilter, IAsyncResultFilter
{
    /// <summary>
    /// Intercepts the action execution to handle the response
    /// </summary>
    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next)
    {
        var executedContext = await next();

        // If an exception occurred, let the exception handler middleware handle it
        if (executedContext.Exception != null && !executedContext.ExceptionHandled)
        {
            return;
        }
    }

    /// <summary>
    /// Intercepts the result execution to wrap the response
    /// </summary>
    public async Task OnResultExecutionAsync(
        ResultExecutingContext context,
        ResultExecutionDelegate next)
    {
        if (context.Result is ObjectResult objectResult)
        {
            // Don't wrap if it's already a StandardResponse
            if (IsStandardResponse(objectResult.Value))
            {
                await next();
                return;
            }

            // Get status code from the result or default to 200
            var statusCode = objectResult.StatusCode ?? (int)HttpStatusCode.OK;
            var standardResponse = WrapResponse(objectResult.Value, statusCode);

            context.Result = new ObjectResult(standardResponse)
            {
                StatusCode = statusCode
            };
        }
        else if (context.Result is StatusCodeResult statusCodeResult)
        {
            var standardResponse = WrapResponse(null, statusCodeResult.StatusCode);

            context.Result = new ObjectResult(standardResponse)
            {
                StatusCode = statusCodeResult.StatusCode
            };
        }
        else if (context.Result is EmptyResult)
        {
            var standardResponse = StandardResponse<object>.Ok(null);

            context.Result = new ObjectResult(standardResponse)
            {
                StatusCode = (int)HttpStatusCode.OK
            };
        }

        await next();
    }

    private bool IsStandardResponse(object? value)
    {
        if (value == null) return false;

        var type = value.GetType();
        return type.IsGenericType &&
               type.GetGenericTypeDefinition() == typeof(StandardResponse<>);
    }

    private static StandardResponse<object?> WrapResponse(object? value, int statusCode)
    {
        return (HttpStatusCode)statusCode switch
        {
            HttpStatusCode.OK => StandardResponse<object?>.Ok(value),
            HttpStatusCode.Created => StandardResponse<object?>.Created(value),
            HttpStatusCode.NoContent => StandardResponse<object?>.Ok(null),
            HttpStatusCode.BadRequest => StandardResponse<object?>.BadRequest("Bad Request", value),
            HttpStatusCode.Unauthorized => StandardResponse<object?>.Unauthorized(),
            HttpStatusCode.Forbidden => StandardResponse<object?>.Error("Forbidden", HttpStatusCode.Forbidden),
            HttpStatusCode.NotFound => StandardResponse<object?>.NotFound(),
            _ => StandardResponse<object?>.Error(
                "An error occurred",
                (HttpStatusCode)statusCode,
                value)
        };
    }
}
