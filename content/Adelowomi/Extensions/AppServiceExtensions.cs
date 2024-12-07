using System;
using Adelowomi.Models.Context;

namespace Adelowomi.Extensions;

public static class AppServiceExtensions
{
    public static void AddAppServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAppDbContext<Context>(configuration);

        services.AddCustomUserIdentity();

        services.AddControllers();

        services.AddGlobalExceptionHandler();
        services.AddStandardResponseInterceptor();
    }
}
