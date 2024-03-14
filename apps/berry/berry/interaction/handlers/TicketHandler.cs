using berry.interaction.actions;
using berry.interaction.ai;
using leash.conversations;
using leash.ticketing.ticket;

namespace berry.interaction.handlers;

public class TicketHandler(IAiContext aiContext, ITicketActor ticketActor) : ITicketHandler
{
    private IAiContext AiContext { get; init; } = aiContext;

    private ITicketActor TicketActor { get; init; } = ticketActor;

    public async Task<string?> HandleTicketComment(ITicket ticket, IThread thread, IComment comment)
    {
        var prompt = CreateRespondToTicketCommentPrompt(ticket, thread, comment);

        var result = await AiContext.InvokePromptAsync(prompt);

        var answer = result.ToString();
        var responseComment = comment.CreateAnswer(answer);

        return await TicketActor.AnswerTicketComment(ticket, thread, responseComment);
    }

    private static string CreateRespondToTicketCommentPrompt(ITicket ticket, IThread thread, IComment comment) =>
        @$"
        In a {ticket.GetType().Name} ticket, with title '{ticket.Title}', a comment was made, where you are mentioned.

        The ticket title is: '{ticket.Title}'
        The ticket description is: '{ticket.Description}'

        {CreateTicketCommentPartOfPrompt(thread, comment)}

        The new comment did mention you, probably it is a question that you should respond to, please provide a kind useful answer to this comment:
        {comment.Content}
        ";

    private static string CreateTicketCommentPartOfPrompt(IThread thread, IComment comment)
    {
        thread.Comments.Remove(comment); // Remove the comment from the thread so it doesn't get processed again

        if (!thread.Comments.Any())
        {
            return string.Empty;
        }

        return @$"
        The following comments were already made on the ticket:
        ```
        {string.Join("\n```\n", thread.Comments.Select(c => c.Content))}
        ";
    }
}