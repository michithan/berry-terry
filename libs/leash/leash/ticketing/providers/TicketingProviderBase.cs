using leash.conversations;
using leash.ticketing.ticket;

namespace leash.ticketing.providers;

public abstract class TicketingProviderBase : ITicketingProvider
{
    public abstract Task<ITicket?> GetTicketByIdAsync(string id);

    public abstract Task<IComment?> GetTicketCommentByIdAsync(string ticketId, string commentId);

    public abstract Task CommentOnTicketAsync(ITicket ticket, IThread thread, IComment comment);
}