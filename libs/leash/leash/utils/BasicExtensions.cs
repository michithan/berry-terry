using System.Text;

namespace leash.utils;

public static class BasicExtensions
{
    public static int ToInt(this string value) => int.Parse(value);

    public static T ToType<T>(this object value) => (T)value;

    public static bool IsTypeOf<T>(this Type type) => type == typeof(T);

    public static string ConvertFromBase64(this string value) => Encoding.UTF8.GetString(Convert.FromBase64String(value));

    public static bool ContainsAny(this string text, params string[] values) =>
        text.ContainsAny(StringComparison.OrdinalIgnoreCase, values);

    public static bool ContainsAny(this string text, StringComparison comparison, params string[] values) =>
        values.Any(value => text.Contains(value, comparison));

    public static string JoinToString(this IEnumerable<string> strings) =>
        string.Join("", strings);
}