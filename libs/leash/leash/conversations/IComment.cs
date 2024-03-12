namespace leash.conversations;

public interface IComment
{
    public string Id { get; init; }

    public bool IsBotMentioned { get; init; }

    public string Content { get; init; }

    public IComment CreateAnswer(string content);
}