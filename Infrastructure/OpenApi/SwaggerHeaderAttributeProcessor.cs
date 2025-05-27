using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace Infrastructure.OpenApi;

/// <summary>
/// Processor for SwaggerHeaderAttribute
/// </summary>
public class SwaggerHeaderAttributeProcessor : IOperationFilter
{
    /// <summary>
    /// Apply the filter to the operation
    /// </summary>
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var customAttributes = context?.MethodInfo?.GetCustomAttributes();
        if (customAttributes?.Any(i => i != null) ?? false)
        {
            foreach (var attr in customAttributes)
            {
                if (attr is SwaggerHeaderAttribute attribute)
                {
                    var parameters = operation.Parameters;

                    var existingParam = parameters.FirstOrDefault(p =>
                        p.In == ParameterLocation.Header && p.Name == attribute.HeaderName);
                    if (existingParam is not null)
                    {
                        parameters.Remove(existingParam);
                    }

                    parameters.Add(new OpenApiParameter
                    {
                        Name = attribute.HeaderName,
                        In = ParameterLocation.Header,
                        Description = attribute.Description,
                        Required = attribute.IsRequired,
                        Schema = new OpenApiSchema
                        {
                            Type = "string",
                            Default = attribute.DefaultValue != null
                                ? new Microsoft.OpenApi.Any.OpenApiString(attribute.DefaultValue)
                                : null
                        }
                    });
                }
            }
        }
    }
}
