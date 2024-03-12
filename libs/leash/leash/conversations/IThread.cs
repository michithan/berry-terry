namespace leash.conversations;

public interface IThread
{
    public string Id { get; init; }

    public IList<IComment> Comments { get; init; }
}