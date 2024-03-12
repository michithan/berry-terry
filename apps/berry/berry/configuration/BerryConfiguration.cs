using leash.clients.azuredevops;
using leash.clients.google;
using leash.utils;

namespace berry.configuration;

public class BerryConfiguration : ConfigurationBase
{
    private string? _BotName { get; set; }
    public required string BotName
    {
        get => GetEnvironmentVariableOrDefault(nameof(BotName), _BotName);
        init => _BotName = value;
    }

    public required AiConfiguration AiConfiguration { get; init; }

    public required AzureDevOpsClientConfiguration AzureDevOpsClientConfiguration { get; init; }

    public required GoogleClientConfiguration GoogleClientConfiguration { get; init; }
}