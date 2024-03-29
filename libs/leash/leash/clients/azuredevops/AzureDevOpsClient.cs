using Microsoft.TeamFoundation.SourceControl.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.Identity;

namespace leash.clients.azuredevops;

public class AzureDevOpsClient(AzureDevOpsClientConfiguration azureDevOpsClientConfiguration) : IAzureDevOpsClient
{
    private AzureDevOpsClientConfiguration AzureDevOpsClientConfiguration { get; init; } = azureDevOpsClientConfiguration;

    private AzureDevOpsClientCore AzureDevOpsClientCore { get; init; } = new(azureDevOpsClientConfiguration);

    private string Project { get; init; } = azureDevOpsClientConfiguration.Project;

    private string RepositoryId { get; init; } = azureDevOpsClientConfiguration.RepositoryId;

    public async Task<IEnumerable<GitPullRequest>> GetAllPullRequestsAsync()
    {
        var searchCriteria = new GitPullRequestSearchCriteria { Status = PullRequestStatus.All };
        return await AzureDevOpsClientCore.GitHttpClient.GetPullRequestsAsync(Project, RepositoryId, searchCriteria);
    }

    public Task<GitPullRequest?> GetPullRequestByIdAsync(int id) =>
        AzureDevOpsClientCore.GitHttpClient.GetPullRequestByIdAsync(id);

    public Task<GitPullRequestCommentThread?> GetPullRequestThreadAsync(int pullRequestId, int threadId) =>
        AzureDevOpsClientCore.GitHttpClient.GetPullRequestThreadAsync(Project, RepositoryId, pullRequestId, threadId);

    public Task<Microsoft.TeamFoundation.SourceControl.WebApi.Comment?> GetPullRequestCommentAsync(int pullRequestId, int threadId, int commentId) =>
        AzureDevOpsClientCore.GitHttpClient.GetCommentAsync(RepositoryId, pullRequestId, threadId, commentId);

    public Task<Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models.Comment> GetWorkItemCommentAsync(int workItemId, int commentId) =>
        AzureDevOpsClientCore.WorkItemTrackingHttpClient.GetCommentAsync(Project, workItemId, commentId, false, CommentExpandOptions.All);

    public Task<WorkItem> GetWorkItemByIdAsync(int id) =>
        AzureDevOpsClientCore.WorkItemTrackingHttpClient.GetWorkItemAsync(id, null, null, WorkItemExpand.All);

    public Task<CommentList> GetWorkItemCommentsAsync(int workItemId) =>
        AzureDevOpsClientCore.WorkItemTrackingHttpClient.GetCommentsAsync(Project, workItemId, null, null, false, CommentExpandOptions.All);

    public Task CreatePullRequestCommentAsync(Microsoft.TeamFoundation.SourceControl.WebApi.Comment comment, int pullRequestId, int threadId) =>
        AzureDevOpsClientCore.GitHttpClient.CreateCommentAsync(comment, RepositoryId, pullRequestId, threadId);

    public Task CreateWorkItemCommentAsync(CommentCreate comment, int workItemId) =>
        AzureDevOpsClientCore.WorkItemTrackingHttpClient.AddCommentAsync(comment, Project, workItemId);

    public async Task<Identity> GetIdentityById(string identityId) =>
        await AzureDevOpsClientCore.IdentityHttpClient.ReadIdentityAsync(new Guid(identityId));
}