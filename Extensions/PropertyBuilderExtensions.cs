using System.Text.Json;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DrzewaAPI.Extensions;

public static class PropertyBuilderExtensions
{
	public static PropertyBuilder<T> HasJsonConversion<T>(this PropertyBuilder<T> propertyBuilder, JsonSerializerOptions? options = null)
	{
		options ??= JsonSerializerOptions.Default;

		propertyBuilder.HasConversion(
			v => JsonSerializer.Serialize(v, options),
			v => JsonSerializer.Deserialize<T>(v, options)!
		);

		var comparer = new ValueComparer<T>(
			(l, r) => JsonSerializer.Serialize(l, options) == JsonSerializer.Serialize(r, options),
						v => JsonSerializer.Serialize(v, options).GetHashCode(),
						v => JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(v, options), options)!
		);

		propertyBuilder.Metadata.SetValueComparer(comparer);

		return propertyBuilder;
	}
}
