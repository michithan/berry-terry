using leash.conversations;
using leash.ticketing.ticket;

namespace berry.interaction.handlers;

public interface ITicketHandler
{
    Task<string?> HandleTicketComment(ITicket ticket, IThread thread, IComment comment);
}