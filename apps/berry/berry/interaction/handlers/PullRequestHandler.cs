using berry.interaction.actions;
using berry.interaction.ai;
using leash.conversations;
using leash.scm.pullRequest;

namespace berry.interaction.handlers;

public class PullRequestHandler(IAiContext aiContext, IPullRequestActor pullRequestActor) : IPullRequestHandler
{
    private IAiContext AiContext { get; init; } = aiContext;

    private IPullRequestActor PullRequestActor { get; init; } = pullRequestActor;

    public async Task HandlePullRequestComment(IPullRequest pullRequest, IThread thread, IComment comment)
    {
        var prompt = @$"
        On the pull request, the following comment was made:
        {comment.Content}
        ";

        var result = await AiContext.InvokePromptAsync(prompt);

        var answer = result.ToString();
        var responseComment = comment.CreateAnswer(answer);

        await PullRequestActor.AnswerPullRequestComment(pullRequest, thread, responseComment);
    }
}