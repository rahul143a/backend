using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;

namespace Infrastructure.Database.Extensions;

/// <summary>
/// Extensions for JSON conversion in entity configurations
/// </summary>
public static class JsonConversionExtensions
{
    /// <summary>
    /// Configure a property to be stored as JSON
    /// </summary>
    public static PropertyBuilder<T> HasJsonConversion<T>(this PropertyBuilder<T> propertyBuilder)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        var converter = new ValueConverter<T, string>(
            v => JsonSerializer.Serialize(v, options),
            v => v == null ? default : JsonSerializer.Deserialize<T>(v, options));

        propertyBuilder.HasConversion(converter);
        return propertyBuilder;
    }
}
