namespace Shared.MultiTenancy;

/// <summary>
/// Constants for multi-tenancy
/// </summary>
public static class MultitenancyConstants
{
    /// <summary>
    /// Default tenant ID
    /// </summary>
    public const string DefaultTenantId = "demo";

    /// <summary>
    /// Root tenant ID
    /// </summary>
    public const string RootTenantId = "root";

    /// <summary>
    /// Tenant ID header name
    /// </summary>
    public const string TenantIdName = "tenant";

    /// <summary>
    /// Tenant ID claim key
    /// </summary>
    public const string TenantIdKey = "tenant";

    /// <summary>
    /// Root tenant information
    /// </summary>
    public static class Root
    {
        /// <summary>
        /// Root tenant ID
        /// </summary>
        public const string Id = RootTenantId;

        /// <summary>
        /// Root tenant name
        /// </summary>
        public const string Name = "Root";

        /// <summary>
        /// Root tenant email
        /// </summary>
        public const string EmailAddress = "admin@root.com";
    }

    /// <summary>
    /// Default tenant information
    /// </summary>
    public static class Demo
    {
        /// <summary>
        /// Demo tenant ID
        /// </summary>
        public const string Id = DefaultTenantId;

        /// <summary>
        /// Demo tenant name
        /// </summary>
        public const string Name = "Demo";

        /// <summary>
        /// Demo tenant email
        /// </summary>
        public const string EmailAddress = "admin@demo.com";
    }
}
