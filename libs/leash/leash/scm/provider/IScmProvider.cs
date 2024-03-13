using leash.conversations;
using leash.scm.pullRequest;

namespace leash.scm.provider;

public interface IScmProvider
{
    Task<IEnumerable<IPullRequest>> GetAllPullRequestsAsync();

    Task<IPullRequest> GetPullRequestByIdAsync(int pullRequestId);

    Task<IThread> GetPullRequestThreadByIdAsync(int pullRequestId, int threadId);

    Task<IComment> GetCommentAsync(int pullRequestId, int threadId, int commentId);

    Task<string?> CommentOnPullRequestThreadAsync(IPullRequest pullRequest, IThread thread, IComment comment);
}