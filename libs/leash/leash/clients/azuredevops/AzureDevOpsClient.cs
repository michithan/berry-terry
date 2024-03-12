using Microsoft.TeamFoundation.SourceControl.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.Identity;
using Microsoft.VisualStudio.Services.Identity.Client;
using Microsoft.VisualStudio.Services.WebApi;

namespace leash.clients.azuredevops;

public class AzureDevOpsClient : IAzureDevOpsClient
{
    private VssConnection Connection { get; init; }

    private string Project { get; init; }

    private string RepositoryId { get; init; }

    private GitHttpClient GitHttpClient => GetHttpClientAsync<GitHttpClient>().Result;

    private WorkItemTrackingHttpClient WorkItemTrackingHttpClient => GetHttpClientAsync<WorkItemTrackingHttpClient>().Result;

    private IdentityHttpClient IdentityHttpClient => GetHttpClientAsync<IdentityHttpClient>().Result;

    private IdentitySelf? IdentitySelf { get; set; }

    public AzureDevOpsClient(AzureDevOpsClientConfiguration azureDevOpsClientConfiguration)
    {
        Project = azureDevOpsClientConfiguration.Project;
        RepositoryId = azureDevOpsClientConfiguration.RepositoryId;
        var organizationUri = new Uri($"https://dev.azure.com/{azureDevOpsClientConfiguration.Organization}");

        VssBasicCredential basicCredential = new(string.Empty, azureDevOpsClientConfiguration.Token);
        Connection = new VssConnection(organizationUri, basicCredential);
    }

    private Task ConnectAsync() => Connection.HasAuthenticated ? Task.CompletedTask : Connection.ConnectAsync();

    private async Task<IdentitySelf> IdentifyAsync()
    {
        IdentitySelf ??= await IdentityHttpClient.GetIdentitySelfAsync();
        return IdentitySelf;
    }

    private async Task<T> GetHttpClientAsync<T>() where T : VssHttpClientBase
    {
        await ConnectAsync();
        return Connection.GetClient<T>();
    }

    public async Task<IEnumerable<GitPullRequest>> GetAllPullRequestsAsync()
    {
        var searchCriteria = new GitPullRequestSearchCriteria { Status = PullRequestStatus.All };
        return await GitHttpClient.GetPullRequestsAsync(Project, RepositoryId, searchCriteria);
    }

    public Task<GitPullRequest?> GetPullRequestByIdAsync(int id) =>
        GitHttpClient.GetPullRequestByIdAsync(id);

    public Task<GitPullRequestCommentThread?> GetPullRequestThreadAsync(int pullRequestId, int threadId) =>
        GitHttpClient.GetPullRequestThreadAsync(Project, RepositoryId, pullRequestId, threadId);

    public Task<Microsoft.TeamFoundation.SourceControl.WebApi.Comment?> GetPullRequestCommentAsync(int pullRequestId, int threadId, int commentId) =>
        GitHttpClient.GetCommentAsync(Project, pullRequestId, threadId, commentId);

    public Task<Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models.Comment> GetWorkItemCommentAsync(int workItemId, int commentId) =>
        WorkItemTrackingHttpClient.GetCommentAsync(Project, workItemId, commentId);

    public Task<IdentityDescriptor> GetIdentityAsync(Guid identityId) =>
        IdentityHttpClient.GetDescriptorByIdAsync(identityId);

    public bool IsMentionedOnComment(string comment) =>
        comment.Contains(IdentifyAsync().Result.Id.ToString(), StringComparison.OrdinalIgnoreCase);

    public Task<WorkItem> GetWorkItemByIdAsync(int id) =>
        WorkItemTrackingHttpClient.GetWorkItemAsync(id, expand: WorkItemExpand.All);

    public Task<CommentList> GetWorkItemCommentsAsync(int workItemId) =>
        WorkItemTrackingHttpClient.GetCommentsAsync(Project, workItemId);

    public Task CreatePullRequestCommentAsync(Microsoft.TeamFoundation.SourceControl.WebApi.Comment comment, int pullRequestId, int threadId) =>
        GitHttpClient.CreateCommentAsync(comment, RepositoryId, pullRequestId, threadId);

    public Task CreateWorkItemCommentAsync(CommentCreate comment, int workItemId) =>
        WorkItemTrackingHttpClient.AddCommentAsync(comment, Project, workItemId);
}