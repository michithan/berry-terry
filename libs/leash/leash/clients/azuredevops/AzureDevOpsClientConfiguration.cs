using leash.utils;

namespace leash.clients.azuredevops;

public class AzureDevOpsClientConfiguration : ConfigurationBase
{
    private string? _Organization { get; set; }
    public required string Organization
    {
        get => GetEnvironmentVariableOrDefault(nameof(Organization), _Organization);
        init => _Organization = value;
    }

    private string? _Project { get; set; }
    public required string Project
    {
        get => GetEnvironmentVariableOrDefault(nameof(Project), _Project);
        init => _Project = value;
    }

    private string? _Token { get; set; }
    public required string Token
    {
        get => GetEnvironmentVariableOrDefault(nameof(Token), _Token);
        init => _Token = value;
    }

    private string? _RepositoryId { get; set; }
    public required string RepositoryId
    {
        get => GetEnvironmentVariableOrDefault(nameof(RepositoryId), _RepositoryId);
        init => _RepositoryId = value;
    }

    private string? _WebhookSecret { get; set; }
    public required string WebhookSecret
    {
        get => GetEnvironmentVariableOrDefault(nameof(WebhookSecret), _WebhookSecret);
        init => _WebhookSecret = value;
    }

    private string? _IdentityId { get; set; }
    public required string IdentityId
    {
        get => GetEnvironmentVariableOrDefault(nameof(IdentityId), _IdentityId);
        init => _IdentityId = value;
    }

    private string? _IdentityDisplayName { get; set; }
    public required string IdentityDisplayName
    {
        get => GetEnvironmentVariableOrDefault(nameof(IdentityDisplayName), _IdentityDisplayName);
        init => _IdentityDisplayName = value;
    }
}