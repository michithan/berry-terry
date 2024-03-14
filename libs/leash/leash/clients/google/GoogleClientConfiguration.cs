using leash.utils;

namespace leash.clients.google;

public class GoogleClientConfiguration : ConfigurationBase
{
    private string? _UserName { get; set; }
    public required string UserName
    {
        get => GetEnvironmentVariableOrDefault(nameof(UserName), _UserName);
        init => _UserName = value;
    }

    private string? _AccessKeyJson { get; set; }
    public required string AccessKeyJson
    {
        get => GetEnvironmentVariableOrDefault(nameof(AccessKeyJson), _AccessKeyJson);
        init => _AccessKeyJson = value;
    }
}