using Shared.MultiTenancy;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Collections.Generic;

namespace Infrastructure.OpenApi;

/// <summary>
/// Startup configuration for OpenAPI
/// </summary>
public static class Startup
{
    /// <summary>
    /// Add OpenAPI services
    /// </summary>
    public static IServiceCollection AddOpenApiDocumentation(this IServiceCollection services, IConfiguration config)
    {
        var settings = config.GetSection(nameof(SwaggerSettings)).Get<SwaggerSettings>();
        if (settings?.Enable != true) return services;

        return services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc(settings.Version, new OpenApiInfo
            {
                Title = settings.Title,
                Version = settings.Version,
                Description = settings.Description,
                Contact = new OpenApiContact
                {
                    Name = settings.ContactName,
                    Email = settings.ContactEmail,
                    Url = new Uri(settings.ContactUrl)
                },
                License = settings.License ? new OpenApiLicense
                {
                    Name = settings.LicenseName,
                    Url = new Uri(settings.LicenseUrl)
                } : null
            });

            options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Description = "Input your Bearer token to access this API",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                BearerFormat = "JWT"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = JwtBearerDefaults.AuthenticationScheme
                        }
                    },
                    Array.Empty<string>()
                }
            });

            // Add tenant header parameter
            options.AddSecurityDefinition("Tenant", new OpenApiSecurityScheme
            {
                Name = MultitenancyConstants.TenantIdName,
                Description = "Input your tenant ID to access this API",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Tenant"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Tenant"
                        }
                    },
                    Array.Empty<string>()
                }
            });

            // Add operation filter for SwaggerHeaderAttribute
            options.OperationFilter<SwaggerHeaderAttributeProcessor>();

            options.EnableAnnotations();
            try
            {
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Inventory.Web.Host.xml"));
            }
            catch
            {
                // XML file might not exist, ignore
            }
        });
    }

    /// <summary>
    /// Use OpenAPI middleware
    /// </summary>
    public static IApplicationBuilder UseOpenApiDocumentation(this IApplicationBuilder app, IConfiguration config)
    {
        var settings = config.GetSection(nameof(SwaggerSettings)).Get<SwaggerSettings>();
        if (settings?.Enable != true) return app;

        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint($"/swagger/{settings.Version}/swagger.json", $"{settings.Title} {settings.Version}");
            options.RoutePrefix = "swagger";
            options.DocExpansion(DocExpansion.None);
            options.DefaultModelsExpandDepth(-1);
            options.DisplayRequestDuration();
        });

        return app;
    }
}
