namespace Domain.Identity;

/// <summary>
/// JWT settings
/// </summary>
public class JwtSettings
{
    /// <summary>
    /// JWT key
    /// </summary>
    public string Key { get; set; } = string.Empty;

    /// <summary>
    /// JWT issuer
    /// </summary>
    public string Issuer { get; set; } = string.Empty;

    /// <summary>
    /// JWT audience
    /// </summary>
    public string Audience { get; set; } = string.Empty;

    /// <summary>
    /// JWT expiration in minutes
    /// </summary>
    public int ExpirationInMinutes { get; set; }

    /// <summary>
    /// Refresh token expiration in days
    /// </summary>
    public int RefreshTokenExpirationInDays { get; set; }
}
