using leash.scm.pullRequest;
using leash.conversations;
using leash.scm.provider;

namespace berry.interaction.actions;

public class PullRequestActor(IScmProvider ScmProvider) : IPullRequestActor
{
    public async Task AnswerPullRequestComment(IPullRequest pullRequest, IThread thread, IComment comment)
    {
        await ScmProvider.CommentOnPullRequestThreadAsync(pullRequest, thread, comment);
    }
}