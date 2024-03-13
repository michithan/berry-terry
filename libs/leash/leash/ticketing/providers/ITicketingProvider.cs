using leash.conversations;
using leash.ticketing.ticket;

namespace leash.ticketing.providers;

public interface ITicketingProvider
{
    Task<ITicket?> GetTicketByIdAsync(string id);

    Task<IComment?> GetTicketCommentByIdAsync(string ticketId, string commentId);

    Task<string?> CommentOnTicketAsync(ITicket ticket, IThread thread, IComment comment);
}