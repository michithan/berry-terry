using Microsoft.TeamFoundation.SourceControl.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.Identity;
using Microsoft.VisualStudio.Services.Identity.Client;
using Microsoft.VisualStudio.Services.WebApi;

namespace leash.clients.azuredevops;

public class AzureDevOpsClientCore
{
    private VssConnection Connection { get; init; }

    private IdentitySelf? _IdentitySelf { get; set; }
    public IdentitySelf IdentitySelf
    {
        get
        {
            _IdentitySelf ??= IdentityHttpClient.GetIdentitySelfAsync().Result;
            return _IdentitySelf;
        }
    }

    public GitHttpClient GitHttpClient => GetHttpClientAsync<GitHttpClient>().Result;

    public WorkItemTrackingHttpClient WorkItemTrackingHttpClient => GetHttpClientAsync<WorkItemTrackingHttpClient>().Result;

    public IdentityHttpClient IdentityHttpClient => GetHttpClientAsync<IdentityHttpClient>().Result;

    public AzureDevOpsClientCore(AzureDevOpsClientConfiguration azureDevOpsClientConfiguration)
    {
        Uri organizationUri = new($"https://dev.azure.com/{azureDevOpsClientConfiguration.Organization}");
        VssBasicCredential basicCredential = new(string.Empty, azureDevOpsClientConfiguration.Token);
        Connection = new VssConnection(organizationUri, basicCredential);
    }

    private Task ConnectAsync() => Connection.HasAuthenticated ? Task.CompletedTask : Connection.ConnectAsync();

    private async Task<T> GetHttpClientAsync<T>() where T : VssHttpClientBase
    {
        await ConnectAsync();
        return Connection.GetClient<T>();
    }
}