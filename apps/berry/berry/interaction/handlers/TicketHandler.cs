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
        var prompt = @$"
        On the ticket, the following comment was made:
        {comment.Content}
        ";

        var result = await AiContext.InvokePromptAsync(prompt);

        var answer = result.ToString();
        var responseComment = comment.CreateAnswer(answer);

        return await TicketActor.AnswerTicketComment(ticket, thread, responseComment);
    }
}