using leash.conversations;

namespace leash.ticketing.ticket;

public abstract class TicketBase : ITicket
{
    public required string Title { get; init; }

    public required string Description { get; init; }

    public required string TicketId { get; init; }

    public required string TicketUrl { get; init; }

    public required IThread CommentThread { get; init; }
}
