using System;
using Adelowomi.Models.Context;
using Adelowomi.Models.IdentityModels;
using Adelowomi.Services;
using Microsoft.AspNetCore.Identity;

namespace Adelowomi.Extensions;

public static class IdentityExtensions
{
    public static void AddCustomUserIdentity(this IServiceCollection services)
    {
        services.AddIdentityApiEndpoints<User>(AddIdentityOptions)
        .AddRoles<Role>()
        .AddRoleManager<RoleManager<Role>>()
        .AddEntityFrameworkStores<Context>();


        services.AddTransient<IEmailSender<User>, IdentityEmailSender>();
    }

    public static void MapIdentityEndpoints(this IEndpointRouteBuilder endpointBuilder)
    {
        var routeGroup = endpointBuilder.MapGroup("/api/auth")
                                        .WithTags(nameof(User))
                                        .MapIdentityApi<User>();
    }

    public static void AddIdentityOptions(IdentityOptions options)
    {
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequireUppercase = true;
        options.Password.RequiredLength = 8;
        options.Password.RequiredUniqueChars = 1;

        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.AllowedForNewUsers = true;

        options.User.AllowedUserNameCharacters =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
        options.User.RequireUniqueEmail = true;
        options.SignIn.RequireConfirmedEmail = true;
    }
}
