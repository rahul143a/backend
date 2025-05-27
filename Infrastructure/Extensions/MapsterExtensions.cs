using Mapster;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Infrastructure.Extensions;

/// <summary>
/// Extensions for Mapster
/// </summary>
public static class MapsterExtensions
{
    /// <summary>
    /// Add Mapster to the service collection
    /// </summary>
    public static IServiceCollection AddMapster(this IServiceCollection services)
    {
        // Register Mapster configuration
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(Assembly.GetExecutingAssembly());
        
        // Register Mapster as a singleton
        services.AddSingleton(config);
        
        return services;
    }
}
