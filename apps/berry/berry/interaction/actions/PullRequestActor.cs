using leash.scm.pullRequest;
using leash.conversations;
using leash.scm.provider;

namespace berry.interaction.actions;

public class PullRequestActor(IScmProvider scmProvider) : IPullRequestActor
{
    private IScmProvider ScmProvider { get; init; } = scmProvider;

    public Task<string?> AnswerPullRequestComment(IPullRequest pullRequest, IThread thread, IComment comment) =>
        ScmProvider.CommentOnPullRequestThreadAsync(pullRequest, thread, comment);
}