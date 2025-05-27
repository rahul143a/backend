namespace Infrastructure.OpenApi;

/// <summary>
/// Attribute to add header to Swagger documentation
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class SwaggerHeaderAttribute : Attribute
{
    /// <summary>
    /// Header name
    /// </summary>
    public string HeaderName { get; }

    /// <summary>
    /// Header description
    /// </summary>
    public string? Description { get; }

    /// <summary>
    /// Default value
    /// </summary>
    public string? DefaultValue { get; }

    /// <summary>
    /// Whether the header is required
    /// </summary>
    public bool IsRequired { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    public SwaggerHeaderAttribute(string headerName, string? description = null, string? defaultValue = null, bool isRequired = false)
    {
        HeaderName = headerName;
        Description = description;
        DefaultValue = defaultValue;
        IsRequired = isRequired;
    }
}
