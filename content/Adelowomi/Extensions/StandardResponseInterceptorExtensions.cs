using System;
using Adelowomi.Utilities;

namespace Adelowomi.Extensions;

// <summary>
/// Extensions to register the response interceptor
/// </summary>
public static class StandardResponseInterceptorExtensions
{
    public static IServiceCollection AddStandardResponseInterceptor(this IServiceCollection services)
    {
        services.AddMvcCore(options =>
        {
            options.Filters.Add<StandardResponseInterceptor>();
        });

        return services;
    }
}
