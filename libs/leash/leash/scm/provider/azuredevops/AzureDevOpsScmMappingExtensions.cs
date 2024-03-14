using leash.conversations;
using leash.conversations.provider.azuredevops;
using leash.scm.pullRequest;
using Microsoft.TeamFoundation.SourceControl.WebApi;

namespace leash.scm.provider.azuredevops;

public static class AzureDevOpsScmMappingExtensions
{
    public static IEnumerable<AzureDevOpsPullRequest> MapToAzureDevOpsPullRequest(this IEnumerable<GitPullRequest> pullRequests) =>
        pullRequests.Select(MapToAzureDevOpsPullRequest).ToList();

    public static AzureDevOpsPullRequest MapToAzureDevOpsPullRequest(this GitPullRequest pullRequest) => new()
    {
        Id = pullRequest.PullRequestId.ToString(),
        Title = pullRequest.Title,
        Description = pullRequest.Description,
        SourceBranch = pullRequest.SourceRefName,
        TargetBranch = pullRequest.TargetRefName,
        Url = pullRequest.Url,
        Threads = []
    };

    public static AzureDevOpsThread MapToAzureDevOpsThread(this GitPullRequestCommentThread adoThread, Func<string, bool> isBotMentioned) => new()
    {
        Id = adoThread.Id.ToString(),
        Comments = adoThread.Comments.Select(comment => comment.MapToAzureDevOpsComment(isBotMentioned)).ToList<IComment>()
    };

    public static AzureDevOpsComment MapToAzureDevOpsComment(this Comment comment, Func<string, bool> isBotMentioned) => new()
    {
        Id = comment.Id.ToString(),
        IsBotMentioned = isBotMentioned(comment.Content),
        Content = comment.Content.Replace($"<@{comment.Author.Id}>", $"@{comment.Author.DisplayName}")
    };
}