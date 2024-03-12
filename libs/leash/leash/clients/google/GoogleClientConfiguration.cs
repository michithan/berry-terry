using leash.utils;

namespace leash.clients.google;

public class GoogleClientConfiguration : ConfigurationBase
{
    private string? _ClientId { get; set; }
    public required string ClientId
    {
        get => GetEnvironmentVariableOrDefault(nameof(ClientId), _ClientId);
        init => _ClientId = value;
    }

    private string? _ClientSecret { get; set; }
    public required string ClientSecret
    {
        get => GetEnvironmentVariableOrDefault(nameof(ClientSecret), _ClientSecret);
        init => _ClientSecret = value;
    }

    private string? _UserName { get; set; }
    public required string UserName
    {
        get => GetEnvironmentVariableOrDefault(nameof(UserName), _UserName);
        init => _UserName = value;
    }
}