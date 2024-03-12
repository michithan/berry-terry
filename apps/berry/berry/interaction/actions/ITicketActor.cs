using leash.conversations;
using leash.ticketing.ticket;

namespace berry.interaction.actions;

public interface ITicketActor
{
    public Task AnswerTicketComment(ITicket ticket, IThread thread, IComment comment);
}