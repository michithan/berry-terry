using leash.conversations;
using leash.scm.pullRequest;

namespace berry.interaction.actions;

public interface IPullRequestActor
{
    public Task AnswerPullRequestComment(IPullRequest pullRequest, IThread thread, IComment comment);
}