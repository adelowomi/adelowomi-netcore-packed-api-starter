using System;
using Adelowomi.Models.ConfigModels;
using Azure.Identity;

namespace Adelowomi.Extensions;

public static class ConfigurationExtensions
{
    public static IServiceCollection AddAppConfiguration(
            this IServiceCollection services,
            IConfiguration configuration)
    {
        // Bind and register the entire configuration
        services.Configure<AppConfig>(
            configuration.GetSection("App"));

        return services;
    }

    public static T GetConfig<T>(this IConfiguration configuration, string section) where T : class
    {
        return configuration.GetSection(section).Get<T>();
    }


    public static WebApplicationBuilder AddConfiguration(this WebApplicationBuilder builder)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(builder.Environment.ContentRootPath)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        builder.Configuration.AddKeyVaultConfiguration(configuration);

        builder.Services.AddAppConfiguration(configuration);

        return builder;
    }

    public static IConfigurationBuilder AddKeyVaultConfiguration(this IConfigurationBuilder builder, IConfiguration configuration)
    {
        string keyVaultUri = configuration["KeyVault:Uri"]
           ?? throw new InvalidOperationException("Key Vault URI is not configured.");

        builder.AddAzureKeyVault(
            new Uri(keyVaultUri),
            new DefaultAzureCredential());

        return builder;
    }
}
