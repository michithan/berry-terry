namespace leash.utils;

public class ConfigurationBase
{
    public string GetEnvironmentVariableOrDefault(string name, string? defaultValue) =>
        Environment.GetEnvironmentVariable($"{GetType().Name}_{name}")
        ?? defaultValue
        ?? throw new ArgumentNullException(name);
}