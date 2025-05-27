using Abstraction.Common;
using Domain.MultiTenancy;
using Infrastructure.Common;
using Infrastructure.Database;
using Infrastructure.Extensions;
using Infrastructure.Identity;
using Infrastructure.MultiTenancy;
using Infrastructure.MultiTenancy.Middleware;
using Infrastructure.OpenApi;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Infrastructure;

/// <summary>
/// Startup configuration for infrastructure
/// </summary>
public static class Startup
{
    /// <summary>
    /// Add infrastructure services
    /// </summary>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config, string tenantId)
    {
        // Configure database settings with Options pattern
        services.AddOptions<DatabaseSettings>()
            .BindConfiguration(nameof(DatabaseSettings))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        return services
            .AddHttpContextAccessor()
            .AddScoped<ICurrentUser, CurrentUser>()
            .AddScoped<ICurrentUserInitializer, CurrentUser>()
            .AddVersioning()
            .AddCorsPolicy(config)
            .AddMultitenancy(config, tenantId)
            .AddPersistence(config)
            .AddIdentity()
            .AddJwtAuthentication(config)
            .AddOpenApiDocumentation(config)
            .AddApplicationServices()
            .AddRouting(options => options.LowercaseUrls = true);
    }

    /// <summary>
    /// Add infrastructure services for meta operations
    /// </summary>
    public static IServiceCollection AddInfrastructureForMeta(this IServiceCollection services, IConfiguration config)
    {
        // Configure database settings with Options pattern
        services.AddOptions<DatabaseSettings>()
            .BindConfiguration(nameof(DatabaseSettings))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        return services
            .AddHttpContextAccessor()
            .AddVersioning()
            .AddCorsPolicy(config)
            .AddMultitenancyForMeta(config);
    }

    /// <summary>
    /// Use infrastructure middleware
    /// </summary>
    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder builder, IConfiguration config)
    {
        return builder
            .UseStaticFiles()
            .UseCorsPolicy()
            .UseRouting()
            .UseAuthentication()
            .UseMultiTenancy()
            .UseTenantMiddleware()
            .UseCurrentUser()
            .UseAuthorization()
            .UseOpenApiDocumentation(config);
    }

    /// <summary>
    /// Add API versioning
    /// </summary>
    private static IServiceCollection AddVersioning(this IServiceCollection services)
    {
        return services.AddApiVersioning(config =>
        {
            config.DefaultApiVersion = new ApiVersion(1, 0);
            config.AssumeDefaultVersionWhenUnspecified = true;
            config.ReportApiVersions = true;
        });
    }

    /// <summary>
    /// Add CORS policy
    /// </summary>
    private static IServiceCollection AddCorsPolicy(this IServiceCollection services, IConfiguration config)
    {
        return services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });
    }

    /// <summary>
    /// Use CORS policy
    /// </summary>
    private static IApplicationBuilder UseCorsPolicy(this IApplicationBuilder app)
    {
        return app.UseCors("AllowAll");
    }

    /// <summary>
    /// Use current user middleware
    /// </summary>
    private static IApplicationBuilder UseCurrentUser(this IApplicationBuilder app)
    {
        return app.UseMiddleware<CurrentUserMiddleware>();
    }

    /// <summary>
    /// Add application services
    /// </summary>
    private static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Add MediatR
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Application.AssemblyReference).Assembly));

        // Add Mapster
        services.AddMapster();

        return services;
    }
}
