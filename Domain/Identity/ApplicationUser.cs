using Domain.Common.Contracts;
using Microsoft.AspNetCore.Identity;

namespace Domain.Identity;

/// <summary>
/// Application user entity
/// </summary>
public class ApplicationUser : IdentityUser, IAggregateRoot
{
    /// <summary>
    /// First name
    /// </summary>
    public string? FirstName { get; set; }

    /// <summary>
    /// Last name
    /// </summary>
    public string? LastName { get; set; }

    /// <summary>
    /// Profile image URL
    /// </summary>
    public string? ImageUrl { get; set; }

    /// <summary>
    /// Whether the user is active
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Password timestamp for tracking password changes
    /// </summary>
    public DateTime PasswordTimeStamp { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Whether MFA is enabled for the user
    /// </summary>
    public bool IsMFAEnabled { get; set; }

    /// <summary>
    /// One-time password for MFA
    /// </summary>
    public string? OTP { get; set; }

    /// <summary>
    /// When the OTP was last updated
    /// </summary>
    public DateTime? OTPUpdatedOn { get; set; }

    /// <summary>
    /// Refresh token for JWT authentication
    /// </summary>
    public string? RefreshToken { get; set; }

    /// <summary>
    /// Refresh token expiry time
    /// </summary>
    public DateTime RefreshTokenExpiryTime { get; set; }

    /// <summary>
    /// When the user was last modified
    /// </summary>
    public DateTime? LastModifiedOn { get; set; }

    /// <summary>
    /// When the user was created
    /// </summary>
    public DateTime? CreatedOn { get; set; }

    /// <summary>
    /// ID of the user who created this user
    /// </summary>
    public Guid CreatedBy { get; set; }

    /// <summary>
    /// ID of the user who last modified this user
    /// </summary>
    public Guid LastModifiedBy { get; set; }

    /// <summary>
    /// Whether the user is deleted (soft delete)
    /// </summary>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// When the user was deleted
    /// </summary>
    public DateTime? DeletedOn { get; set; }

    /// <summary>
    /// ID of the user who deleted this user
    /// </summary>
    public Guid? DeletedBy { get; set; }

    /// <summary>
    /// External object ID (for external authentication)
    /// </summary>
    public string? ObjectId { get; set; }
}
