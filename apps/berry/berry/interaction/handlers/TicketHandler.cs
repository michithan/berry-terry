using berry.interaction.actions;
using berry.interaction.ai;
using leash.conversations;
using leash.ticketing.ticket;

namespace berry.interaction.handlers;

public class TicketHandler(IAiContext AiContext, ITicketActor ticketActor) : ITicketHandler
{
    public async Task HandleTicketComment(ITicket ticket, IThread thread, IComment comment)
    {
        var prompt = @$"
        On the ticket, the following comment was made:
        {comment.Content}
        ";

        var result = await AiContext.InvokePromptAsync(prompt);

        var answer = result.ToString();
        var responseComment = comment.CreateAnswer(answer);

        await ticketActor.AnswerTicketComment(ticket, thread, responseComment);
    }
}