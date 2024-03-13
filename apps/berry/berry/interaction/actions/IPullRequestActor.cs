using leash.conversations;
using leash.scm.pullRequest;

namespace berry.interaction.actions;

public interface IPullRequestActor
{
    public Task<string?> AnswerPullRequestComment(IPullRequest pullRequest, IThread thread, IComment comment);
}