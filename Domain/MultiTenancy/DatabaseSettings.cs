namespace Domain.MultiTenancy;

/// <summary>
/// Database settings
/// </summary>
public class DatabaseSettings
{
    /// <summary>
    /// Static connection string for direct access
    /// </summary>
    public static string ConnectionString { get; set; } = "Host=qa-lrb.postgres.database.azure.com;Port=5432;Database=inventoryManagement;Username=dbmasteruser;Password=8KvzkWW=e,N^XbYPL5}kLfuNoNyNGgBz";

    /// <summary>
    /// Static database provider
    /// </summary>
    public static string DBProvider { get; set; } = "PostgreSQL";

    /// <summary>
    /// Database connection string (instance property)
    /// </summary>
    public string ConnectionStr { get; set; } = string.Empty;

    /// <summary>
    /// Database provider
    /// </summary>
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
