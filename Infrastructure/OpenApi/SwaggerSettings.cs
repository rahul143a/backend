namespace Infrastructure.OpenApi;

/// <summary>
/// Swagger settings
/// </summary>
public class SwaggerSettings
{
    /// <summary>
    /// Enable Swagger
    /// </summary>
    public bool Enable { get; set; }

    /// <summary>
    /// API title
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// API version
    /// </summary>
    public string Version { get; set; } = string.Empty;

    /// <summary>
    /// API description
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Contact name
    /// </summary>
    public string ContactName { get; set; } = string.Empty;

    /// <summary>
    /// Contact email
    /// </summary>
    public string ContactEmail { get; set; } = string.Empty;

    /// <summary>
    /// Contact URL
    /// </summary>
    public string ContactUrl { get; set; } = string.Empty;

    /// <summary>
    /// Enable license
    /// </summary>
    public bool License { get; set; }

    /// <summary>
    /// License name
    /// </summary>
    public string LicenseName { get; set; } = string.Empty;

    /// <summary>
    /// License URL
    /// </summary>
    public string LicenseUrl { get; set; } = string.Empty;
}
