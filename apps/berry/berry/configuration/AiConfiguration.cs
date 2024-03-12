using leash.utils;

namespace berry.configuration;

public class AiConfiguration : ConfigurationBase
{
    private string? _Model { get; set; }
    public string Model
    {
        get => GetEnvironmentVariableOrDefault(nameof(Model), _Model);
        init => _Model = value;
    }

    private string? _AzureEndpoint { get; set; }
    public string AzureEndpoint
    {
        get => GetEnvironmentVariableOrDefault(nameof(AzureEndpoint), _AzureEndpoint);
        init => _AzureEndpoint = value;
    }

    private string? _ApiKey { get; set; }
    public string ApiKey
    {
        get => GetEnvironmentVariableOrDefault(nameof(_ApiKey), _ApiKey);
        init => _ApiKey = value;
    }

    private string? _AnthropicApiKey { get; set; }
    public string AnthropicApiKey
    {
        get => GetEnvironmentVariableOrDefault(nameof(AnthropicApiKey), _AnthropicApiKey);
        init => _AnthropicApiKey = value;
    }
}