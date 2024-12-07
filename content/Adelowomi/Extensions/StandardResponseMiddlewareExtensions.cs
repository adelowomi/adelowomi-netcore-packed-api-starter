using System;
using Adelowomi.Utilities;

namespace Adelowomi.Extensions;

public static class StandardResponseMiddlewareExtensions
{
    public static IApplicationBuilder UseStandardResponse(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<StandardResponseMiddleware>();
    }
}
