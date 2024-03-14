using berry.interaction.actions;
using berry.interaction.ai;
using leash.conversations;
using leash.scm.pullRequest;
using leash.utils;

namespace berry.interaction.handlers;

public class PullRequestHandler(IAiContext aiContext, IPullRequestActor pullRequestActor) : IPullRequestHandler
{
    private IAiContext AiContext { get; init; } = aiContext;

    private IPullRequestActor PullRequestActor { get; init; } = pullRequestActor;

    public async Task HandlePullRequestComment(IPullRequest pullRequest, IThread thread, IComment comment)
    {
        var prompt = CreateRespondToPullRequestCommentPrompt(pullRequest, thread, comment);

        var result = await AiContext.InvokePromptAsync(prompt);

        var answer = result.ToString();
        var responseComment = comment.CreateAnswer(answer);

        await PullRequestActor.AnswerPullRequestComment(pullRequest, thread, responseComment);
    }

    private static string CreateRespondToPullRequestCommentPrompt(IPullRequest pullRequest, IThread thread, IComment comment) =>
        @$"
        In a {pullRequest.GetType().Name} pull request, with title '{pullRequest.Title}', a comment was made, where you are mentioned.

        First, let's review the pull request details:

        --- Start of pull request section ---

        Title: '{pullRequest.Title}'

        --- Start description section ---
        {pullRequest.Description}
        --- End description section ---

        --- Start of comment threads section ---
        {CreatePullRequestThreadsCommentsPartOfPrompt(pullRequest, thread)}
        --- End of comment threads section ---

        --- End of pull request section ---

        Know you know all details about the pull request, let's focus on the new comment:

        The new comment did mention you, probably it is a question that you should respond to, please provide a kind useful answer to this comment:
        {comment.Content}
        ";

    private static string CreatePullRequestThreadsCommentsPartOfPrompt(IPullRequest pullRequest, IThread thread)
    {
        var threads = pullRequest.Threads.Remove(thread);
        if (!thread.Comments.Any())
        {
            return string.Empty;
        }

        return pullRequest.Threads.Select(CreatePullRequestThreadCommentsPartOfPrompt).JoinToString();
    }


    private static string CreatePullRequestThreadCommentsPartOfPrompt(IThread thread) =>
        @$"
        --- Start of comment thread section ---
        {thread.Comments.Select(CreatePullRequestThreadCommentPartOfPrompt).JoinToString()}
        --- End of comment thread section ---
        ";

    private static string CreatePullRequestThreadCommentPartOfPrompt(IComment comment) =>
        @$"
        --- Start of comment ---
        {comment.Content}
        --- End of comment ---
        ";
}