namespace leash.utils;

public static class BasicExtensions
{
    public static int ToInt(this string value) => int.Parse(value);

    public static T ToType<T>(this object value) => (T)value;

    public static bool IsTypeOf<T>(this Type type) => type == typeof(T);
}