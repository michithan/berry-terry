using leash.conversations;
using leash.ticketing.providers;
using leash.ticketing.ticket;

namespace berry.interaction.actions;

public class TicketActor(ITicketingProvider ticketingProvider) : ITicketActor
{
    public async Task AnswerTicketComment(ITicket ticket, IThread thread, IComment comment)
    {
        await ticketingProvider.CommentOnTicketAsync(ticket, thread, comment);
    }
}