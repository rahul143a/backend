using Abstraction.Common;
using Domain.Common.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.Database;

/// <summary>
/// Audit trail for entity changes
/// </summary>
public class AuditTrail
{
    /// <summary>
    /// Constructor
    /// </summary>
    public AuditTrail(EntityEntry entry, ICurrentUser currentUser)
    {
        Entry = entry;
        UserId = currentUser.GetUserId();
        TenantId = currentUser.GetTenant();
        TrailType = TrailType.None;
        OldValues = new Dictionary<string, object?>();
        NewValues = new Dictionary<string, object?>();
        ChangedColumns = new List<string>();
        KeyValues = new Dictionary<string, object?>();
        TemporaryProperties = new List<PropertyEntry>();
    }

    /// <summary>
    /// Entity entry
    /// </summary>
    public EntityEntry Entry { get; }

    /// <summary>
    /// User ID
    /// </summary>
    public string? UserId { get; set; }

    /// <summary>
    /// Tenant ID
    /// </summary>
    public string? TenantId { get; set; }

    /// <summary>
    /// Table name
    /// </summary>
    public string? TableName { get; set; }

    /// <summary>
    /// Trail type
    /// </summary>
    public TrailType TrailType { get; set; }

    /// <summary>
    /// Old values
    /// </summary>
    public Dictionary<string, object?> OldValues { get; }

    /// <summary>
    /// New values
    /// </summary>
    public Dictionary<string, object?> NewValues { get; }

    /// <summary>
    /// Changed columns
    /// </summary>
    public List<string> ChangedColumns { get; }

    /// <summary>
    /// Key values
    /// </summary>
    public Dictionary<string, object?> KeyValues { get; }

    /// <summary>
    /// Temporary properties
    /// </summary>
    public List<PropertyEntry> TemporaryProperties { get; }

    /// <summary>
    /// Whether the entity has temporary properties
    /// </summary>
    public bool HasTemporaryProperties => TemporaryProperties.Any();
}

/// <summary>
/// Trail type
/// </summary>
public enum TrailType
{
    /// <summary>
    /// None
    /// </summary>
    None = 0,

    /// <summary>
    /// Create
    /// </summary>
    Create = 1,

    /// <summary>
    /// Update
    /// </summary>
    Update = 2,

    /// <summary>
    /// Delete
    /// </summary>
    Delete = 3
}
