using leash.clients.azuredevops;
using leash.conversations;
using leash.scm.pullRequest;
using leash.utils;
using Microsoft.TeamFoundation.SourceControl.WebApi;

namespace leash.scm.provider.azuredevops;

public class AzureDevOpsScmProvider : ScmProviderBase, IAzureDevOpsScmProvider
{
    private string Organization { get; init; }

    private string Project { get; init; }

    private string RepositoryId { get; init; }

    IAzureDevOpsClient AzureDevOpsClient { get; init; }

    public AzureDevOpsScmProvider(AzureDevOpsClientConfiguration azureDevOpsConfiguration, IAzureDevOpsClient? connection = null)
    {
        Organization = azureDevOpsConfiguration.Organization;
        Project = azureDevOpsConfiguration.Project;
        RepositoryId = azureDevOpsConfiguration.RepositoryId;

        var organizationUri = new Uri($"https://dev.azure.com/{Organization}");

        AzureDevOpsClient = connection ?? new AzureDevOpsClient(organizationUri, Project, RepositoryId, azureDevOpsConfiguration.Token);
    }

    public override async Task<IEnumerable<IPullRequest>> GetAllPullRequestsAsync()
    {
        var pullRequests = await AzureDevOpsClient.GetAllPullRequestsAsync();
        return pullRequests.MapToAzureDevOpsPullRequest().ToList();
    }

    public override async Task<IPullRequest> GetPullRequestByIdAsync(int id)
    {
        var pullRequest = await AzureDevOpsClient.GetPullRequestByIdAsync(id) ?? throw new Exception($"Pull request with id {id} not found");
        return pullRequest.MapToAzureDevOpsPullRequest();
    }

    public override async Task<IThread> GetPullRequestThreadByIdAsync(int pullRequestId, int threadId)
    {
        var thread = await AzureDevOpsClient.GetPullRequestThreadAsync(pullRequestId, threadId) ?? throw new Exception($"Thread with id {threadId} not found");
        return thread.MapToAzureDevOpsThread(AzureDevOpsClient.IsMentionedOnComment);
    }

    public override async Task CommentOnPullRequestThreadAsync(IPullRequest pullRequest, IThread thread, IComment comment)
    {
        var azureDevOpsComment = new Comment()
        {
            Content = comment.Content
        };
        await AzureDevOpsClient.CreatePullRequestCommentAsync(azureDevOpsComment, pullRequest.Id.ToInt(), thread.Id.ToInt());
    }

    public override async Task<IComment> GetCommentAsync(int pullRequestId, int threadId, int commentId)
    {
        var adoComment = await AzureDevOpsClient.GetPullRequestCommentAsync(pullRequestId, threadId, commentId)
            ?? throw new Exception($"Comment with id {commentId} not found");
        return adoComment.MapToAzureDevOpsComment(AzureDevOpsClient.IsMentionedOnComment);
    }
}