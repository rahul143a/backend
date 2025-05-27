using System.ComponentModel.DataAnnotations;

namespace Domain.MultiTenancy;

/// <summary>
/// Database settings
/// </summary>
public class DatabaseSettings
{
    /// <summary>
    /// Database connection string
    /// </summary>
    [Required]
    public string ConnectionString { get; set; } = string.Empty;

    /// <summary>
    /// Database provider
    /// </summary>
    [Required]
    public string Provider { get; set; } = "PostgreSQL";

    /// <summary>
    /// Enable automatic migrations
    /// </summary>
    public bool EnableAutoMigration { get; set; } = true;

    /// <summary>
    /// Enable detailed error messages
    /// </summary>
    public bool EnableDetailedErrors { get; set; } = false;

    /// <summary>
    /// Enable sensitive data logging
    /// </summary>
    public bool EnableSensitiveDataLogging { get; set; } = false;
}
