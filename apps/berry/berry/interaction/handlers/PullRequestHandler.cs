using berry.interaction.actions;
using berry.interaction.ai;
using leash.conversations;
using leash.scm.pullRequest;

namespace berry.interaction.handlers;

public class PullRequestHandler(IAiContext AiContext, IPullRequestActor pullRequestActor) : IPullRequestHandler
{
    public async Task HandlePullRequestComment(IPullRequest pullRequest, IThread thread, IComment comment)
    {
        var prompt = @$"
        On the pull request, the following comment was made:
        {comment.Content}
        ";

        var result = await AiContext.InvokePromptAsync(prompt);

        var answer = result.ToString();
        var responseComment = comment.CreateAnswer(answer);

        await pullRequestActor.AnswerPullRequestComment(pullRequest, thread, responseComment);
    }
}