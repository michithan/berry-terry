using leash.conversations;
using leash.scm.pullRequest;

namespace berry.interaction.handlers;

public interface IPullRequestHandler
{
    Task HandlePullRequestComment(IPullRequest pullRequest, IThread thread, IComment comment);
}