using Finbuckle.MultiTenant;
using Abstraction.MultiTenancy;
using Domain.MultiTenancy;
using Shared.MultiTenancy;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace Infrastructure.MultiTenancy.Middleware;

/// <summary>
/// Middleware for handling tenant resolution and validation
/// </summary>
public class TenantMiddleware
{
    private readonly RequestDelegate _next;

    /// <summary>
    /// Constructor
    /// </summary>
    public TenantMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    /// <summary>
    /// Process the request
    /// </summary>
    public async Task InvokeAsync(HttpContext context)
    {
        // Check if tenant is in the header
        if (context.Request.Headers.TryGetValue(MultitenancyConstants.TenantIdName, out var tenantFromHeader) && !string.IsNullOrEmpty(tenantFromHeader))
        {
            // Get the tenant context
            var multiTenantContext = context.GetMultiTenantContext<AppTenantInfo>();

            // If tenant context is null or tenant info is null or different from the header
            if (multiTenantContext == null ||
                multiTenantContext.TenantInfo == null ||
                multiTenantContext.TenantInfo.Id != tenantFromHeader)
            {
                // Get the tenant service from the service provider
                var tenantService = context.RequestServices.GetRequiredService<ITenantService>();

                // Try to get the tenant from the service
                var headerTenantId = tenantFromHeader.ToString();
                var tenant = await tenantService.GetByIdAsync(headerTenantId);

                if (tenant != null)
                {
                    // Store the tenant in the context items
                    context.Items["CurrentTenant"] = new AppTenantInfo
                    {
                        Id = tenant.Id,
                        Identifier = tenant.Id,
                        Name = tenant.Name,
                        ConnectionString = tenant.ConnectionString,
                        AdminEmail = tenant.AdminEmail,
                        IsActive = tenant.IsActive
                    };

                    // Add tenant claim to the user if authenticated
                    if (context.User.Identity?.IsAuthenticated == true)
                    {
                        var identity = context.User.Identity as ClaimsIdentity;
                        if (identity != null)
                        {
                            // Remove existing tenant claim if any
                            var existingClaim = context.User.FindFirst(MultitenancyConstants.TenantIdKey);
                            if (existingClaim != null)
                            {
                                identity.RemoveClaim(existingClaim);
                            }

                            // Add the new tenant claim
                            identity.AddClaim(new Claim(MultitenancyConstants.TenantIdKey, tenant.Id ?? string.Empty));
                        }
                    }
                }
                else
                {
                    // Tenant not found, return a 400 Bad Request with a clear error message
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    context.Response.ContentType = "application/json";

                    var errorResponse = new
                    {
                        succeeded = false,
                        message = $"Tenant '{headerTenantId}' not found",
                        errors = new[]
                        {
                            new
                            {
                                code = "TENANT_NOT_FOUND",
                                description = $"The tenant '{headerTenantId}' specified in the request header does not exist",
                                details = new
                                {
                                    tenantId = headerTenantId
                                }
                            }
                        }
                    };

                    await context.Response.WriteAsJsonAsync(errorResponse);
                    return;
                }
            }
        }

        // Validate tenant in claims matches tenant in header
        // Check if we have both an Authorization header and a tenant header
        bool hasAuthHeader = context.Request.Headers.ContainsKey("Authorization");
        bool hasTenantHeader = context.Request.Headers.ContainsKey(MultitenancyConstants.TenantIdName);

        if (hasAuthHeader && hasTenantHeader)
        {
            // Get tenant from claims
            var tenantFromClaims = context.User?.FindFirst(MultitenancyConstants.TenantIdKey)?.Value;
            var tenantFromHeaderStr = context.Request.Headers[MultitenancyConstants.TenantIdName].ToString();

            // If both tenant values are present, validate they match
            if (!string.IsNullOrEmpty(tenantFromClaims) && !string.IsNullOrEmpty(tenantFromHeaderStr))
            {
                if (tenantFromClaims != tenantFromHeaderStr)
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    context.Response.ContentType = "application/json";

                    var errorResponse = new
                    {
                        succeeded = false,
                        message = "Tenant mismatch",
                        errors = new[]
                        {
                            new
                            {
                                code = "TENANT_MISMATCH",
                                description = $"The tenant in your token '{tenantFromClaims}' does not match the tenant in the request header '{tenantFromHeaderStr}'",
                                details = new
                                {
                                    tokenTenant = tenantFromClaims,
                                    headerTenant = tenantFromHeaderStr
                                }
                            }
                        }
                    };

                    await context.Response.WriteAsJsonAsync(errorResponse);
                    return;
                }
            }
        }

        // Call the next middleware
        await _next(context);
    }
}
