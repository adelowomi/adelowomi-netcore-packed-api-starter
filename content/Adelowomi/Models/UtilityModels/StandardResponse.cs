using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Adelowomi.Models.UtilityModels;

/// <summary>
/// Represents a standardized API response wrapper for consistent response formatting
/// </summary>
/// <typeparam name="T">The type of the response data</typeparam>
public class StandardResponse<T>
{
    /// <summary>
    /// Indicates if the request was successful
    /// </summary>
    public bool Success { get; private set; }

    /// <summary>
    /// Response message providing additional context
    /// </summary>
    public string? Message { get; private set; }

    /// <summary>
    /// The response payload
    /// </summary>
    public T? Data { get; private set; }

    /// <summary>
    /// HTTP status code of the response
    /// </summary>
    public HttpStatusCode StatusCode { get; private set; }

    /// <summary>
    /// Optional error details
    /// </summary>
    public object? Errors { get; private set; }

    /// <summary>
    /// Timestamp of when the response was created
    /// </summary>
    public DateTimeOffset Timestamp { get; private set; } = DateTimeOffset.UtcNow;

    private StandardResponse(
        bool success,
        string? message,
        T? data,
        HttpStatusCode statusCode,
        object? errors = null)
    {
        Success = success;
        Message = message;
        Data = data;
        StatusCode = statusCode;
        Errors = errors;
    }

    #region Success Responses

    /// <summary>
    /// Creates a successful response with default OK status
    /// </summary>
    public static StandardResponse<T> Ok(T data) =>
        new(true, "Operation completed successfully", data, HttpStatusCode.OK);

    /// <summary>
    /// Creates a successful response with custom message
    /// </summary>
    public static StandardResponse<T> Ok(T data, string message) =>
        new(true, message, data, HttpStatusCode.OK);

    /// <summary>
    /// Creates a successful response with no data
    /// </summary>
    public static StandardResponse<T> Ok(string message) =>
        new(true, message, default, HttpStatusCode.OK);

    /// <summary>
    /// Creates a successful response for resource creation
    /// </summary>
    public static StandardResponse<T> Created(T data) =>
        new(true, "Resource created successfully", data, HttpStatusCode.Created);

    #endregion

    #region Error Responses

    /// <summary>
    /// Creates an error response with Internal Server Error status
    /// </summary>
    public static StandardResponse<T> Error(string message, object? errors = null) =>
        new(false, message, default, HttpStatusCode.InternalServerError, errors);

    /// <summary>
    /// Creates an error response with custom status code
    /// </summary>
    public static StandardResponse<T> Error(string message, HttpStatusCode statusCode, object? errors = null) =>
        new(false, message, default, statusCode, errors);

    /// <summary>
    /// Creates a Bad Request response
    /// </summary>
    public static StandardResponse<T> BadRequest(string message, object? errors = null) =>
        new(false, message, default, HttpStatusCode.BadRequest, errors);

    /// <summary>
    /// Creates a Not Found response
    /// </summary>
    public static StandardResponse<T> NotFound(string message = "Resource not found") =>
        new(false, message, default, HttpStatusCode.NotFound);

    /// <summary>
    /// Creates an Unauthorized response
    /// </summary>
    public static StandardResponse<T> Unauthorized(string message = "Unauthorized access") =>
        new(false, message, default, HttpStatusCode.Unauthorized);

    /// <summary>
    /// Creates a Forbidden response
    /// </summary>
    public static StandardResponse<T> Forbidden(string message = "Access forbidden") =>
        new(false, message, default, HttpStatusCode.Forbidden);

    #endregion

    #region Fluent Methods

    /// <summary>
    /// Adds or updates the response message
    /// </summary>
    public StandardResponse<T> WithMessage(string message)
    {
        Message = message;
        return this;
    }

    /// <summary>
    /// Adds or updates error details
    /// </summary>
    public StandardResponse<T> WithErrors(object errors)
    {
        Errors = errors;
        return this;
    }

    #endregion
}

/// <summary>
/// Base controller providing standardized response handling
/// </summary>
public class StandardControllerBase : ControllerBase
{
    /// <summary>
    /// Returns an appropriate ObjectResult based on the StandardResponse status code
    /// </summary>
    protected IActionResult Response<T>(StandardResponse<T> response) => response.StatusCode switch
    {
        HttpStatusCode.OK => Ok(response),
        HttpStatusCode.Created => StatusCode((int)HttpStatusCode.Created, response),
        HttpStatusCode.BadRequest => BadRequest(response),
        HttpStatusCode.Unauthorized => Unauthorized(response),
        HttpStatusCode.Forbidden => StatusCode((int)HttpStatusCode.Forbidden, response),
        HttpStatusCode.NotFound => NotFound(response),
        HttpStatusCode.InternalServerError => StatusCode((int)HttpStatusCode.InternalServerError, response),
        _ => StatusCode((int)response.StatusCode, response)
    };
}
