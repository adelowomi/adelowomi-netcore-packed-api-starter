using System;
using System.Net;
using System.Text.Json;
using Adelowomi.Models.UtilityModels;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;

namespace Adelowomi.Utilities;

public class StandardResponseMiddleware
{
    private readonly RequestDelegate _next;

    public StandardResponseMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var originalBodyStream = context.Response.Body;

        try
        {
            // Create a new memory stream to capture the response
            using var memoryStream = new MemoryStream();
            context.Response.Body = memoryStream;

            // Continue down the pipeline
            await _next(context);

            // Only process API endpoints (skip static files, etc.)
            if (IsApiEndpoint(context))
            {
                memoryStream.Seek(0, SeekOrigin.Begin);
                
                string responseBody = await new StreamReader(memoryStream).ReadToEndAsync();

                object? responseObject = null;
                if (!string.IsNullOrEmpty(responseBody))
                {
                    try
                    {
                        responseObject = JsonSerializer.Deserialize<object>(responseBody);
                    }
                    catch
                    {
                        responseObject = responseBody;
                    }
                }

                // Don't wrap if it's already a StandardResponse
                if (responseObject != null && !IsStandardResponse(responseObject))
                {
                    var standardResponse = WrapResponse(responseObject, context.Response.StatusCode);
                    
                    var wrappedResponse = JsonSerializer.Serialize(standardResponse);
                    
                    // Clear the memory stream and write the wrapped response
                    memoryStream.SetLength(0);
                    using var writer = new StreamWriter(memoryStream, leaveOpen: true);
                    await writer.WriteAsync(wrappedResponse);
                    await writer.FlushAsync();
                }
            }

            memoryStream.Seek(0, SeekOrigin.Begin);
            await memoryStream.CopyToAsync(originalBodyStream);
        }
        finally
        {
            context.Response.Body = originalBodyStream;
        }
    }

    private bool IsApiEndpoint(HttpContext context)
    {
        // Check if it's an API endpoint based on path or other criteria
        var endpoint = context.Features.Get<IEndpointFeature>()?.Endpoint;
        var isApi = endpoint?.Metadata?.GetMetadata<ApiControllerAttribute>() != null ||
                   context.Request.Path.StartsWithSegments("/api");
                   
        // Check content type
        var isJsonResponse = context.Response.ContentType?.Contains("json") ?? false;
        
        return isApi && isJsonResponse;
    }

    private bool IsStandardResponse(object value)
    {
        if (value == null) return false;

        // Check if it's already a StandardResponse using type information
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
