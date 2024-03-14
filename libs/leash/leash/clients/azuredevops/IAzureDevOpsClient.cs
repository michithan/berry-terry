using Microsoft.TeamFoundation.SourceControl.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.Identity;

namespace leash.clients.azuredevops;

public interface IAzureDevOpsClient
{
    public Task<IEnumerable<GitPullRequest>> GetAllPullRequestsAsync();

    public Task<GitPullRequest?> GetPullRequestByIdAsync(int id);

    public Task<GitPullRequestCommentThread?> GetPullRequestThreadAsync(int pullRequestId, int threadId);

    public Task<Microsoft.TeamFoundation.SourceControl.WebApi.Comment?> GetPullRequestCommentAsync(int pullRequestId, int threadId, int commentId);

    public Task<Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models.Comment> GetWorkItemCommentAsync(int workItemId, int commentId);

    public Task<WorkItem> GetWorkItemByIdAsync(int id);

    public Task<CommentList> GetWorkItemCommentsAsync(int workItemId);

    public Task CreatePullRequestCommentAsync(Microsoft.TeamFoundation.SourceControl.WebApi.Comment comment, int pullRequestId, int threadId);

    public Task CreateWorkItemCommentAsync(CommentCreate comment, int workItemId);

    public Task<Identity> GetIdentityById(string identityId);
}