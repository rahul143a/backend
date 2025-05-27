using System.Globalization;

namespace Web.Host;

/// <summary>
/// Startup configuration for configurations
/// </summary>
internal static class Startup
{
    /// <summary>
    /// Add configurations
    /// </summary>
    internal static WebApplicationBuilder AddConfigurations(this WebApplicationBuilder host)
    {
        const string configurationsDirectory = "Configurations";
        var env = host.Environment.EnvironmentName.ToLower(CultureInfo.InvariantCulture);

        // Base appsettings
        host.Configuration
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true);

        // Logging configuration
        host.Configuration
            .AddJsonFile($"{configurationsDirectory}/logger.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"{configurationsDirectory}/logger.{env}.json", optional: true, reloadOnChange: true);

        // Database configuration
        host.Configuration
            .AddJsonFile($"{configurationsDirectory}/database.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"{configurationsDirectory}/database.{env}.json", optional: true, reloadOnChange: true);

        // JWT authentication configuration
        host.Configuration
            .AddJsonFile($"{configurationsDirectory}/jwt.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"{configurationsDirectory}/jwt.{env}.json", optional: true, reloadOnChange: true);

        // OpenAPI configuration
        host.Configuration
            .AddJsonFile($"{configurationsDirectory}/openapi.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"{configurationsDirectory}/openapi.{env}.json", optional: true, reloadOnChange: true);

        // Add environment variables
        host.Configuration.AddEnvironmentVariables();

        return host;
    }
}
