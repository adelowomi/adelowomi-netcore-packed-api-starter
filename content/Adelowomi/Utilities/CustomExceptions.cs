using System;

namespace Adelowomi.Utilities;

// <summary>
/// Custom exceptions for specific error cases
/// </summary>
public class ValidationException : Exception
{
    public object Errors { get; }

    public ValidationException(string message, object errors) : base(message)
    {
        Errors = errors;
    }
}

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message)
    {
    }
}
