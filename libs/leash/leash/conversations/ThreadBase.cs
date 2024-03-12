namespace leash.conversations;

public abstract class ThreadBase : IThread
{
    public required string Id { get; init; }

    public required IList<IComment> Comments { get; init; }
}