using leash.conversations;
using leash.scm.pullRequest;

namespace leash.scm.provider;

public abstract class ScmProviderBase : IScmProvider
{
    public abstract Task<IEnumerable<IPullRequest>> GetAllPullRequestsAsync();

    public abstract Task<IPullRequest> GetPullRequestByIdAsync(int pullRequestId);

    public abstract Task<IThread> GetPullRequestThreadByIdAsync(int pullRequestId, int threadId);

    public abstract Task<IComment> GetCommentAsync(int pullRequestId, int threadId, int commentId);

    public abstract Task<string?> CommentOnPullRequestThreadAsync(IPullRequest pullRequest, IThread thread, IComment comment);
}