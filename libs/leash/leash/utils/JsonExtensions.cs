using System.Text.Json;
using System.Text.Json.Nodes;

namespace leash.utils;

public static class NotificationBodyExtensions
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new() { WriteIndented = true };

    public static string ToJsonString(this JsonElement jsonElement) =>
        JsonObject.Create(jsonElement)?.ToJsonString(JsonSerializerOptions) ?? string.Empty;

    public static string ToJsonString(this object jObject) =>
        JsonSerializer.Serialize(jObject, JsonSerializerOptions);

    public static JsonElement? GetProperty(this JsonElement notificationBody, params string[] propertyNames)
    {
        JsonElement property = notificationBody;
        foreach (string propertyName in propertyNames)
        {
            var propertyFound = property.TryGetProperty(propertyName, out property);
            if (!propertyFound)
            {
                return null;
            }
        }
        return property;
    }

    public static T? GetPropertyValue<T>(this JsonElement notificationBody, params string[] propertyNames)
    {
        T? value = default;

        JsonElement? nullableProperty = notificationBody.GetProperty(propertyNames);
        if (!nullableProperty.HasValue)
        {
            return value;
        }

        JsonElement property = nullableProperty.Value;
        var type = typeof(T);
        return property.ValueKind switch
        {
            JsonValueKind.String when type.IsTypeOf<string>() => (property.GetString() ?? string.Empty).ToType<T>(),
            JsonValueKind.Number when type.IsTypeOf<string>() => property.GetInt32().ToString().ToType<T>(),
            JsonValueKind.Number when type.IsTypeOf<int>() => property.GetInt32().ToType<T>(),
            JsonValueKind.True when type.IsTypeOf<bool>() => true.ToType<T>(),
            JsonValueKind.False when type.IsTypeOf<bool>() => false.ToType<T>(),
            _ => value
        };
    }

    public static T GetPropertyValueOrDefault<T>(this JsonElement notificationBody, params string[] propertyNames)
    {
        var type = typeof(T);
        return type switch
        {
            _ when type.IsTypeOf<string>() => GetPropertyValue<T>(notificationBody, propertyNames) ?? string.Empty.ToType<T>(),
            _ when type.IsTypeOf<int>() => GetPropertyValue<T>(notificationBody, propertyNames) ?? 0.ToType<T>(),
            _ when type.IsTypeOf<bool>() => GetPropertyValue<T>(notificationBody, propertyNames) ?? 0.ToType<T>(),
            _ => throw new NotImplementedException($"GetPropertyValueOrDefault is not implemented for Type {typeof(T)}."),
        };
    }
}
