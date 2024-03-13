using leash.conversations;
using leash.ticketing.providers;
using leash.ticketing.ticket;

namespace berry.interaction.actions;

public class TicketActor(ITicketingProvider ticketingProvider) : ITicketActor
{
    private ITicketingProvider TicketingProvider { get; init; } = ticketingProvider;

    public Task<string?> AnswerTicketComment(ITicket ticket, IThread thread, IComment comment) =>
        TicketingProvider.CommentOnTicketAsync(ticket, thread, comment);
}