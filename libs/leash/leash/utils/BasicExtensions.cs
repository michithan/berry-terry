using System.Text.Json;
using System.Text.Json.Nodes;

namespace leash.utils;

public static class BasicExtensions
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new() { WriteIndented = true };

    public static int ToInt(this string value) => int.Parse(value);

    public static string ToJsonString(this JsonElement jsonElement) =>
        JsonObject.Create(jsonElement)?.ToJsonString(JsonSerializerOptions) ?? string.Empty;

    public static string ToJsonString(this object jObject) =>
        JsonSerializer.Serialize(jObject, JsonSerializerOptions);
}