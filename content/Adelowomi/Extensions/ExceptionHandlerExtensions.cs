using System;
using Adelowomi.Utilities;

namespace Adelowomi.Extensions;

/// <summary>
/// Extension method to register the exception handler
/// </summary>
public static class ExceptionHandlerExtensions
{
    public static IServiceCollection AddGlobalExceptionHandler(this IServiceCollection services)
    {
        services.AddTransient<GlobalExceptionHandlerMiddleware>();
        return services;
    }

    public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
    {
        return app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
    }
}