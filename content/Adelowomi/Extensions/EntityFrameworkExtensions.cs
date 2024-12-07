using System;
using Adelowomi.Models.ConfigModels;
using Adelowomi.Models.Context;
using Adelowomi.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Adelowomi.Extensions;

public static class EntityFrameworkExtensions
{
    // add dbcontext extension methods here
    public static IServiceCollection AddAppDbContext<TContext>(
        this IServiceCollection services,
        IConfiguration configuration) where TContext : DbContext
    {
        var assembly = typeof(TContext).Assembly.GetName().Name;
        var appConfig = configuration.GetConfig<AppConfig>("App");
        services.AddDbContext<TContext>(options =>
        {
            options.UseMySql(appConfig.ConnectionString, MySqlServerVersion.AutoDetect(appConfig.ConnectionString), b => b.MigrationsAssembly(assembly)).UseCamelCaseNamingConvention();
            options.UseOpenIddict<int>();
        });

        services.AddScoped<IRepositoryFactory>(provider =>
                new RepositoryFactory(provider.GetRequiredService<Context>()));

        return services;
    }
}
