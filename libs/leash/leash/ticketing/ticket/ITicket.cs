using leash.conversations;

namespace leash.ticketing.ticket;

public interface ITicket
{
    public string Title { get; init; }

    public string Description { get; init; }

    public string TicketId { get; init; }

    public string TicketUrl { get; init; }

    public IThread CommentThread { get; init; }
}
